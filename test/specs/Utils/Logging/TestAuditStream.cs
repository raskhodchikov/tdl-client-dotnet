﻿using System.Text;
using TDL.Client.Audit;

namespace TDL.Test.Specs.Utils.Logging
{
    public class TestAuditStream : IAuditStream
    {
        private readonly StringBuilder log = new StringBuilder();

        public void ClearLog()
        {
            log.Clear();
        }

        public string GetLog() => log.ToString();

        public void WriteLine(string value)
        {
            log.AppendLine(value);
        }

        public override string ToString() => GetLog();
    }
}
