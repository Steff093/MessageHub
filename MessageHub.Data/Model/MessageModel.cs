namespace MessageHub.Data.Model
{
    public class MessageModel
    {
        public string UserName { get; set; }
        public string? Sender { get; set; }
        public string? Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
