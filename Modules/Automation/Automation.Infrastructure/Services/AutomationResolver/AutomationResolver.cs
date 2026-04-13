using Automation.Domain.Dictionaries;
using Automation.Infrastructure.Jobs;
using Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Automation.Infrastructure.Services
{
    public class AutomationResolver : IAutomationResolver
    {
        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks)
        {
            var currentSchema = schema;
            foreach(var task in tasks)
            {
                if (task.Operation == Operations.ArchiveData)
                {
                    var job = new ArchiveJob { SourceDir = string.Empty, DestDir = string.Empty };
                    JsonConvert.PopulateObject(task.JsonData ?? string.Empty, job);
                    currentSchema = currentSchema.AddJob(job);
                    task.Handled = true;
                }
            }
        }
    }
}
