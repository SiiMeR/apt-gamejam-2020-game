using System;
using UnityEngine;

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
        public Vector3Int location;
        public bool expires = true;
        
        public EventDTO(string name)
        {
            this.name = name;
        }
        
        public EventDTO(string name, string text, string acceptText, string declineText, Action acceptAction, Action declineAction, Vector3Int location)
        {
            this.name = name;
            this.text = text;
            this.acceptText = acceptText;
            this.declineText = declineText;
            this.acceptAction = acceptAction;
            this.declineAction = declineAction;
            this.location = location;
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