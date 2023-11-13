using MediatR;
using PayrollApi.Services.Payroll.Domain.Entities;

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
}
