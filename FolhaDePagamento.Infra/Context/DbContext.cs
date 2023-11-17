using FolhaDePagamento.Domain.Interfaces;
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
    public class MyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        private readonly ICommandRepository<Employee> _commandRepository;
        private readonly IQueryRepository<Employee> _queryRepository;

        public MyDbContext(DbContextOptions<MyDbContext> options,
                      ICommandRepository<Employee> commandRepository,
                       IQueryRepository<Employee> queryRepository)
        : base(options)
        {
            _commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
            _queryRepository = queryRepository ?? throw new ArgumentNullException(nameof(queryRepository));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            
        }

     
    }

}
