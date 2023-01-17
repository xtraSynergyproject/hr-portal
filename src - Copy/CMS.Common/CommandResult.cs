using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CMS.Common
{
    public class CommandResult<T>
    {

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Messages { get; set; }
        public T Item { get; set; }

        public static CommandResult<T> Instance()
        {
            return new CommandResult<T>();
        }

        public static CommandResult<T> Instance(T item)
        {
            return new CommandResult<T>(item);
        }

        public static CommandResult<T> Instance(T item, bool isSuccess, Dictionary<string, string> messages)
        {
            return new CommandResult<T>(item, isSuccess, messages);
        }
        public static CommandResult<T> Instance(T item, Dictionary<string, string> messages)
        {
            return new CommandResult<T>(item, false, messages);
        }

        public static CommandResult<T> Instance(T item, bool isSuccess, string messages)
        {
            return new CommandResult<T>(item, isSuccess, messages);
        }
        public static CommandResult<T1> Instance<T1, TKey>(T1 item, Func<T, TKey> keySelector, string message) where T1 : T
        {

            var dict = new Dictionary<string, string>();
            dict.Add(keySelector.GetType().Name, message);
            return new CommandResult<T1>(item, false, dict);
        }
        private CommandResult()
        {
            IsSuccess = true;
        }
        private CommandResult(T item)
        {
            IsSuccess = true;
            Item = item;
        }

        private CommandResult(T item, bool isSuccess, Dictionary<string, string> messages)
        {
            IsSuccess = isSuccess;
            Messages = messages;
            Item = item;
        }

        private CommandResult(T item, bool isSuccess, string messages)
        {
            IsSuccess = isSuccess;
            Message = messages;
            Item = item;
        }
        public string HtmlError
        {
            get

            {
                if (Messages != null && Messages.Count > 0)
                {
                    var error = "<ul>";
                    foreach (var err in Messages)
                    {

                        error = string.Concat(error, "<li>", err.Value, "</li>");
                    }
                    error = string.Concat(error, "</ul>");
                    return error;
                }
                else if (Message.IsNotNullAndNotEmpty())
                {
                    return $"<ul><li>{Message}</li></ul>";
                }
                return string.Empty;



            }
        }
        public string ErrorText
        {
            get

            {
                if (Messages != null && Messages.Count > 0)
                {
                    var error = "";
                    foreach (var err in Messages)
                    {
                        error = string.Concat(error, err.Value, ";");
                    }
                    return error;
                }
                return Message;
            }
        }


    }
}
