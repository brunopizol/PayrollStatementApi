using FolhaDePagamento.Application.Funcionarios.Events;
using FolhaDePagamento.Application.Funcionarios.Queries;
using FolhaDePagamento.Infra.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PayrollApi.Services.Payroll.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Handlers
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
    {
        private readonly MyDbContext _dbContext;
        private readonly IMediator _mediator;

        public GetEmployeeByIdQueryHandler(MyDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await GetEmployeeAsync(request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                await NotifyEmployeeNotFound(request.EmployeeId, cancellationToken);
                return null;
            }

            await NotifyEmployeeFound(request.EmployeeId, cancellationToken);
            return employee;
        }

        private async Task<Employee> GetEmployeeAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
        }

        private async Task NotifyEmployeeNotFound(int employeeId, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new EmployeeNotFoundEvent(employeeId), cancellationToken);
        }

        private async Task NotifyEmployeeFound(int employeeId, CancellationToken cancellationToken)
        {
            await _mediator.Publish(new EmployeeFoundEvent(employeeId), cancellationToken);
        }
    }
}
