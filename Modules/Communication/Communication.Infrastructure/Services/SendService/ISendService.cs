using Base;
using Communication.Domain.Entities;
using Communication.Infrastructure.Services.SendService.Models;

namespace Communication.Infrastructure.Services.SendService
{
    public interface ISendService
    {
        Task<string> SendMail(IEnumerable<Email> emails, UserData userData);

        Task<string> GenerateFromTemplate(EmailGenerationModel model, UserData userData);
    }
}