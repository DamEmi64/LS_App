using Base;
using Newtonsoft.Json;
using RPG.Domain.Dictionaries;
using RPG.Infrastructure.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG.Infrastructure.Services
{
    public class RPGAutomationResolver : IAutomationResolver
    {
        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks)
        {
            var currentSchema = schema;
            foreach (var task in tasks)
            {
                if (task.Operation == Operations.GetLastRPG)
                {
                    var genJob = new GetLastEditedRPGJob();
                    currentSchema = currentSchema.AddJob(genJob);
                    task.Handled = true;
                }
                else if (task.Operation == Operations.GenerateSummary)
                {
                    var job = new GenerateSummaryJob();
                    JsonConvert.PopulateObject(task.JsonData ?? string.Empty, job);
                    currentSchema = currentSchema.AddJob(job);
                    task.Handled = true;
                }
            }
        }
    }
}
