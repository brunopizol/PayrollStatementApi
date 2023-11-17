using FluentValidation;
using FolhaDePagamento.Application.Funcionarios.Validators;
using FolhaDePagamento.Domain.Interfaces;
using MediatR;
using PayrollApi.Services.Payroll.Domain.Entities;
using System;

public class CreateEmployeeCommand : IRequest<Employee>
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Document { get; }
    public string Department { get; }
    public double GrossSalary { get; }
    public DateTime HireDate { get; }
    public bool HasHealthPlanDiscount { get; }
    public bool HasDentalPlanDiscount { get; }
    public bool HasTransportationVoucherDiscount { get; }

    public CreateEmployeeCommand(
       string firstName,
       string lastName,
       string document,
       string department,
       double grossSalary,
       DateTime hireDate,
       bool hasHealthPlanDiscount,
       bool hasDentalPlanDiscount,
       bool hasTransportationVoucherDiscount)
    {
        FirstName = firstName;
        LastName = lastName;
        Document = document;
        Department = department;
        GrossSalary = grossSalary;
        HireDate = hireDate;
        HasHealthPlanDiscount = hasHealthPlanDiscount;
        HasDentalPlanDiscount = hasDentalPlanDiscount;
        HasTransportationVoucherDiscount = hasTransportationVoucherDiscount;
    }
    public static CreateEmployeeCommand Create(
       string firstName,
       string lastName,
       string document,
       string department,
       double grossSalary,
       DateTime hireDate,
       bool hasHealthPlanDiscount,
       bool hasDentalPlanDiscount,
       bool hasTransportationVoucherDiscount)
    {
        var validator = new CreateEmployeeCommandValidator();
        validator.ValidateAndThrow(new CreateEmployeeCommand(
            firstName,
            lastName,
            document,
            department,
            grossSalary,
            hireDate,
            hasHealthPlanDiscount,
            hasDentalPlanDiscount,
            hasTransportationVoucherDiscount));

        return new CreateEmployeeCommand(
            firstName,
            lastName,
            document,
            department,
            grossSalary,
            hireDate,
            hasHealthPlanDiscount,
            hasDentalPlanDiscount,
            hasTransportationVoucherDiscount);
    }


}
