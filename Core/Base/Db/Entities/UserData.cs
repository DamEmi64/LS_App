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

        public static UserData System =>
            new()
            {
                Id = 0,
                UserId = Guid.Empty.ToString(),
                Login = "System",
                Email = "system@example.com"
            };
    }
}