using System;
using System.Threading.Tasks;
using FolhaDePagamento.Application.Funcionarios.Commands;
using FolhaDePagamento.Application.Funcionarios.DTOs;
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
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employeeDto)
        {
            try
            {
                var command = MapToCreateEmployeeCommand(employeeDto);
                var result = await _mediator.Send(command);

                return HandleEmployeeCreationResult(result);
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
                var result = await _mediator.Send(new GetEmployeeByIdQuery(employeeId));

                return HandleEmployeeResult(new PayrollStatementDto
                {
                  
                }, "Employee not found, please review the provided ID.");
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
                var command = new GeneratePayrollStatementCommand(employeeId);
                var result = await _mediator.Send(command);

                return HandleEmployeeResult(result, "Employee not found, please review the provided ID.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        private CreateEmployeeCommand MapToCreateEmployeeCommand(Employee employeeDto)
        {
            return new CreateEmployeeCommand(
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
        }

        private IActionResult HandleEmployeeCreationResult(Employee result)
        {
            {
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
        }

        private IActionResult HandleEmployeeResult(PayrollStatementDto result, string errorMessage)
        {
            if (result == null)
            {
                return BadRequest(new { Message = errorMessage });
            }

            return Ok(result);
        }
    }
    }
