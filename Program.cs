using HoneyRaesAPI.Models;
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
        DateCompleted=new DateTime()
    },
    new ServiceTicket
    {
        Id=2,
        CustomerId=2,
        EmployeeId=0,
        Description="Ticket 2",
        Emergency=false,
        DateCompleted=new DateTime()
    },
    new ServiceTicket
    {
        Id=3,
        CustomerId=3,
        EmployeeId=3,
        Description="Ticket 3",
        Emergency=true,
        DateCompleted= new DateTime()
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
    }
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
    return Results.Ok();
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Now;
});

app.MapGet("/servicetickets/emergencies", () =>
{
    List<ServiceTicket> emergencies = serviceTickets.Where(st => st.Emergency == true && st.DateCompleted == new DateTime()).ToList();
    return emergencies;
});

app.MapGet("/servicetickets/unassigned", () =>
{
    List<ServiceTicket> unassigned = serviceTickets.Where(st => st.EmployeeId == 0).ToList();
    return unassigned;
});

app.MapGet("/employees/available", () =>
{
    List<ServiceTicket> incomplete = serviceTickets.Where(st => st.DateCompleted == new DateTime()).ToList();
    // List<Employee> available = employees.Where(e => e.Id != incomplete.EmpoloyeeId).ToList();
    //return available;
});

app.Run();




