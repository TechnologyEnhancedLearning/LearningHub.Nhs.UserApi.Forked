// <copyright file="ITimezoneOffsetManager.cs" company="HEE.nhs.uk">
// Copyright (c) HEE.nhs.uk.
// </copyright>

namespace LearningHub.Nhs.UserApi.Repository.Interface.LH
{
    /// <summary>
    /// The TimezoneOffsetManager interface.
    /// </summary>
    public interface ITimezoneOffsetManager
    {
        /// <summary>
        /// Gets User Timezone Offset.
        /// </summary>
        int? UserTimezoneOffset { get; }
    }
}
