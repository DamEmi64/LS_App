namespace Base
{
    /// <summary>
    ///     Information about permissions
    /// </summary>
    public class PermissionInfo
    {
        /// <summary>
        ///     Permission key
        /// </summary>
        public required string Key { get; set; }

        /// <summary>
        ///     Permission description
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        ///     Is basic permission (for user role)
        /// </summary>
        public bool IsBasic { get; set; } = true;

        /// <summary>
        ///     Create permission
        /// </summary>
        /// <param name="key">permission key</param>
        /// <param name="description">permission description</param>
        /// <param name="isBasic">Is available for all users</param>
        /// <returns></returns>
        public static PermissionInfo Create(string key, string description, bool isBasic = true)
            => new() { Key = key, Description = description, IsBasic = isBasic };
    }
}