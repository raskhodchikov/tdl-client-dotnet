using System;

namespace TDL.Client
{
	public class TdlClient
	{
		protected string m_hostname;
		protected int m_port;
		protected string m_uniqueId;
		protected TimeSpan m_timeToWaitForRequests;


		public TdlClient( string hostname, int port, string uniqueId, TimeSpan timeToWaitForRequests )
		{
			m_hostname = hostname;
			m_port = port;
			m_uniqueId = uniqueId;
			m_timeToWaitForRequests = timeToWaitForRequests;
		}
	}
}
