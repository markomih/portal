﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataClasses
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("Conversation")]
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public virtual Member Sender { get; set; }
    }
}