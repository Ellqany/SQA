using System;

namespace SQA.Models
{
    public class Message
    {
        public string MessageID { get; set; }
        public string Subject { get; set; }
        public string Object { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
