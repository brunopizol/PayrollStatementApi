using FluentValidation;
using FolhaDePagamento.Application.Funcionarios.DTOs;
using MediatR;
using System;

namespace FolhaDePagamento.Application.Funcionarios.Commands
{
    public class GeneratePayrollStatementCommand : IRequest<PayrollStatementDto>
    {
        public int EmployeeId { get; }

        public GeneratePayrollStatementCommand(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public class Validator : AbstractValidator<GeneratePayrollStatementCommand>
        {
            public Validator()
            {
                RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("EmployeeId must be greater than zero.");
            }
        }

        public static GeneratePayrollStatementCommand Create(int employeeId, IValidator<GeneratePayrollStatementCommand> validator)
        {
            validator.ValidateAndThrow(new GeneratePayrollStatementCommand(employeeId));

            return new GeneratePayrollStatementCommand(employeeId);
        }
    }

    public static class GeneratePayrollStatementCommandFactory
    {
        public static GeneratePayrollStatementCommand Create(int employeeId, IValidator<GeneratePayrollStatementCommand> validator)
        {
            return GeneratePayrollStatementCommand.Create(employeeId, validator);
        }
    }
}
