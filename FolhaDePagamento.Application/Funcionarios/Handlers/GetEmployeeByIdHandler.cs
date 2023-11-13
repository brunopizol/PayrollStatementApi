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
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

            if (employee == null)
            {
                await _mediator.Publish(new EmployeeNotFoundEvent(request.EmployeeId), cancellationToken);
                return null;
            }

            await _mediator.Publish(new EmployeeFoundEvent(request.EmployeeId), cancellationToken);
            return employee;
        }
    }
}
