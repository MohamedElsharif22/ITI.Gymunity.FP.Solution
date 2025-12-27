using ITI.Gymunity.FP.Domain.Models.ProgramAggregate;
using ITI.Gymunity.FP.Domain.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Specefications
{
 public class ProgramWithTrainerSpec : BaseSpecification<Program>
 {
 public ProgramWithTrainerSpec(string? searchTerm = null)
 {
 AddInclude(q => q.Include(p => p.TrainerProfile).ThenInclude(tp => tp.User));

 if (!string.IsNullOrEmpty(searchTerm))
 {
 Criteria = p => p.Title.Contains(searchTerm) ||
 (p.TrainerProfile != null && p.TrainerProfile.User.FullName.Contains(searchTerm)) ||
 (p.TrainerProfile != null && p.TrainerProfile.Handle.Contains(searchTerm));
 }

 AddOrderByDesc(p => p.CreatedAt);
 ApplyPagination(0,50);
 }
 }
}
