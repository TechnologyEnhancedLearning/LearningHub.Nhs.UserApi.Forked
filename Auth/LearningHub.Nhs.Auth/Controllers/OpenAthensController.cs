namespace LearningHub.Nhs.Auth.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Services;
    using IdentityServer4.Stores;
    using LearningHub.Nhs.Auth.Configuration;
    using LearningHub.Nhs.Auth.Interfaces;
    using LearningHub.Nhs.Auth.ViewModels;
    using LearningHub.Nhs.Caching;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The open athens controller.
    /// </summary>
    [AllowAnonymous]
    ////[Route("openathens")]
    ////[Authorize]
    public class OpenAthensController : IdentityServerController
    {
        /// <summary>
        /// The client return url key.
        /// </summary>
        private const string ClientReturnUrlKey = "clientReturnUrl";

        /// <summary>
        /// The _oa lh client dict.
        /// </summary>
        private readonly IOptions<OpenAthensLearningHubClientDictionary> oalhClientDict;

        /// <summary>
        /// The config settings.
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<OpenAthensController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAthensController"/> class.
        /// </summary>
        /// <param name="oalhClientDict">
        /// The oalh Client Dict.
        /// </param>
        /// <param name="userService">
        /// The user service.
        /// </param>
        /// <param name="events">
        /// The events.
        /// </param>
        /// <param name="clientStore">
        /// The client store.
        /// </param>
        /// <param name="webSettings">
        /// The web settings.
        /// </param>
        /// <param name="cacheService">
        /// The cache service.
        /// </param>
        /// <param name="config">
        /// The base config settings.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public OpenAthensController(
            IOptions<OpenAthensLearningHubClientDictionary> oalhClientDict,
            IUserService userService,
            IEventService events,
            IClientStore clientStore,
            WebSettings webSettings,
            ICacheService cacheService,
            IConfiguration config,
            ILogger<OpenAthensController> logger)
            : base(userService, events, clientStore, webSettings, cacheService)
        {
            this.oalhClientDict = oalhClientDict;
            this.config = config;
            this.logger = logger;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        public IActionResult Index()
        {
            return this.Content("OpenAthens Index");
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <param name="origin">
        /// The origin.
        /// </param>
        /// <param name="returnUrl">
        /// The return Url.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Returned error.
        /// </exception>
        public async Task Login(string clientId, string origin, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(origin))
            {
                throw new Exception("ClientId or origin are empty.");
            }

            string oalhClient;
            try
            {
                oalhClient = this.oalhClientDict.Value[clientId];
            }
            catch (Exception)
            {
                oalhClient = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(oalhClient))
            {
                throw new Exception("No OA LH client found.");
            }

            if (!$"https://{oalhClient}".StartsWith(origin, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Invalid origin");
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                this.Response.Cookies.Append(
                ClientReturnUrlKey,
                returnUrl,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddMinutes(5),
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                    Secure = true,
                });
            }

            var internalReturnUrl = $"/openathens/oacallback?clientId={clientId}";
            var authProps = new AuthenticationProperties { RedirectUri = internalReturnUrl };
            await this.HttpContext.ChallengeAsync("oidc_oa", authProps);

            // return Content("Route is working");
        }

        //// This uri is subsumed into the OpenID Connect middleware so probably does not need to exist?

        /// <summary>
        /// The oa redirect.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Route("[controller]/oa-redirect")]
        public IActionResult OaRedirect(string returnUrl = "/")
        {
            return this.Content("Openathens has redirected after logon.");
        }

        /// <summary>
        /// The oa call back.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IActionResult> OaCallBack(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception("clientId was empty");
            }

            string clientReturnUrl = string.Empty;
            if (this.Request.Cookies.ContainsKey(ClientReturnUrlKey))
            {
                clientReturnUrl = this.Request.Cookies[ClientReturnUrlKey];
                this.Response.Cookies.Delete(ClientReturnUrlKey);
            }

            var result =
                await this.HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                this.logger.LogError(result?.Failure, "Error with authentication at Openathens.");
                throw new Exception("External authentication error");
            }

            var (provider, providerUserId, claims) = this.FindClaimsFromOpenAthens(result);

            await this.HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            string oaLhClient;
            try
            {
                oaLhClient = this.oalhClientDict.Value[clientId];
            }
            catch (Exception)
            {
                oaLhClient = string.Empty;
            }

            string oaUserId = claims.Where(c => c.Type == "eduPersonTargetedID").Select(c => c.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(oaUserId))
            {
                var user = await this.UserService.GetUserByOAUserIdAsync(oaUserId);
                if (user != null)
                {
                    await this.SignInUser(user.Id, user.UserName, false, null, oaUserId);
                    this.logger.LogInformation("Successful login of user ({userId}) via OpenAthens in Auth service", user.Id);
                    return this.Redirect(string.IsNullOrWhiteSpace(clientReturnUrl) ? $"https://{oaLhClient}" : clientReturnUrl);
                }
            }
            else
            {
                this.logger.LogError("No eduPersonTargetedID received from OpenAthens.");
                throw new Exception("No eduPersonTargetedID received from OpenAthens.");
            }

            var model = new OaCallBackViewModel
            {
                Provider = provider,
                ProviderUserId = providerUserId,
                Claims = claims,
                ClientId = clientId,
            };

            return this.View(model);
        }

        /// <summary>
        /// The find claims from open athens.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="(string, string, IReadOnlyCollection)"/>.
        /// </returns>
        private (string, string, IReadOnlyCollection<Claim>) FindClaimsFromOpenAthens(
            AuthenticateResult result)
        {
            const string eduPersonScopedAffiliationClaim = "eduPersonScopedAffiliation";
            var oaUser = result.Principal;

            var userIdClaim = oaUser.FindFirst(JwtClaimTypes.Subject) ??
                              oaUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown UserId");

            var claims = oaUser.Claims.ToList();
            var scopeList = this.config.GetSection("OaScopes").Get<List<string>>();
            if (claims.All(a => a.Type != eduPersonScopedAffiliationClaim))
            {
                if (claims.Any(a => a.Type == ClaimTypes.NameIdentifier))
                {
                    var nameIdent = claims.Find(f => f.Type == ClaimTypes.NameIdentifier);
                    if (nameIdent != null)
                    {
                        foreach (var scope in scopeList)
                        {
                            if (!nameIdent.Value.Contains(scope, StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }
                            //// We have to make up an eduPersonScopedAffiliation claim so clients
                            //// can check the scope.
                            var eduClaim = new Claim(eduPersonScopedAffiliationClaim, $"member@noscopeissued{scope}");
                            claims.Add(eduClaim);
                            break;
                        }
                    }
                }

                if (claims.All(a => a.Type != eduPersonScopedAffiliationClaim))
                {
                    // We can't find a suitable claim to find scope so now to try looking through all the ones we have
                    Claim eduClaim = null;
                    foreach (var claim in claims)
                    {
                        var circuitBreak = false;
                        foreach (var scope in scopeList)
                        {
                            if (!claim.Value.Contains(scope, StringComparison.CurrentCultureIgnoreCase))
                            {
                                continue;
                            }

                            // We found a scope in a claim!
                            eduClaim = new Claim(eduPersonScopedAffiliationClaim, $"member@noscopeissued{scope}");
                            circuitBreak = true;
                            break;
                        }

                        if (circuitBreak)
                        {
                            break;
                        }
                    }

                    // We can't find a suitable scope, we will issue an out of scope claim for the clients to handle
                    eduClaim = eduClaim ?? new Claim("eduPersonScopedAffiliation", $"member@noscopeissued.noscope.uk");
                    claims.Add(eduClaim);
                }
            }
            else
            {
                if (claims.Any(a => a.Type == eduPersonScopedAffiliationClaim))
                {
                    // scope check for eduPersonScopedAffiliationClaim
                    var eduPerClaim = claims.First(f => f.Type == eduPersonScopedAffiliationClaim);
                    if (!string.IsNullOrWhiteSpace(eduPerClaim.Value))
                    {
                        var jsonArrTest = new Regex(@"\[.*\]", RegexOptions.Singleline);

                        if (!jsonArrTest.IsMatch(eduPerClaim.Value))
                        {
                            // eduPersonScopedAffiliation is a string!
                            foreach (var scope in scopeList)
                            {
                                if (!eduPerClaim.Value.Contains(scope, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    continue;
                                }

                                this.logger.LogTrace(
                                    $"{eduPersonScopedAffiliationClaim} contains a correct scoped claim");
                            }
                        }
                        else
                        {
                            if (jsonArrTest.IsMatch(eduPerClaim.Value))
                            {
                                // eduPersonScopedAffiliation is an array!
                                var circuitBreaker = false;
                                foreach (var scope in scopeList)
                                {
                                    var jsonArr = JsonSerializer.Deserialize<List<string>>(eduPerClaim.Value);
                                    foreach (var claimScope in jsonArr)
                                    {
                                        if (!claimScope.Contains(scope))
                                        {
                                            continue;
                                        }

                                        this.logger.LogTrace(
                                            $"{eduPersonScopedAffiliationClaim} contains a correct scoped claim");
                                        claims.Remove(eduPerClaim);
                                        eduPerClaim = new Claim(eduPersonScopedAffiliationClaim, claimScope);
                                        claims.Add(eduPerClaim);
                                        circuitBreaker = true;
                                        break;
                                    }

                                    if (circuitBreaker)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            claims.Remove(userIdClaim);

            var provider = result.Properties.Items[".AuthScheme"];
            var providerUserId = userIdClaim.Value;

            return (provider, providerUserId, claims);
        }
    }
}
