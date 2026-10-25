using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace System.Infrastructure.Filters
{
    public class BasicDashboardFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context) => true;
    }
}
