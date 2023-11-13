using FolhaDePagamento.Application.Funcionarios.Commands;
using FolhaDePagamento.Application.Funcionarios.Events;
using FolhaDePagamento.Domain.Interfaces;
using MediatR;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Employee>
    {
        private readonly ICommandRepository<Employee> _commandRepository;
        private readonly IQueryRepository<Employee> _queryRepository;
        private readonly IMediator _mediator;

        public CreateEmployeeCommandHandler(
            ICommandRepository<Employee> commandRepository,
            IQueryRepository<Employee> queryRepository,
            IMediator mediator)
        {
            _commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await _queryRepository.GetByDocumentAsync(request.Document);
            if (existingEmployee != null)
            {
                await _mediator.Publish(new EmployeeCreationFailedEvent(request, "Employee with the same document already exists."));
                existingEmployee.SetAlreadyExists(true);
                return existingEmployee;
            }

            var newEmployee = new Employee(
                request.FirstName,
                request.LastName,
                request.Document,
                request.Department,
                request.GrossSalary,
                request.HireDate,
                request.HasHealthPlanDiscount,
                request.HasDentalPlanDiscount,
                request.HasTransportationVoucherDiscount
            );

            if (!string.IsNullOrWhiteSpace(newEmployee.ErrorMessage))
            {
                await _mediator.Publish(new EmployeeCreationFailedEvent(request, newEmployee.ErrorMessage));
                return newEmployee;
            }

            await _commandRepository.AddAsync(newEmployee);
            await _commandRepository.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new EmployeeCreatedEvent(newEmployee));

            return newEmployee;
        }
    }
}
