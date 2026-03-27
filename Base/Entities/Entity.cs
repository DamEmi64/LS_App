using System.ComponentModel.DataAnnotations.Schema;

namespace Base
{
    /// <summary>
    ///     Entity base
    /// </summary>
    public class Entity
    {
        /// <summary>
        ///     Primary key - Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        ///     Insert date
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTimeOffset InsDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        ///     Last update date
        /// </summary>
        public DateTimeOffset UpdDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        ///     Insertion editor data
        /// </summary>
        public string? InsBy { get; set; }

        /// <summary>
        ///     Last update editor data
        /// </summary>
        public string? UpdBy { get; set; }
    }
}