﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillFromAfarBot
{
    public static class Logger
    {
        private static List<string> log = new List<string>();

        public static event EventHandler LogAdded;

        public static void Log(string message)
        {
            log.Add(message);

            LogAdded?.Invoke(null, new EventArgs());
        }

        public static string GetLastLog()
        {
            if (log.Count > 0)
            {
                return log[log.Count - 1];
            }
            else
            {
                return null;
            }
               
        }
    }
}
