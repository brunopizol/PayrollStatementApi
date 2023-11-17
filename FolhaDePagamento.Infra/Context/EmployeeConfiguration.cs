using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolhaDePagamento.Infra.Context
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).HasMaxLength(50);
            builder.Property(e => e.LastName).HasMaxLength(50);
            builder.Property(e => e.Document).HasMaxLength(20);
            builder.Property(e => e.Department).HasMaxLength(50);
            builder.Property(e => e.GrossSalary).HasPrecision(18, 2);
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.HealthPlanDiscount).IsRequired();
            builder.Property(e => e.DentalPlanDiscount).IsRequired();
            builder.Property(e => e.TransportationVoucherDiscount).IsRequired();
        }
    }
}
