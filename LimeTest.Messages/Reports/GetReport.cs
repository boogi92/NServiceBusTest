using System;
using LimeTest.Messages.Poems;
using NServiceBus;

namespace LimeTest.Messages.Reports
{
    public class GetReport : IMessage {}

    public class ResponseGetReport : IMessage
    {
        public string Path { get; set; }

        public Exception Error { get; set; }
    }
}