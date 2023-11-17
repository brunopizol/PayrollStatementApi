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
            try
            {
                return await HandleInternal(request, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception or handle accordingly
                // You might also want to notify the user about the failure
                await _mediator.Publish(new EmployeeCreationFailedEvent(request, "An error occurred while processing the request."));
                return null; // or throw a more specific exception if needed
            }
        }

        private async Task<Employee> HandleInternal(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await CheckIfEmployeeExistsAsync(request.Document);
            if (existingEmployee != null)
            {
                await NotifyEmployeeCreationFailed(request, "Employee with the same document already exists.");
                existingEmployee.SetAlreadyExists(true);
                return existingEmployee;
            }

            var newEmployee = CreateNewEmployee(request);

            if (!string.IsNullOrWhiteSpace(newEmployee.ErrorMessage))
            {
                await NotifyEmployeeCreationFailed(request, newEmployee.ErrorMessage);
                return newEmployee;
            }

            await SaveNewEmployeeAsync(newEmployee, cancellationToken);

            await NotifyEmployeeCreated(newEmployee);

            return newEmployee;
        }
        private async Task<Employee> CheckIfEmployeeExistsAsync(string document)
        {
            return await _queryRepository.GetByDocumentAsync(document);
        }

        private async Task NotifyEmployeeCreationFailed(CreateEmployeeCommand request, string errorMessage)
        {
            await _mediator.Publish(new EmployeeCreationFailedEvent(request, errorMessage));
        }

        private Employee CreateNewEmployee(CreateEmployeeCommand request)
        {
            return new Employee(
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
        }

        private async Task SaveNewEmployeeAsync(Employee newEmployee, CancellationToken cancellationToken)
        {
            await _commandRepository.AddAsync(newEmployee);
            await _commandRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task NotifyEmployeeCreated(Employee newEmployee)
        {
            await _mediator.Publish(new EmployeeCreatedEvent(newEmployee));
        }


    }
}
