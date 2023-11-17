using FolhaDePagamento.Domain.Interfaces;
using FolhaDePagamento.Infra.Context;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FolhaDePagamento.Infra.Repositories
{
    public class QueryEmployeeRepository : IQueryRepository<Employee>
    {
        private readonly MyDbContext _dbContext;

        public QueryEmployeeRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee> GetById(int id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetByDocumentAsync(string document)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Document == document);
        }

        public static class EmployeeRepositoryFactory
        {
            public static IQueryRepository<Employee> CreateQueryRepository(MyDbContext dbContext)
            {
                return new QueryEmployeeRepository(dbContext);
            }
        }
    }
}
