using ITI.Gymunity.FP.Domain.Models;
using ITI.Gymunity.FP.Domain.RepositoiesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Domain
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        TRepo Repository<TEntity, TRepo>()
            where TRepo : IRepository<TEntity>
            where TEntity : BaseEntity;
        Task<int> CompleteAsync();
    }
}
