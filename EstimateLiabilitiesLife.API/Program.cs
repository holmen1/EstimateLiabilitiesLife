using System.Text.Json;
using EstimateLiabilitiesLife;
using EstimateLiabilitiesLife.API.Models;
using Microsoft.FSharp.Collections;


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

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);


app.MapGet("/", () => "Hello World!\n");

app.MapGet("/test", () =>
    Results.Json(new Reserving.cashflow(0, 0.0, 55.5), options));

app.MapPost("/cashflows", async (Contract c) =>
{
    var contract = c.InsuranceContract();
    FSharpList<Reserving.cashflow> cf = Reserving.projectCashflows(c.valueDate, contract);

    return Results.Created("/cashflows", cf);
});

app.Run();
