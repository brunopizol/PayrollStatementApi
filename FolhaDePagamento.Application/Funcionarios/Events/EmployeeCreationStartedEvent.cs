using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class EmployeeCreationStartedEvent
    {
        public DateTime CreationStartTime { get; }

        public EmployeeCreationStartedEvent(DateTime creationStartTime)
        {
            CreationStartTime = creationStartTime;
        }
    }
}
