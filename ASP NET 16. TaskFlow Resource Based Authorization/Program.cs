using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger()
                .AddFluentValidation()
                .AddTaskFlowDbContext(builder.Configuration)
                .AddIdentityAndDb(builder.Configuration)
                .AddJwtAuthenticationAndAuthorization(builder.Configuration)
                .AddCorsPolicy()
                .AddAutoMapperAndOtherServices();

var app = builder.Build();

app.UseTaskFlowPiplene();

await app.EnsureRolesSeededAsync();

app.Run();
