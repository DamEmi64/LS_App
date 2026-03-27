using Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();

IoC.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

app.Setup(builder.Configuration);

app.Run();