namespace MinimalApi_SendSlackMessage.Model
{
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
}
