namespace MinimalApi_SendSlackMessage.Model
{
    public class Message
    {
        public string Text { get; set; }
        public string WebHookUrl { get; set; }
        public string Username { get; set; }
        public string Channel { get; set; }
    }
}
