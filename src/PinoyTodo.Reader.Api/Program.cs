using PinoyTodo.Reader.Application;
using PinoyTodo.Reader.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalApplication();
builder.Services.AddLocalInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
