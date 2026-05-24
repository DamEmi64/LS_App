var builder = WebApplication.CreateBuilder(args);

var build = Connector.Main.InitializeConnector(builder);

var app = build.Build();

app.Run();