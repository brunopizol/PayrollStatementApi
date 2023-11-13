using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.DTOs
{
    public class PayrollStatementDto
    {
        public string ReferenceMonth { get; set; }
        public List<PayrollStatementItemDto> Items { get; set; }
        public double GrossSalary { get; set; }
        public double TotalDeductions { get; set; }
        public double NetSalary { get; set; }
    }
}
