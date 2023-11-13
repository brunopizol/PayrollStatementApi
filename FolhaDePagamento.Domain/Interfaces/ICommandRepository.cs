using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Domain.Interfaces
{
    public interface ICommandRepository<T>
    {
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task AddAsync(T entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
