using HoneyRaesAPI.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


List<Customer> customers = new List<Customer>
{
    new Customer()
    {
        Id = 1,
        Name = "Customer 1",
        Address = "123 cusomter 1 way"
    },
    new Customer()
    {
        Id = 2,
        Name = "Customer 2",
        Address = "123 cusomter 2 way"
    },
    new Customer()
    {
        Id = 3,
        Name = "Customer 3",
        Address = "123 cusomter 3 way"
    },
    new Customer()
    {
        Id = 4,
        Name = "Customer 4",
        Address = "123 cusomter 4 way"
    }

};
List<Employee> employees = new List<Employee>()
{
    new Employee
    {
        Id = 1,
        Name = "Employee 1",
        Specialty = "Bugs"
    },
    new Employee
    {
        Id = 2,
        Name = "Employee 2",
        Specialty = "HTML"
    },
    new Employee
    {
        Id = 3,
        Name = "Employee 3",
        Specialty = "C#"
    },
     new Employee
    {
        Id = 4,
        Name = "Employee 4",
        Specialty = "Utility"
    },
};
List<ServiceTicket> serviceTickets = new List<ServiceTicket>()
{
    new ServiceTicket
    {
        Id=1,
        CustomerId=1,
        EmployeeId=0,
        Description="Ticket 1",
        Emergency=false,
        DateCompleted= DateTime.MinValue
    },
    new ServiceTicket
    {
        Id=2,
        CustomerId=2,
        EmployeeId=0,
        Description="Ticket 2",
        Emergency=false,
        DateCompleted= DateTime.MinValue
    },
    new ServiceTicket
    {
        Id=3,
        CustomerId=3,
        EmployeeId=3,
        Description="Ticket 3",
        Emergency=true,
        DateCompleted= DateTime.MinValue
    },
    new ServiceTicket
    {
        Id=4,
        CustomerId=1,
        EmployeeId=2,
        Description="Ticket 4",
        Emergency=false,
        DateCompleted=new DateTime(2023,05,31)
    },
    new ServiceTicket
    {
        Id=5,
        CustomerId=2,
        EmployeeId=3,
        Description="Ticket 5",
        Emergency=true,
        DateCompleted=new DateTime(2023,02,14)
    },
    new ServiceTicket
    {
        Id=6,
        CustomerId=3,
        EmployeeId=3,
        Description="Ticket 6",
        Emergency=false,
        DateCompleted= DateTime.MinValue
    },
    new ServiceTicket 
    {   Id=7,
        CustomerId=2,
        EmployeeId=1,
        Description="Ticket 7",
        Emergency=false,
        DateCompleted=new DateTime(2021,07,31)
    },
    new ServiceTicket
    {   Id=8,
        CustomerId=4,
        EmployeeId=1,
        Description="Ticket 8",
        Emergency=false,
        DateCompleted=new DateTime(2020,01,31)
    },
    new ServiceTicket
    {   Id=9,
        CustomerId=4,
        EmployeeId=2,
        Description="Ticket 9",
        Emergency=false,
        DateCompleted=new DateTime(2023,08,02)
    },
    new ServiceTicket
    {   Id=10,
        CustomerId=4,
        EmployeeId=1,
        Description="Ticket 10",
        Emergency=false,
        DateCompleted=new DateTime(2023,08,02)
    },
    new ServiceTicket
    {   Id=11,
        CustomerId=4,
        EmployeeId=1,
        Description="Ticket 11",
        Emergency=false,
        DateCompleted=new DateTime(2023,08,01)
    },
};

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/servicetickets", () =>
{
    return serviceTickets;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    foreach (var t in serviceTickets)
    {
        t.Employee = null;
    }
    foreach (var e in employees)
    {
        e.ServiceTickets = null;
    }
    foreach (var c in customers)
    {
        c.ServiceTickets = null;
    }
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    serviceTicket.Customer = customers.FirstOrDefault(e => e.Id == serviceTicket.CustomerId);
    return Results.Ok(serviceTicket);
});

app.MapGet("/employees", () =>
{
    return employees;
});

app.MapGet("/customers", () =>
{
    return customers;
});

app.MapGet("/employees/{id}", (int id) =>
{
    foreach (var t in serviceTickets)
    {
        t.Employee = null;
    }
    foreach (var e in employees)
    {
        e.ServiceTickets = null;
    }
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    employee.ServiceTickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(employee);
});

app.MapGet("/customers/{id}", (int id) =>
{
    foreach (var t in serviceTickets)
    {
        t.Customer = null;
    }
    foreach (var c in customers)
    {
        c.ServiceTickets = null;
    }
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    customer.ServiceTickets = serviceTickets.Where(st => st.CustomerId == id).ToList();
    return Results.Ok(customer);
});

