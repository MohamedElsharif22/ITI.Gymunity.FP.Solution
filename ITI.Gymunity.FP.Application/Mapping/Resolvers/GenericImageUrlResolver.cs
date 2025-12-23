using AutoMapper;
using ITI.Gymunity.FP.Application.Contracts.ExternalServices;

namespace ITI.Gymunity.FP.Application.Mapping.Resolvers
{
 // Generic resolver to map any string URL field to resolved absolute URL
 public class GenericImageUrlResolver<TSource, TDestination> : IMemberValueResolver<TSource, TDestination, string?, string?>
 {
 private readonly IImageUrlResolver _imageUrlResolver;

 public GenericImageUrlResolver(IImageUrlResolver imageUrlResolver)
 {
 _imageUrlResolver = imageUrlResolver;
 }

 public string? Resolve(TSource source, TDestination destination, string? sourceMember, string? destMember, ResolutionContext context)
 {
 if (string.IsNullOrWhiteSpace(sourceMember))
 return sourceMember;

 return _imageUrlResolver.ResolveImageUrl(sourceMember);
 }
 }
}
