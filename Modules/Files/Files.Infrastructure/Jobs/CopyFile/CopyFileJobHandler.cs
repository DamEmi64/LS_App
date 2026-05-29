using Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Files.Infrastructure.Jobs.CopyFile
{
    public class CopyFileJobHandler : JobHandler<CopyFileJob>
    {
        public CopyFileJobHandler(IJobContext jobContext) : base(jobContext)
        {
        }

        public override async Task Execute(CopyFileJob request)
        {
            var atr = System.IO.File.GetAttributes(request.Source);

            if ((atr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(request.Destination)))
                {
                    System.IO.Directory.CreateDirectory(request.Destination);
                }

                if (!request.Destination.EndsWith("\\"))
                {
                    request.Destination += "\\";
                }

                var files = Directory.GetFiles(request.Source);
                foreach (var filepath in files)
                {
                    var file = Path.GetFileName(filepath);
                    System.IO.File.Copy(filepath, request.Destination + file);
                }
            }
            else
            {
                System.IO.File.Copy(request.Source, request.Destination);
            }
        }
    }
}
