using FolhaDePagamento.Domain.Enums;
using System;

namespace FolhaDePagamento.Application.Funcionarios.DTOs
{
    public class PayrollStatementItemDto
    {
        public string Type { get; }
        public double Amount { get; }
        public string Description { get; }
        public SalaryType SalaryType { get; }

        public PayrollStatementItemDto(
            string type,
            double amount,
            string description,
            SalaryType salaryType)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Amount = amount;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            SalaryType = salaryType;
        }
    }
}
