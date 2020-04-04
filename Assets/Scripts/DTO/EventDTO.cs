using System;

namespace DTO
{
    public class EventDTO
    {
        public string name;
        public string text;
        public string acceptText;
        public string declineText;
        public Action acceptAction;
        public Action declineAction;
        
        public EventDTO(string name)
        {
            this.name = name;
        }
        
        public EventDTO(string name, string text, string acceptText, string declineText, Action acceptAction, Action declineAction)
        {
            this.name = name;
            this.text = text;
            this.acceptText = acceptText;
            this.declineText = declineText;
            this.acceptAction = acceptAction;
            this.declineAction = declineAction;
        }
        
        
        
    }
}