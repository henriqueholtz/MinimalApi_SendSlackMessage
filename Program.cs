using MinimalApi_SendSlackMessage.Model;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (IConfiguration Configuration) =>
{
    return new { 
        Message = "Welcome to SendSlackMessage - The Minimal Api from dotnet 6!", 
        OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
        Test = Configuration["test"]
    };
});

app.MapPost("/send", (Message message) =>
{
    Response response = Request.SendMessage(message);
    if (response != null && !response.Error) return Results.Ok(response.Message);

    return Results.BadRequest(response);
}).Produces<Response>();

app.Run();
