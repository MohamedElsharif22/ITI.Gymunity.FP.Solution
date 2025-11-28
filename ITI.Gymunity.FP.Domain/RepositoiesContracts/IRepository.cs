using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain.RepositoiesContracts
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetWithSpecsAsync(ISpecification<TEntity> specs);
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllWithSpecsAsync(ISpecification<TEntity> specs);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> GetCountWithspecsAsync(ISpecification<TEntity> specs);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
