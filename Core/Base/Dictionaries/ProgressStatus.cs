using System.ComponentModel.DataAnnotations;

namespace Base
{
    /// <summary>
    ///     Process/Job progress status
    /// </summary>
    public enum ProgressStatus
    {
        [Display(Name = "New")]
        New,

        [Display(Name = "Executing")]
        Executing,

        [Display(Name = "Success")]
        Success,

        [Display(Name = "Failed")]
        Failed,

        [Display(Name = "Paused")]
        Paused,

        [Display(Name = "Cancelled")]
        Cancelled
    }
}