using System.Text.Json;
using EstimateLiabilitiesLife;
using EstimateLiabilitiesLife.API.Models;


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

app.MapPost("/reserve", async (Contract c) =>
{
    var contract = c.InsuranceContract();
    var fta = Reserving.technicalProvision(c.valueDate, contract);

    var reserve = new ReserveResult
    {
        contractNo = c.contractNo,
        valueDate = c.valueDate,
        pvTechnicalProvision = fta
    };
    return Results.Created($"/reserve", reserve);
});

app.Run();
