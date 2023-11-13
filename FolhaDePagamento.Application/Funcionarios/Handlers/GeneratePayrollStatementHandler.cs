using FolhaDePagamento.Application.Funcionarios.Commands;
using FolhaDePagamento.Application.Funcionarios.DTOs;
using FolhaDePagamento.Application.Funcionarios.Events;
using FolhaDePagamento.Domain.Enums;
using FolhaDePagamento.Infra.Context;
using MediatR;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Handlers
{
    public class GeneratePayrollStatementCommandHandler : IRequestHandler<GeneratePayrollStatementCommand, PayrollStatementDto>
    {
        private readonly MyDbContext _dbContext;
        private readonly IMediator _mediator;
        private double _totalDeductions = 0;
        private readonly List<PayrollStatementItemDto> _items;

        public GeneratePayrollStatementCommandHandler(MyDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _items = new List<PayrollStatementItemDto>();
        }

        public async Task<PayrollStatementDto> Handle(GeneratePayrollStatementCommand request, CancellationToken cancellationToken)
        {
            var employee = await _dbContext.Employees.FindAsync(request.EmployeeId);
            if (employee == null)
            {
                await _mediator.Publish(new PayrollStatementGenerationFailedEvent(request.EmployeeId, "Employee not found."));
                return null;
            }

            CalculateNetAmount(employee);

            var payrollStatement = new PayrollStatementDto
            {
                ReferenceMonth = DateTime.Now.ToString("MMMM yyyy"),
                Items = _items,
                GrossSalary = employee.GrossSalary,
                TotalDeductions = -_totalDeductions,
                NetSalary = employee.GrossSalary - _totalDeductions
            };

            await _mediator.Publish(new PayrollStatementGeneratedEvent(employee.Id,payrollStatement));

            return payrollStatement;
        }

        private void CalculateNetAmount(Employee employee)
        {
            AddEarnings("Salary", employee.GrossSalary);
            AddDeductionIfTrue("INSS", CalculateInss(employee.GrossSalary) > 0);
            AddDeductionIfTrue("Income Tax", CalculateIncomeTax(employee.GrossSalary) > 0);
            AddDeductionIfTrue("Dental Plan Discount", employee.DentalPlanDiscount == true);
            AddDeductionIfTrue("Health Plan Discount", employee.HealthPlanDiscount == true);
            AddDeductionIfTrue("Transportation Voucher Discount", employee.TransportationVoucherDiscount == true && employee.GrossSalary >= 1500, employee.GrossSalary * 0.06);
            AddNonInfluence("FGTS", employee.GrossSalary * 0.08);
        }

        private void AddDeductionIfTrue(string description, bool condition, double value = 0)
        {
            if (condition)
            {
                AddDeduction(description, value);
            }
        }

        private void AddDeduction(string description, double value)
        {
            _totalDeductions += value;
            _items.Add(new PayrollStatementItemDto
            {
                Type = SalaryType.Deduction.ToString(),
                Amount = -value,
                Description = description
            });
        }

        private void AddEarnings(string description, double value)
        {
            _items.Add(new PayrollStatementItemDto
            {
                Type = SalaryType.Earnings.ToString(),
                Amount = value,
                Description = description
            });
        }

        private void AddNonInfluence(string description, double value)
        {
            _items.Add(new PayrollStatementItemDto
            {
                Type = SalaryType.NonInfluence.ToString(),
                Amount = value,
                Description = description
            });
        }

        private double CalculateInss(double grossSalary)
        {
            var brackets = new List<(double Limit, double Rate)>
            {
                (1045.00, 0.075),
                (2089.60, 0.09),
                (3134.40, 0.12),
                (double.MaxValue, 0.14)
            };

            return brackets.FirstOrDefault(bracket => grossSalary <= bracket.Limit).Rate * grossSalary;
        }

        private double CalculateIncomeTax(double baseSalary)
        {
            List<(double Bracket, double Rate, double Ceiling)> brackets = new List<(double, double, double)>
            {
                (1903.98, 0, 0),
                (2826.65, 0.075, 142.8),
                (3751.05, 0.15, 354.8),
                (4664.68, 0.225, 636.13),
                (double.MaxValue, 0.275, 869.36)
            };

            return Math.Min(baseSalary *
                        brackets.First(bracket => baseSalary <= bracket.Bracket).Rate,
                        brackets.First(bracket => baseSalary <= bracket.Bracket).Ceiling);
        }
    }
}
