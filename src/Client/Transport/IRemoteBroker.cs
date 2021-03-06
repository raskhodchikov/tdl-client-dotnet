using System;
using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Utils;

namespace TDL.Client.Transport
{
    public interface IRemoteBroker : IDisposable
    {
        Maybe<Request> Receive();

        void RespondTo(Request request, IResponse response);
    }
}
