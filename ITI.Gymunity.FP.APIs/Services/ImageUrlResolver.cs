using ITI.Gymunity.FP.Application.Contracts.ExternalServices;
using Microsoft.Extensions.Configuration;

namespace ITI.Gymunity.FP.APIs.Services
{
 public class ImageUrlResolver : IImageUrlResolver
 {
 private readonly IConfiguration _config;

 public ImageUrlResolver(IConfiguration config)
 {
 _config = config;
 }

 public string ResolveImageUrl(string url)
 {
 if (string.IsNullOrWhiteSpace(url))
 return url;

 if (url.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) ||
 url.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase) ||
 url.StartsWith("//"))
 {
 return url;
 }

 var baseUrl = _config["Assets:BaseUrl"] ?? _config["BaseApiUrl"];
 if (string.IsNullOrWhiteSpace(baseUrl))
 return url;

 return baseUrl.TrimEnd('/') + "/" + url.TrimStart('/');
 }
 }
}
