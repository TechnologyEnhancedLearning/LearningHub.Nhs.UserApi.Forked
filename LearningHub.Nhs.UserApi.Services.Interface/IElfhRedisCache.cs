// <copyright file="IElfhRedisCache.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Services.Interface
{
    using Microsoft.Extensions.Caching.Distributed;

    /// <summary>
    /// Interface for ELFH Redis Cache.
    /// </summary>
    public interface IElfhRedisCache : IDistributedCache
    {
    }
}
