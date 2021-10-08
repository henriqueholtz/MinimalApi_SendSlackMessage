using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/send", (Message message) =>
{
    Request.SendMessage(message);
    return message;
});

app.Run();

public class Message
{
    private static readonly HttpClient _client = new HttpClient();
    public string Text { get; set; }
    public string WebHookUrl { get; set; }
}

public static class Request
{
    private static readonly HttpClient _client = new HttpClient();
    private const string MEDIATYPE = "application/json";
    public static string SendMessage(Message message)
    {
        Uri webHookUri;
        if (!Uri.TryCreate(message.WebHookUrl, UriKind.RelativeOrAbsolute, out webHookUri))
            return "error"; //new Response(true, "");

        using (var request = new HttpRequestMessage(HttpMethod.Post, message.WebHookUrl))
        {
            var requestBody = JsonSerializer.Serialize(new { text = "test 123", username = "Username", response_type = "ephemeral", channel = "messages" });
            request.Content = new StringContent(requestBody, Encoding.UTF8, MEDIATYPE);
            var response = _client.SendAsync(request).GetAwaiter().GetResult();
            var responseString = response.Content.ReadAsStringAsync();
            return responseString.Result;
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