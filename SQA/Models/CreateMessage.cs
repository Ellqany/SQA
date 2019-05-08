using System.Collections.Generic;

namespace SQA.Models
{
    public class CreateMessage
    {
        public List<Message> Messages { get; set; }
        public string txtMessage { get; set; }
        public string UserName { get; set; }
        public string Sender { get; set; }
    }
}
