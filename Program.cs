using System.Collections.Immutable;
using MinimalApi_SendSlackMessage.Model;

var builder = WebApplication.CreateBuilder(args);

#region Swagger
bool swagger = Boolean.TrueString.Equals(builder.Configuration.GetSection("enable-swagger").Value, StringComparison.InvariantCultureIgnoreCase);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
if (swagger) app.UseSwagger();


app.MapGet("/", (IConfiguration Configuration) =>
{
    return new { 
        Message = "Welcome to SendSlackMessage - The Minimal Api from dotnet 6!", 
        OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
        Test = Configuration["test"],
        EnalbeSwagger = swagger,
        Environment = builder.Environment.EnvironmentName
    };
});

app.MapPost("/send", (Message message) =>
{
    Response response = Request.SendMessage(message);
    if (response != null && !response.Error) return Results.Ok(response.Message);

    return Results.BadRequest(response);
}).Produces<Response>();

if (swagger) app.UseSwaggerUI();
app.Run();
