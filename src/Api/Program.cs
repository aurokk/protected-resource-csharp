using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = "https://localhost:20000";
        options.ApiName = "protected-resource";
    });

services
    .AddAuthorization(options =>
        options.AddPolicy(
            name: "read",
            configurePolicy: b => b.RequireScope("protected-resource.read")
        )
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();