using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace System.Application
{
    public class DashboardAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
