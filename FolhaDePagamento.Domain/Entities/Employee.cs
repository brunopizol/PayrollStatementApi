using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using FluentValidation;

namespace PayrollApi.Services.Payroll.Domain.Entities
{
    public class Employee
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Document { get; private set; }
        public string Department { get; private set; }
        public double GrossSalary { get; private set; }
        public DateTime HireDate { get; private set; }
        public bool HealthPlanDiscount { get; private set; }
        public bool DentalPlanDiscount { get; private set; }
        public bool TransportationVoucherDiscount { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public string? ErrorMessage { get; private set; }

        [NotMapped]
        [JsonIgnore]
        public bool? AlreadyExists { get; private set; }

        private Employee()
        {
            // Private constructor to enforce the use of the factory method
        }

        public Employee(string firstName, string lastName, string document, string department, double grossSalary,
                        DateTime hireDate, bool healthPlanDiscount, bool dentalPlanDiscount, bool transportationVoucherDiscount)
        {
            FirstName = firstName;
            LastName = lastName;
            Document = document;
            Department = department;
            GrossSalary = grossSalary;
            HireDate = hireDate;
            HealthPlanDiscount = healthPlanDiscount;
            DentalPlanDiscount = dentalPlanDiscount;
            TransportationVoucherDiscount = transportationVoucherDiscount;
            AlreadyExists = false;

            var validationResult = new EmployeeValidation().Validate(this);

            if (!validationResult.IsValid)
            {
                ErrorMessage = string.Join(", ", validationResult.Errors.Select(error => error.ErrorMessage));
            }
        }

        public static Employee Create(string firstName, string lastName, string document, string department, double grossSalary,
                        DateTime hireDate, bool healthPlanDiscount, bool dentalPlanDiscount, bool transportationVoucherDiscount)
        {
            return new Employee(firstName, lastName, document, department, grossSalary, hireDate,
                                healthPlanDiscount, dentalPlanDiscount, transportationVoucherDiscount);
        }

        public void SetAlreadyExists(bool value)
        {
            AlreadyExists = value;
        }

        private class EmployeeValidation : AbstractValidator<Employee>
        {
            public EmployeeValidation()
            {
                RuleFor(e => e.FirstName)
                    .NotEmpty().WithMessage("Employee's first name is required")
                    .MinimumLength(1).WithMessage("First name is too short");

                RuleFor(e => e.Document)
                    .NotEmpty().WithMessage("Employee's document is required")
                    .Must(BeValidCpf).WithMessage("Invalid CPF");
            }
            private bool BeValidCpf(string cpf)
            {
                cpf = cpf.Trim().Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)
                    return false;

                var digits = cpf.Select(c => (int)char.GetNumericValue(c)).ToArray();
                var tempCpf = digits.Take(9).ToArray();

                Func<int[], int, int> calculateDigit = (values, multiplier) =>
                    values.Select((digit, index) => (int)digit * (multiplier - index)).Sum() % 11;

                var remainder1 = 11 - calculateDigit(tempCpf, 10);
                var digit1 = (remainder1 < 10) ? remainder1.ToString() : "0";

                tempCpf = tempCpf.Append(int.Parse(digit1)).ToArray();
                var remainder2 = 11 - calculateDigit(tempCpf, 11);
                var digit2 = (remainder2 < 10) ? remainder2.ToString() : "0";

                return cpf.EndsWith(digit1 + digit2);
            }
        }
    }
}
