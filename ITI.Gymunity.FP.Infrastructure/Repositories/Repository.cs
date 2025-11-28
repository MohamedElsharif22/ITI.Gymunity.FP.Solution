using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using ITI.Gymunity.FP.Domain.Specification;
using ITI.Gymunity.FP.Infrastructure._Data;
using ITI.Gymunity.FP.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Infrastructure.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _Context = context;

        public void Add(T entity) => _Context.Add(entity);

        public void Update(T entity) => _Context.Update(entity);

        public void Delete(T entity) => _Context.Remove(entity);

        public async Task<IEnumerable<T>> GetAllWithSpecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).ToListAsync();


        public async Task<int> GetCountWithspecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).CountAsync();


        public async Task<T?> GetWithSpecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).FirstOrDefaultAsync();


        public async Task<T?> GetByIdAsync(int id)
            => await _Context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _Context.Set<T>().ToListAsync();

        protected IQueryable<T> ApplySpecifications(ISpecification<T> specs)
            => SpecificationEvaluator<T>.BuildQuery(_Context.Set<T>(), specs);

    }
}
