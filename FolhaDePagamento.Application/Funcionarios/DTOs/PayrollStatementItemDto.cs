using FolhaDePagamento.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.DTOs
{
    public class PayrollStatementItemDto
    {
        public string Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }

        public SalaryType SalaryType { get; set; }
    }
}
