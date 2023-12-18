using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var webHost = builder.WebHost;

{
    var publicHttpPort = configuration.GetValue<int?>("PublicApi:HttpPort");
    var publicHttpsPort = configuration.GetValue<int?>("PublicApi:HttpsPort");
    var publicApiPorts = new List<string>();
    if (publicHttpPort != null) publicApiPorts.Add($"http://+:{publicHttpPort}");
    if (publicHttpsPort != null) publicApiPorts.Add($"https://+:{publicHttpsPort}");
    if (!publicApiPorts.Any()) throw new Exception();

    var allPorts = publicApiPorts.ToArray();
    var allPortsUnique = publicApiPorts.ToHashSet();
    if (allPorts.Length != allPortsUnique.Count) throw new Exception();

    webHost.UseUrls(string.Join(";", allPorts));
}

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
    .AddIdentityServerAuthentication(
        authenticationScheme: IdentityServerAuthenticationDefaults.AuthenticationScheme,
        jwtBearerOptions: options =>
        {
            options.Audience = "beam";
            var authority = configuration.GetValue<string>("Denji:PublicApi:BaseUrl") ?? throw new Exception();
            options.Authority = authority;
            options.RequireHttpsMetadata = false;
            var issuers = configuration.GetSection("DenjiMiddleware:IssuerUrl").Get<string[]>() ?? throw new Exception();
            options.TokenValidationParameters.ValidIssuers = issuers;
        },
        introspectionOptions: null
    );

services
    .AddAuthorization(options =>
        options.AddPolicy(
            name: "read",
            configurePolicy: b => b.RequireScope("beam.read")
        )
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();