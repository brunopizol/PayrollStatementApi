using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FolhaDePagamento.Application.Funcionarios.Commands;
using FolhaDePagamento.Application.Funcionarios.DTOs;
using FolhaDePagamento.Application.Funcionarios.Events;
using FolhaDePagamento.Domain.Enums;
using FolhaDePagamento.Infra.Context;
using MediatR;
using PayrollApi.Services.Payroll.Domain.Entities;

namespace FolhaDePagamento.Application.Funcionarios.Handlers
{
    public class GeneratePayrollStatementCommandHandler : IRequestHandler<GeneratePayrollStatementCommand, PayrollStatementDto>
    {
        private readonly MyDbContext _dbContext;
        private readonly IMediator _mediator;       
        private readonly List<PayrollStatementItemDto> _items;

        public GeneratePayrollStatementCommandHandler(MyDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _items = new List<PayrollStatementItemDto>();
        }

        public async Task<PayrollStatementDto> Handle(GeneratePayrollStatementCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await HandleInternal(request, cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception or handle accordingly
                // You might also want to notify the user about the failure
                await _mediator.Publish(new PayrollStatementGenerationFailedEvent(request.EmployeeId, "An error occurred while processing the request."));
                return null; // or throw a more specific exception if needed
            }
        }

        private async Task<PayrollStatementDto> HandleInternal(GeneratePayrollStatementCommand request, CancellationToken cancellationToken)
        {
            var employee = await GetEmployeeAsync(request.EmployeeId);
            if (employee == null)
            {
                await NotifyPayrollStatementGenerationFailed(request.EmployeeId, "Employee not found.");
                return null;
            }

            var payrollStatement = CreatePayrollStatement(employee);

            await NotifyPayrollStatementGenerated(employee.Id, payrollStatement);

            return payrollStatement;
        }

        private async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            return await _dbContext.Employees.FindAsync(employeeId);
        }

        private async Task NotifyPayrollStatementGenerationFailed(int employeeId, string errorMessage)
        {
            await _mediator.Publish(new PayrollStatementGenerationFailedEvent(employeeId, errorMessage));
        }

        private PayrollStatementDto CreatePayrollStatement(Employee employee)
        {
            var deductions = new List<PayrollStatementItemDto>();

            AddEarnings(deductions, "Salary", employee.GrossSalary);
            AddDeductionIfTrue(deductions, "INSS", CalculateInss(employee.GrossSalary) > 0);
            AddDeductionIfTrue(deductions, "Income Tax", CalculateIncomeTax(employee.GrossSalary) > 0);
            AddDeductionIfTrue(deductions, "Dental Plan Discount", employee.DentalPlanDiscount == true);
            AddDeductionIfTrue(deductions, "Health Plan Discount", employee.HealthPlanDiscount == true);
            AddDeductionIfTrue(deductions, "Transportation Voucher Discount", employee.TransportationVoucherDiscount == true && employee.GrossSalary >= 1500, employee.GrossSalary * 0.06);
            AddNonInfluence(deductions, "FGTS", employee.GrossSalary * 0.08);

            var totalDeductions = deductions.Sum(d => d.Amount);

            return new PayrollStatementDto(
                DateTime.Now.ToString("MMMM yyyy"),
                deductions.AsReadOnly(),
                employee.GrossSalary,
                -totalDeductions,
                employee.GrossSalary - totalDeductions
            );
        }



        private List<PayrollStatementItemDto> CalculatePayrollItems(Employee employee)
        {
            var items = new List<PayrollStatementItemDto>();

            AddEarnings(items, "Salary", employee.GrossSalary);
            AddDeductionIfTrue(items, "INSS", CalculateInss(employee.GrossSalary) > 0);
            AddDeductionIfTrue(items, "Income Tax", CalculateIncomeTax(employee.GrossSalary) > 0);
            AddDeductionIfTrue(items, "Dental Plan Discount", employee.DentalPlanDiscount == true);
            AddDeductionIfTrue(items, "Health Plan Discount", employee.HealthPlanDiscount == true);
            AddDeductionIfTrue(items, "Transportation Voucher Discount", employee.TransportationVoucherDiscount == true && employee.GrossSalary >= 1500, employee.GrossSalary * 0.06);
            AddNonInfluence(items, "FGTS", employee.GrossSalary * 0.08);

            return items;
        }

  


        private void AddDeductionIfTrue(List<PayrollStatementItemDto> items, string description, bool condition, double value = 0)
        {
            if (condition)
            {
                AddDeduction(items, description, value);
            }
        }

        private void AddDeduction(List<PayrollStatementItemDto> items, string description, double value)
        {
            items.Add(new PayrollStatementItemDto(
                SalaryType.Deduction.ToString(),
                -value, // negativo para representar dedução
                description,
                SalaryType.Deduction // ou o tipo apropriado para dedução
            ));
        }


        private void AddEarnings(List<PayrollStatementItemDto> items, string description, double value)
        {
            items.Add(new PayrollStatementItemDto(
                SalaryType.Earnings.ToString(),
                value,
                description,
                SalaryType.Earnings // ou o tipo apropriado para ganhos
            ));
        }

        private void AddNonInfluence(List<PayrollStatementItemDto> items, string description, double value)
        {
            items.Add(new PayrollStatementItemDto(
                SalaryType.NonInfluence.ToString(),
                value,
                description,
                SalaryType.NonInfluence // ou o tipo apropriado para itens sem influência
            ));
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

        private async Task NotifyPayrollStatementGenerated(int employeeId, PayrollStatementDto payrollStatement)
        {
            await _mediator.Publish(new PayrollStatementGeneratedEvent(employeeId, payrollStatement));
        }
    }
}
