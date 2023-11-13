using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Domain.Enums
{
    public enum SalaryType
    {
        
        Deduction,
        NonInfluence, // FGTS is a tax that is only highlighted within the PayrollStatement; there are no changes to the net amount
        Earnings
    }

}
