using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Utility
{
    public static class Log
    {
        public static Logger Instance { get; private set; }
        static Log()
        {
            LogManager.ReconfigExistingLoggers();

            Instance = LogManager.GetCurrentClassLogger();
        }

        public static void Info(this Logger logger, DelimeterEnum seperator = DelimeterEnum.Space, params object[] param)
        {
            var sep = " ";
            switch (seperator)
            {
                case DelimeterEnum.Space:
                    sep = " ";
                    break;
                case DelimeterEnum.Comma:
                    sep = ",";
                    break;
                case DelimeterEnum.SemiColon:
                    sep = ";";
                    break;
                case DelimeterEnum.Pipe:
                    sep = "|";
                    break;
                default:
                    break;
            }
            logger.Info(string.Join(sep, param));
        }

    }
}
