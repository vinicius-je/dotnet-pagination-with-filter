using Microsoft.EntityFrameworkCore;
using StockApi.Infrastructure.Configuration;
using StockApi.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Database Condiguration
builder.Services.ConfigureDbExtension(builder.Configuration);

// CORS, Allow Any Origin for test
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

var app = builder.Build();
CreateDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

void CreateDatabase(WebApplication app)
{
    var scoped = app.Services.CreateScope();
    var context = scoped.ServiceProvider.GetService<AppDbContext>();
    context?.Database.EnsureCreated();

    var sqlFile = "./Scripts/inserts.sql";
    var sql = File.ReadAllText(sqlFile);
    context?.Database.ExecuteSqlRaw(sql);
}
