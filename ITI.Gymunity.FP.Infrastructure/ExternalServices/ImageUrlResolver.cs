using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.ExternalServices
{
    public class ImageUrlResolver(IConfiguration configuration): IImageUrlResolver
    {
        private readonly IConfiguration _configuration = configuration;

        public string ResolveImageUrl(string url)
        {
            var baseUrl = _configuration["BaseApiUrl"];
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return url;
            }
            else
            {
                return $"{baseUrl?.TrimEnd('/')}/{url.TrimStart('/')}";
            }
        }
    }
}
