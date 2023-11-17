using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class EmployeeCreatedEvent
    {
        public Employee CreatedEmployee { get; }

        public EmployeeCreatedEvent(Employee createdEmployee)
        {
            CreatedEmployee = createdEmployee ?? throw new ArgumentNullException(nameof(createdEmployee));
        }
    }

}
