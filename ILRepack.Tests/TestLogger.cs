using System;
using System.Collections.Generic;
using System.Text;
using ILRepacking;

namespace ILRepack.Tests
{
    public class TestLogger : ILogger
    {
        public bool ShouldLogVerbose { get; set; } = true;

        private enum Kind
        {
            Log,
            Verbose,
            Info,
            Warn,
            Error,
            DuplicateIgnored
        }

        private List<(Kind kind, string text)> messages = new List<(Kind kind, string text)>();

        public void DuplicateIgnored(string ignoredType, object ignoredObject)
        {
            messages.Add((Kind.DuplicateIgnored, $"{ignoredType}, {ignoredObject}"));
        }

        public void Error(string msg)
        {
            messages.Add((Kind.Error, msg));
        }

        public void Info(string msg)
        {
            messages.Add((Kind.Info, msg));
        }

        public void Log(object str)
        {
            messages.Add((Kind.Log, Convert.ToString(str)));
        }

        public void Verbose(string msg)
        {
            messages.Add((Kind.Verbose, msg));
       }

        public void Warn(string msg)
        {
            messages.Add((Kind.Warn, msg));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var item in messages)
            {
                sb.AppendLine($"{item.kind}: {item.text}");
            }

            return sb.ToString();
        }
    }
}