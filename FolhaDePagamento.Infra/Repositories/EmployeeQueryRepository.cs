using FolhaDePagamento.Domain.Interfaces;
using FolhaDePagamento.Infra.Context;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Infra.Repositories
{
    public class QueryEmployeeRepository : IQueryRepository<Employee>
    {
        private readonly MyDbContext dbContext;

        public QueryEmployeeRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Employee> GetById(int id)
        {
            return await dbContext.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await dbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetByDocumentAsync(string document)
        {
            return await dbContext.Employees.FirstOrDefaultAsync(e => e.Document == document);
        }
    }
}
