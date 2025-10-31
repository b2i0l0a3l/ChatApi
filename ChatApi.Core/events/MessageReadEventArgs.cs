using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Core.events
{
    public class MessageReadEventArgs : EventArgs
    {
        public Guid ConversationID { get; set; }
    }
}