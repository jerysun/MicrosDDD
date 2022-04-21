using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserMgr.Domain;
using UserMgr.Infra;
using UserMgr.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MvcOptions>(o =>
{
  o.Filters.Add<UnitOfWorkFilter>();
});
builder.Services.AddDbContext<UserDbContext>(o =>
{
 o.UseSqlServer("Server=.;Database=MicrosDDDDB;integrated security=True;Encrypt=False;MultipleActiveResultSets=True");
});
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISmsCodeSender, MockSmsCodeSender>();
builder.Services.AddDistributedMemoryCache(); // In product environment, Redis alike must be used instead.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
