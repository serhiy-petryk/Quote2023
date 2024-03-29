﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScanner.Helpers
{
    public static class Logger
    {
        public delegate void MessageAddedEventHandler(object sender, MessageAddedEventArgs e);
        public static event MessageAddedEventHandler MessageAdded;

        // private static readonly List<MessageAddedEventArgs> Messages = new List<MessageAddedEventArgs>();

        public static void AddMessage(string message)
        {
            var sf = new StackFrame(1);
            var method = sf.GetMethod();
            var callMethodName = method.DeclaringType?.Name + "." + method.Name;

            var oMessage = new MessageAddedEventArgs(message, callMethodName);
            // Messages.Add(oMessage);
            MessageAdded?.Invoke(null, oMessage);
        }

        public class MessageAddedEventArgs : EventArgs
        {
            public readonly DateTime Date = DateTime.Now;
            public readonly string MethodName;
            public readonly string Message;
            public string FullMessage => $"{MethodName}. {Message}";
            public MessageAddedEventArgs(string msg, string methodName)
            {
                Message = msg;
                MethodName = methodName;
                if (Message.StartsWith("!"))
                    Debug.Print($"LOGGER: {FullMessage}");
            }
        }
    }
}
