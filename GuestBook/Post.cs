using System;
using System.Collections.Generic;
using System.Text;

namespace GuestBook
{
    class Post
    {
        //Skapar properties med tillhörande setters aoch getters för att hålla variablerna privata
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private string sender;

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
    }
}
