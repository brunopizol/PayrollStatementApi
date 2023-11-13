using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Domain.Interfaces
{
    public interface IEmployeesContext
    {
        DbSet<Employee> Employees { get; }
        Task<int> SaveChangesAsync();
    }
}
