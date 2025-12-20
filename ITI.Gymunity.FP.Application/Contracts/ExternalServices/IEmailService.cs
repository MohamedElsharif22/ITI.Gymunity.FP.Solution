using ITI.Gymunity.FP.Infrastructure.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Contracts.ExternalServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
        Task SendBulkEmailAsync(List<EmailRequest> requests);
    }
}
