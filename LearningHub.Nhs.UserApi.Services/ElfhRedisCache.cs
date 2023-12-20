// <copyright file="ElfhRedisCache.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services
{
    using LearningHub.Nhs.UserApi.Services.Interface;
    using Microsoft.Extensions.Caching.Redis;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// ELFH Redis Cache.
    /// </summary>
    public class ElfhRedisCache : RedisCache, IElfhRedisCache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElfhRedisCache"/> class.
        /// </summary>
        /// <param name="optionsAccessor">ELFH Redis Cache Options.</param>
        public ElfhRedisCache(IOptions<RedisCacheOptions> optionsAccessor)
            : base(optionsAccessor)
        {
        }
    }
}
