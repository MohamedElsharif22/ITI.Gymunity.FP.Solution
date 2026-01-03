using ITI.Gymunity.FP.Application.DTOs.Program;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IAiProgramService
 {
 Task<ProgramAiResult> GenerateProgramAsync(ProgramAiCreateRequest request);
 }
}