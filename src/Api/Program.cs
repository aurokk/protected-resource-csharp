using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(options =>
    {
        var authority = configuration.GetValue<string>("Auth:BaseUrl") ?? throw new Exception();
        options.Authority = authority;
        options.ApiName = "protected-resource";
        options.RequireHttpsMetadata = false;
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