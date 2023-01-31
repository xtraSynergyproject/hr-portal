using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public class CommandResult<T>
    {
        private CommandResult()
        {
            IsSuccess = true;
            ErrorType = ErrorTypeEnum.Error;
        }
        private CommandResult(T item)
        {
            IsSuccess = true;
            Item = item;
            ErrorType = ErrorTypeEnum.Error;
        }

        private CommandResult(T item, bool isSuccess, params KeyValuePair<string, string>[] messages)
        {
            IsSuccess = isSuccess;
            Messages = new List<KeyValuePair<string, string>>();
            Messages = messages;
            Item = item;
            ErrorType = ErrorTypeEnum.Error;
        }
        private CommandResult(T item, bool isSuccess, IList<KeyValuePair<string, string>> messages)
        {
            IsSuccess = isSuccess;
            Messages = new List<KeyValuePair<string, string>>();
            Messages = messages;
            Item = item;
            ErrorType = ErrorTypeEnum.Error;
        }
        private CommandResult(T item, bool isSuccess, string key, string msg)
        {
            IsSuccess = isSuccess;
            Messages = new List<KeyValuePair<string, string>>();
            Messages.Add(new KeyValuePair<string, string>(key, msg));
            Item = item;
            ErrorType = ErrorTypeEnum.Error;
        }
        private CommandResult(T item, bool isSuccess, ErrorTypeEnum errorType, string key, string msg)
        {
            IsSuccess = isSuccess;
            Messages = new List<KeyValuePair<string, string>>();
            Messages.Add(new KeyValuePair<string, string>(key, msg));
            Item = item;
            ErrorType = errorType;
        }
        public bool IsSuccess { get; set; }

        public IList<KeyValuePair<string, string>> Messages { get; set; }
        public T Item { get; set; }

        public static CommandResult<T> Instance()
        {
            return new CommandResult<T>();
        }

        public static CommandResult<T> Instance(T item)
        {
            return new CommandResult<T>(item);
        }

        public static CommandResult<T> Instance(T item, bool isSuccess, params KeyValuePair<string, string>[] messages)
        {
            return new CommandResult<T>(item, isSuccess, messages);
        }
        public static CommandResult<T> Instance(T item, bool isSuccess, IList<KeyValuePair<string, string>> messages)
        {
            return new CommandResult<T>(item, isSuccess, messages);
        }
        public static CommandResult<T> Instance(T item, bool isSuccess, string key, string msg)
        {
            return new CommandResult<T>(item, isSuccess, key, msg);
        }
        public static CommandResult<T> Instance(T item, bool isSuccess, ErrorTypeEnum errorType, string key, string msg)
        {
            return new CommandResult<T>(item, isSuccess, errorType, key, msg);
        }

        public string MessageString
        {
            get
            {
                if (Messages != null)
                {
                    var allKeys = (from kvp in Messages select kvp.Value).Distinct().ToList();
                    return string.Join(" ", allKeys);
                }
                else
                {
                    return "";
                }
            }
        }
        public ErrorTypeEnum ErrorType { get; set; }


    }
}
