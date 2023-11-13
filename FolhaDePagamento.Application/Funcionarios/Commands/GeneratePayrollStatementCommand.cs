using FolhaDePagamento.Application.Funcionarios.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Commands
{
    public class GeneratePayrollStatementCommand : IRequest<PayrollStatementDto>
    {
        public int EmployeeId { get; set; }
    }
}
