using ITI.Gymunity.FP.Application.DTOs.Program;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Services
{
 public interface IProgramImportService
 {
 Task ImportAiResultAsync(int programId, ProgramAiResult aiResult);
 }
}
