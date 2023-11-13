using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class PayrollStatementGenerationFailedEvent
    {
        public int EmployeeId { get; }
        public string ErrorMessage { get; }

        public PayrollStatementGenerationFailedEvent(int employeeId, string errorMessage)
        {
            EmployeeId = employeeId;
            ErrorMessage = errorMessage;
        }
    }

}
