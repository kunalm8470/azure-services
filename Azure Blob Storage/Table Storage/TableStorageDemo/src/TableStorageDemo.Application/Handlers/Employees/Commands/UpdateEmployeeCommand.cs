﻿using Azure;
using MediatR;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Application.Handlers.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest<Response>
    {
        public Employee Employee { get; set; }
    }
}