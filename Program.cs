using eSusFarm.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IUssdRegistrationService, UssdRegistrationService>();

var app = builder.Build();

app.MapControllers();

app.Run();
