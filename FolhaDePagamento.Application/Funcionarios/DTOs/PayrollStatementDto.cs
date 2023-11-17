using System;
using System.Collections.Generic;

namespace FolhaDePagamento.Application.Funcionarios.DTOs
{
    public class PayrollStatementDto
    {
        public string ReferenceMonth { get; }
        public IReadOnlyList<PayrollStatementItemDto> Items { get; }
        public double GrossSalary { get; }
        public double TotalDeductions { get; }
        public double NetSalary { get; }

        public PayrollStatementDto(
            string referenceMonth,
            IReadOnlyList<PayrollStatementItemDto> items,
            double grossSalary,
            double totalDeductions,
            double netSalary)
        {
            ReferenceMonth = referenceMonth ?? throw new ArgumentNullException(nameof(referenceMonth));
            Items = items ?? throw new ArgumentNullException(nameof(items));
            GrossSalary = grossSalary;
            TotalDeductions = totalDeductions;
            NetSalary = netSalary;
        }
    }
}
