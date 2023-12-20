// <copyright file="CachingService.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Services
{
    using System;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    using LearningHub.Nhs.Models.Common.Interfaces;

    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// The caching service.
    /// </summary>
    /// <typeparam name="T">Input type.</typeparam>
    public class CachingService<T> : ICachingService<T>
    {
        /// <summary>
        /// The cache.
        /// </summary>
        private readonly IDistributedCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingService{T}"/> class.
        /// </summary>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public CachingService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// The get async.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<T> GetAsync(string key, Func<Task<T>> action)
        {
            T t = default(T);
            string json = string.Empty;
            var cacheValue = await this.cache.GetAsync(key).ConfigureAwait(false);
            if (cacheValue != null)
            {
                json = Encoding.UTF8.GetString(cacheValue);
            }
            //// From cache
            if (!string.IsNullOrEmpty(json))
            {
                t = JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                // Else from function delegate
                if (action != null)
                {
                    t = await action().ConfigureAwait(false);
                }

                if (t != null)
                {
                    // Add to cache
                    string serializedEntity = JsonSerializer.Serialize(t);
                    var options =
                        new DistributedCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                            };
                    await this.cache.SetAsync(key, Encoding.UTF8.GetBytes(serializedEntity), options).ConfigureAwait(false);
                }
            }

            return t;
        }

        /// <summary>
        /// The remove async.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task RemoveAsync(string key)
        {
            await this.cache.RemoveAsync(key).ConfigureAwait(false);
        }
    }
}
