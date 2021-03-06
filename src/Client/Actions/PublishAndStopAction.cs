﻿using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Transport;
using TDL.Client.Utils;

namespace TDL.Client.Actions
{
    public class PublishAndStopAction : IClientAction
    {
        public string AuditText { get; } = "(NOT PUBLISHED)";

        public void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response)
        {
            remoteBroker.RespondTo(request, response);
        }

        public Maybe<Request> GetNextRequest(RemoteBroker remoteBroker) => Maybe<Request>.None;
    }
}
