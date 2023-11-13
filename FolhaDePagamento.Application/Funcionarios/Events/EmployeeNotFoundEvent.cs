using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class EmployeeNotFoundEvent : INotification
    {
        public int EmployeeId { get; }

        public EmployeeNotFoundEvent(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
