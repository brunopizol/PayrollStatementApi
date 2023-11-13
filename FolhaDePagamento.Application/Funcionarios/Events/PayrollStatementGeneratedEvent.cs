using FolhaDePagamento.Application.Funcionarios.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class PayrollStatementGeneratedEvent
    {
        public int EmployeeId { get; }
        public PayrollStatementDto PayrollStatement { get; }

        public PayrollStatementGeneratedEvent(int employeeId, PayrollStatementDto payrollStatement)
        {
            EmployeeId = employeeId;
            PayrollStatement = payrollStatement;
        }
    }

}
