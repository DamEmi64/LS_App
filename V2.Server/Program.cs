using Api;

var builder = WebApplication.CreateBuilder(args);

var build = new Startup(builder);

var app = build.Build(builder);

app.Run();