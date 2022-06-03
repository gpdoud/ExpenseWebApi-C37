using ExpenseWebApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ExpenseDbContext>(x => {
    x.UseSqlServer(builder.Configuration.GetConnectionString("DevWin"));
});

builder.Services.AddCors();
//builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
//{
//    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//}));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//app.UseCors("corsapp");

app.UseAuthorization();

app.MapControllers();

app.Run();
