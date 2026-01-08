using ITI.Gymunity.FP.Application.DTOs.User.Subscribe;
using System.Collections.Generic;

namespace ITI.Gymunity.FP.Application.DTOs.Trainer
{
 public class PackageWithSubscriptionsResponse
 {
 public PackageResponse Package { get; set; } = null!;
 public List<SubscriptionResponse> Subscriptions { get; set; } = new List<SubscriptionResponse>();
 }
}
