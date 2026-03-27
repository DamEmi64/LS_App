namespace Base
{
    /// <summary>
    ///     User data
    /// </summary>
    public class UserData
    {
        /// <summary>
        ///     User Data id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     User Id (from Identity)
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        ///     User login
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        ///     User email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        ///     User role
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        ///     User permissions
        /// </summary>
        public string[] Permissions { get; set; } = Array.Empty<string>();

        public UserData Clone()
            => new()
            {
                Id = 0,
                UserId = UserId,
                Login = Login,
                Email = Email,
                Role = Role,
                Permissions = Permissions.ToArray()
            };
    }

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