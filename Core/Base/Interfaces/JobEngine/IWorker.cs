using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    /// <summary>
    ///    Background worker that run in cycles via hangfire recurring job
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        ///     Schedule cron expression for hangfire recurring job
        /// </summary>
        string Cron { get; }

        /// <summary>
        /// Worker method
        /// </summary>
        void Execute();
    }
}
