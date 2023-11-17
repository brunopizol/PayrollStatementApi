using FolhaDePagamento.Domain.Interfaces;
using FolhaDePagamento.Infra.Context;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;

public class CommandEmployeeRepository : ICommandRepository<Employee>, IDisposable
{
    private readonly MyDbContext _dbContext;

    public CommandEmployeeRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public void Insert(Employee entity)
    {
        _dbContext.Employees.Add(entity);
    }

    public void Update(Employee entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(Employee entity)
    {
        _dbContext.Employees.Remove(entity);
    }

    public async Task AddAsync(Employee entity)
    {
        await _dbContext.Employees.AddAsync(entity);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
