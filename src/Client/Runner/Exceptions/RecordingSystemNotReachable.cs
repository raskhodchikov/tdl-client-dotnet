﻿using System;

namespace TDL.Client.Runner.Exceptions
{
    public class RecordingSystemNotReachable : Exception
    {
        public RecordingSystemNotReachable(Exception innerException)
            : base($"Could not reach recording system: {innerException.Message}", innerException)
        {
        }
    }
}
