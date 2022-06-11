using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TableStorageDemo.API.Middlewares;
using TableStorageDemo.API.Models.Requests;
using TableStorageDemo.Application.Handlers.Employees.Commands;
using TableStorageDemo.Application.Handlers.Employees.Queries;
using TableStorageDemo.Domain.Entities;
using TableStorageDemo.Domain.Exceptions;

namespace TableStorageDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Employee>> GetEmployee([FromQuery] string partition_key, [FromQuery] string row_key)
        {
            Employee? found = await _mediator.Send(new GetEmployeeByPartitionKeyAndRowKeyQuery
            {
                PartitionKey = partition_key,
                RowKey = row_key
            });

            if (found == default)
            {
                throw new ResourceNotFoundException();
            }

            return Ok(found);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] AddEmployee employee)
        {
            Guid employeeId = Guid.NewGuid();

            Response created = await _mediator.Send(new CreateEmployeeCommand
            {
                Employee = new()
                {
                    Id = employeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Department = employee.Department,
                    PartitionKey = employee.Department,
                    RowKey = employeeId.ToString()
                }
            });

            created.Headers.TryGetValue("Location", out string locationHeader);

            return Created(locationHeader, default);
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee([FromBody] UpdateEmployee employee)
        {
            Response updated = await _mediator.Send(new UpdateEmployeeCommand
            {
                Employee = new()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    Department = employee.Department,
                    PartitionKey = employee.PartitionKey,
                    RowKey = employee.RowKey,
                    ETag = new ETag(employee.Etag)
                }
            });

            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee([FromBody] DeleteEmployeeQuery employeeQuery)
        {
            await _mediator.Send(new DeleteEmployeeCommand
            {
                PartitionKey = employeeQuery.PartitionKey,
                RowKey = employeeQuery.RowKey,
                ETag = employeeQuery.Etag
            });

            return NoContent();
        }
    }
}
