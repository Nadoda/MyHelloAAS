namespace HelloAssetAdministrationShell.MqttConnection.I40MessageFormat
{
    public class Frame
    {
        public SemanticProtocol semanticProtocol { get; set; }
        public string type { get; set; }
        public string messageId { get; set; }
        public Sender sender { get; set; }
        public Receiver receiver { get; set; }
        public string replyBy { get; set; }
        public string conversationId { get; set; }
    }
}