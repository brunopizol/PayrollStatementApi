using FluentValidation;
using FolhaDePagamento.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name cannot be empty.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name cannot be empty.");
            RuleFor(x => x.Document).NotEmpty().WithMessage("Document cannot be empty.");
            RuleFor(x => x.Department).NotEmpty().WithMessage("Department cannot be empty.");
            RuleFor(x => x.GrossSalary).GreaterThan(0).WithMessage("Gross salary must be greater than zero.");
            RuleFor(x => x.HireDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Hire date cannot be in the future.");
            // Adicione mais regras conforme necessário
        }
    }
}
