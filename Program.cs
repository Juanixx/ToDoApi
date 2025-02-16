var builder = WebApplication.CreateBuilder(args);

// Add DI - Add methods

var app = builder.Build();

//Configure pipeline - Use method...

app.MapGet("/", () => "Hello World!");

app.Run();
