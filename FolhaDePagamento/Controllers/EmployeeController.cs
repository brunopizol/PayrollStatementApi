using FolhaDePagamento.Application.Funcionarios.Commands;
using FolhaDePagamento.Application.Funcionarios.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PayrollApi.Services.Payroll.Domain.Entities;

namespace FolhaDePagamento.Presentation.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class EmployeeController : ControllerBase
        {
            private readonly IMediator _mediator;

            public EmployeeController(IMediator mediator)
            {
                _mediator = mediator;
            }

        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employeeDto)
        {
            try
            {
                var command = new CreateEmployeeCommand(
                    firstName: employeeDto.FirstName,
                    lastName: employeeDto.LastName,
                    document: employeeDto.Document,
                    department: employeeDto.Department,
                    grossSalary: employeeDto.GrossSalary,
                    hireDate: employeeDto.HireDate,
                    hasHealthPlanDiscount: employeeDto.HealthPlanDiscount,
                    hasDentalPlanDiscount: employeeDto.DentalPlanDiscount,
                    hasTransportationVoucherDiscount: employeeDto.TransportationVoucherDiscount
                );

                var result = await _mediator.Send(command);
                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    return BadRequest(new { Message = "Failed to create employee", ErrorMessage = result.ErrorMessage });
                }

                if (result.AlreadyExists == true)
                {
                    return Ok(new { Message = "Employee already exists, please check the data.", Employee = result });
                }

                return Ok(new { Message = "Employee created successfully", Employee = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
            [Route("GetEmployeeById/{employeeId}")]
            public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
            {
                try
                {
                    var query = new GetEmployeeByIdQuery(employeeId);
                    var result = await _mediator.Send(query);

                    if (result == null)
                    {
                        return BadRequest(new { Message = "Employee not found, please review the provided ID." });
                    }

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }

            [HttpGet]
            [Route("GeneratePayrollStatement/{employeeId}")]
            public async Task<IActionResult> GeneratePayrollStatement(int employeeId)
            {
                try
                {
                    var query = new GeneratePayrollStatementCommand { EmployeeId = employeeId };
                    var result = await _mediator.Send(query);

                    if (result != null)
                    {
                        return Ok(result);
                    }

                    return BadRequest(new { Message = "Employee not found, please review the provided ID." });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }
        }
    }