app.MapPost("/employees", (Employee employee) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    employee.Id = employees.Max(e => e.Id) + 1;
    employees.Add(employee);
    return employees;
});

app.MapPost("/customers", (Customer customer) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    customer.Id = customers.Max(c => c.Id) + 1;
    customers.Add(customer);
    return customers;
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);
    return serviceTicket;
});

app.MapDelete("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    serviceTickets.Remove(serviceTicket);
    return Results.Ok(serviceTickets);
});

app.MapDelete("/employees/{id}", (int id) =>
{
    Employee employeeToDelete = employees.FirstOrDefault(e => e.Id == id);
    employees.Remove(employeeToDelete);
    return Results.Ok(employees);
});

app.MapDelete("/customers/{id}", (int id) =>
{
    Customer customerToDelete = customers.FirstOrDefault(c => c.Id == id);
    customers.Remove(customerToDelete);
    return Results.Ok(customers);
});

app.MapPut("/employees/{id}", (int id, Employee employee) =>
{
    Employee employeeToUpdate = employees.FirstOrDefault(st => st.Id == id);
    int employeeIndex = employees.IndexOf(employeeToUpdate);
    if (employeeToUpdate == null)
    {
        return Results.NotFound();
    }
    //the id in the request route doesn't match the id from the ticket in the request body. That's a bad request!
    if (id != employee.Id)
    {
        return Results.BadRequest();
    }
    employees[employeeIndex] = employee;
    return Results.Ok(employees);
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault(st => st.Id == id);
    int ticketIndex = serviceTickets.IndexOf(ticketToUpdate);
    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    //the id in the request route doesn't match the id from the ticket in the request body. That's a bad request!
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }
    serviceTickets[ticketIndex] = serviceTicket;
    return Results.Ok(serviceTickets);
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Now;
});

app.MapGet("/servicetickets/emergencies", () =>
{
    List<ServiceTicket> emergencies = serviceTickets.Where(st => st.Emergency == true && st.DateCompleted == DateTime.MinValue).ToList();
    return emergencies;
});

app.MapGet("/servicetickets/unassigned", () =>
{
    List<ServiceTicket> unassigned = serviceTickets.Where(st => st.EmployeeId == 0).ToList();
    return unassigned;
});

app.MapGet("/employees/available", () =>
{
    List<ServiceTicket> incomplete = serviceTickets.Where(st => st.DateCompleted == DateTime.MinValue).ToList();
    List<Employee> available = employees.Where(e => !incomplete.Exists(item => item.EmployeeId == e.Id)).ToList();
    return available;
});

app.MapGet("/employees/{empId}/customers", (int empId) =>
{
    List<ServiceTicket> employeesTickets = serviceTickets.Where(st => st.EmployeeId == empId).ToList();
    List<int> custIds = employeesTickets.Select(et => et.CustomerId).ToList();
    return customers.Where(c => custIds.Contains(c.Id)).ToList();
});

app.MapGet("/servicetickets/review", () =>
{
    List<ServiceTicket> pastReview = serviceTickets.Where(st => st.DateCompleted != DateTime.MinValue).OrderBy(st => st.DateCompleted).ToList();
    return pastReview;
});

app.MapGet("/servicetickets/priority", () =>
{
    List<ServiceTicket> priority = serviceTickets.Where(st => st.DateCompleted == DateTime.MinValue).OrderByDescending(st => st.Emergency).ThenBy(st => st.EmployeeId).ToList();
    return priority;
});

app.MapGet("/servicetickets/empofmonth", () =>
{
    var currentMonth = DateTime.Now.Month;
    List<ServiceTicket> currentMonthTix = serviceTickets.Where(st => st.DateCompleted.Month == currentMonth).ToList();
    var empCurrentTix = currentMonthTix.GroupBy(tick => tick.EmployeeId)
    .Select(tick => new { empId = tick.Key, count = tick.Count() })
    .OrderByDescending(resultOfSelect => resultOfSelect.count)
    .FirstOrDefault();
   
    return employees.Where(e => empCurrentTix.empId == e.Id);
});

app.MapGet("/servicetickets/inactive", () =>
{
    var customerTix = serviceTickets.GroupBy(ticket => ticket.CustomerId).ToList();
    var custIds = customerTix
    .Select(custTick => new {mostRecentDate = custTick.Max(ticket => ticket.DateCompleted), customerId = custTick.Key }) //select gives me the specified data that i want or need back
    .Where(resultOfSelect => resultOfSelect.mostRecentDate.AddYears(1) < DateTime.Now && resultOfSelect.mostRecentDate != DateTime.MinValue) // where gives me the data that is true based on a condition i set
    .Select(resultofWhere => resultofWhere.customerId)
    .ToList();
    return customers.Where(c => custIds.Contains(c.Id)).ToList(); 
});

app.Run();




