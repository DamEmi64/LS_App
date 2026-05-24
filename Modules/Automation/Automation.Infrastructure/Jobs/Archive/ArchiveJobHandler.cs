using Base;
using Base.Job;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Automation.Infrastructure.Jobs.Archive
{
    public class ArchiveJobHandler : JobHandler<ArchiveJob>
    {
        public ArchiveJobHandler(IJobContext jobContext) : base(jobContext)
        {
        }

        public override Task Execute(ArchiveJob request)
        {
            if (File.Exists(request.DestDir))
            {
                File.Delete(request.DestDir);
            }

            // Create the zip archive
            ZipFile.CreateFromDirectory(request.SourceDir, request.DestDir, CompressionLevel.Optimal, includeBaseDirectory: true);
        }
    }
}
