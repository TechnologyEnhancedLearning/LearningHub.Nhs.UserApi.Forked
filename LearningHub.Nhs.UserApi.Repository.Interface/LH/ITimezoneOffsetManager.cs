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
