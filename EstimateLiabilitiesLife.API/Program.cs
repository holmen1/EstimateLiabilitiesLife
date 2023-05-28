using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using EstimateLiabilitiesLife;
using EstimateLiabilitiesLife.API.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ReservingDb>(opt =>
    opt.UseInMemoryDatabase("ReserveList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

app.MapGet("/reserves", async (ReservingDb db) =>
    await db.Reserves.ToListAsync());

app.MapGet("/reserves/{contractNo}", async (int contractNo, ReservingDb db) =>
    await db.Reserves.FindAsync(contractNo)
        is ReserveResult r
        ? Results.Ok(r)
        : Results.NotFound());

app.MapPost("/reserves", async (Contract c, ReservingDb db) =>
{
    var contract = c.InsuranceContract();
    var fta = Reserving.technicalProvision(c.valueDate, contract);

    var reserve = new ReserveResult()
    {
        contractNo = c.contractNo,
        valueDate = c.valueDate,
        pvTechnicalProvision = fta
    };

    db.Reserves.Add(reserve);
    await db.SaveChangesAsync();

    return Results.Created($"/reserve/{c.contractNo}", reserve);
});


app.Run();


internal class ReservingDb : DbContext
{
    public ReservingDb(DbContextOptions<ReservingDb> options)
        : base(options)
    {
    }

    public DbSet<ReserveResult> Reserves => Set<ReserveResult>();
}