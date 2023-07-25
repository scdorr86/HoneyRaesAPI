using HoneyRaesAPI.Models;

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
List<Employee> employees = new List<Employee>
{
    new Employee()
    {
        Id = 1,
        Name = "Employee 1",
        Specialty = "Bugs"
    },
    new Employee()
    {
        Id = 2,
        Name = "Employee 2",
        Specialty = "HTML"
    },
    new Employee()
    {
        Id = 3,
        Name = "Employee 3",
        Specialty = "C#"
    },
};
List<ServiceTicket> serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket()
    {
        Id=1,
        CustomerId=1,
        EmployeeId=0,
        Description="Ticket 1",
        Emergency=false,
        DateCompleted=new DateTime()
    },
    new ServiceTicket()
    {
        Id=2,
        CustomerId=2,
        EmployeeId=0,
        Description="Ticket 2",
        Emergency=false,
        DateCompleted=new DateTime()
    },
    new ServiceTicket()
    {
        Id=3,
        CustomerId=3,
        EmployeeId=3,
        Description="Ticket 3",
        Emergency=true,
        DateCompleted=new DateTime()
    },
    new ServiceTicket()
    {
        Id=4,
        CustomerId=1,
        EmployeeId=2,
        Description="Ticket 4",
        Emergency=false,
        DateCompleted=new DateTime(2023,05,31)
    },
    new ServiceTicket()
    {
        Id=5,
        CustomerId=2,
        EmployeeId=3,
        Description="Ticket 5",
        Emergency=true,
        DateCompleted=new DateTime(2023,02,14)
    }
};


var builder = WebApplication.CreateBuilder(args);

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/servicetickets", () =>
{
    return serviceTickets;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
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
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(customer);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

