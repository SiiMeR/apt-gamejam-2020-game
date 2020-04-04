using System;

namespace DTO
{
    public class EventDTO
    {
        public string name;
        public string text;
        public string acceptText;
        public string declineText;
        // public Func<>
        
        public EventDTO(string name)
        {
            this.name = name;
        }
        
        public EventDTO(string name, string text, string acceptText, string declineText)
        {
            this.name = name;
            this.text = text;
            this.acceptText = acceptText;
            this.declineText = declineText;
        }
        
        
        
    }
}