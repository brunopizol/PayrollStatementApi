using FolhaDePagamento.Application.Funcionarios.DTOs;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class PayrollStatementGeneratedEvent
    {
        public int EmployeeId { get; }
        public PayrollStatementDto PayrollStatement { get; }

        public PayrollStatementGeneratedEvent(int employeeId, PayrollStatementDto payrollStatement)
        {
            EmployeeId = employeeId;
            PayrollStatement = payrollStatement ?? throw new ArgumentNullException(nameof(payrollStatement));
        }
    }
}
