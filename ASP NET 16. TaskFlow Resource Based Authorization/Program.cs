using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Data;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Extensions;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Middleware;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Services;
using FluentValidation.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger()
            .AddTaskFlowDbContext(builder.Configuration)
            .AddIdentityAndDb(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddCorsPolicy()
            .AddFluentValidation()
            .AddAuthentication();


var app = builder.Build();

app.UseTaskFlowPiple();
await app.EnsureRolesSeededAsync();

app.Run();
