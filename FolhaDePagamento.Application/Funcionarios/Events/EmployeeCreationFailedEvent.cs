using FolhaDePagamento.Application.Funcionarios.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class EmployeeCreationFailedEvent
    {
        public CreateEmployeeCommand CreateEmployeeCommand { get; }
        public string ErrorMessage { get; }

        public EmployeeCreationFailedEvent(CreateEmployeeCommand createEmployeeCommand, string errorMessage)
        {
            CreateEmployeeCommand = createEmployeeCommand;
            ErrorMessage = errorMessage;
        }
    }

}
