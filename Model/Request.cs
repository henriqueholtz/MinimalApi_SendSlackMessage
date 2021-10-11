using System.Text;
using System.Text.Json;

namespace MinimalApi_SendSlackMessage.Model
{
    public static class Request
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string MEDIATYPE = "application/json";
        public static Response SendMessage(Message message)
        {
            Uri webHookUri;
            try
            {
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
            catch (Exception ex)
            {
                return new Response(true, $"Exception throwded: {ex.Message}");
            }
        }
    }
}
