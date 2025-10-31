using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Core.events;

namespace ChatApi.Application.handler
{
    public class ChatEventHandler : EventArgs
    {
        public delegate Task MessageReadHandler(object? sender, MessageReadEventArgs e);
        public event MessageReadHandler? OnMessageRead;

        public void RaiseMessageRead(object? sender, MessageReadEventArgs e)
        {
            OnMessageRead?.Invoke(sender, e);
        }

    }
}