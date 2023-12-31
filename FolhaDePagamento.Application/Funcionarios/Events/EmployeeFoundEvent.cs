﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Application.Funcionarios.Events
{
    public class EmployeeFoundEvent : INotification
    {
        public int EmployeeId { get; }

        public EmployeeFoundEvent(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
