using Base;

namespace Files.Infrastructure.Jobs.UploadFile
{
    internal class UploadFileJobHandler : JobHandler<UploadFileJob>
    {
        public UploadFileJobHandler(IJobContext jobContext) : base(jobContext)
        {
        }

        public override async Task Execute(UploadFileJob request)
        {
            var byteArray = Convert.FromBase64String(request.FileData);

            var path = request.Locaction;

            bool exists = Directory.Exists(path);

            if (!exists)
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllBytesAsync(path, byteArray);
        }
    }
}
