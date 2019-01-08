using System;
using System.Collections.Generic;
using NServiceBus;

namespace LimeTest.Messages.People
{
    public class GetPeoples : IMessage {}

    public class ResponseGetPeoples : IMessage
    {
        public List<Data.Entity.People> Peoples { get; set; }

        public Exception Error { get; set; }
    }
}