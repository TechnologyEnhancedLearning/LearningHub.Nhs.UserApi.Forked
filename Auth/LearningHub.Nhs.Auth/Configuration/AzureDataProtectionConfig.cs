// <copyright file="AzureDataProtectionConfig.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.Auth.Configuration
{
    /// <summary>
    /// The Azure Data Protection config.
    /// </summary>
    public class AzureDataProtectionConfig
    {
        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the storage container name.
        /// </summary>
        public string StorageContainerName { get; set; }

        /// <summary>
        /// Gets or sets the vault key identifier.
        /// </summary>
        public string VaultKeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the tenant id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}