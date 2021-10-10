using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/send", (Message message) =>
{
    Response response = Request.SendMessage(message);
    if (response != null && !response.Error) return Results.Ok(response.Message);

    return Results.BadRequest(response);
}).Produces<Response>();

app.Run();

public class Message
{
    private static readonly HttpClient _client = new HttpClient();
    public string Text { get; set; }
    public string WebHookUrl { get; set; }
    public string Username { get; set; }
    public string Channel { get; set; }
}

public static class Request
{
    private static readonly HttpClient _client = new HttpClient();
    private const string MEDIATYPE = "application/json";
    public static Response SendMessage(Message message)
    {
        Uri webHookUri;
        if (!Uri.TryCreate(message.WebHookUrl, UriKind.RelativeOrAbsolute, out webHookUri))
            return new Response(true, "You must send a valid url to the WebHookUrl!"); 

        using (var request = new HttpRequestMessage(HttpMethod.Post, message.WebHookUrl))
        {
            var requestBody = JsonSerializer.Serialize(new { text = message.Text, username = message.Username, response_type = "ephemeral", channel = message.Channel });
            request.Content = new StringContent(requestBody, Encoding.UTF8, MEDIATYPE);
            var response = _client.SendAsync(request).GetAwaiter().GetResult();
            var responseString = response.Content.ReadAsStringAsync();

            return new Response(!responseString.IsCompletedSuccessfully, responseString.Result);
        }
    }
}

public class Response
{
    public string Message { get; set; }
    public bool Error { get; set; }
    public Response(bool error, string message)
    {
        Message = message;
        Error = error;
    }
}