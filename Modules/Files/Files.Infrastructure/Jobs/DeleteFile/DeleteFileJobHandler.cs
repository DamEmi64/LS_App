using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Files.Infrastructure.Jobs.DeleteFile
{
    public class DeleteFileJobHandler : JobHandler<DeleteFileJob>
    {
        public DeleteFileJobHandler(IJobContext jobContext) : base(jobContext)
        {
        }

        public override Task Execute(DeleteFileJob request)
        {
            var atr = File.GetAttributes(request.Locaction);

            if ((atr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Directory.Delete(request.Locaction, true);
            }
            else
            {
                File.Delete(request.Locaction);
            }

            return Task.CompletedTask;
        }
    }
}
