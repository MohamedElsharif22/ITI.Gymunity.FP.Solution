using ITI.Gymunity.FP.Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Contracts.ExternalServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
        Task SendBulkEmailAsync(List<EmailRequest> requests);
    }
}
