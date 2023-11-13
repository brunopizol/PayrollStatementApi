using FolhaDePagamento.Domain.Interfaces;
using FolhaDePagamento.Infra.Context;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

public class CommandEmployeeRepository : ICommandRepository<Employee>
{
    private readonly MyDbContext dbContext;

    public CommandEmployeeRepository(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Insert(Employee entity)
    {
        dbContext.Employees.Add(entity);
        dbContext.SaveChanges();
    }

    public void Update(Employee entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.SaveChanges();
    }

    public void Delete(Employee entity)
    {
        dbContext.Employees.Remove(entity);
        dbContext.SaveChanges();
    }

    public async Task AddAsync(Employee entity)
    {
        await dbContext.Employees.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
