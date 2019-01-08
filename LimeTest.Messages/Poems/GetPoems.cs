using System;
using System.Collections.Generic;
using LimeTest.Data.Entity;
using NServiceBus;

namespace LimeTest.Messages.Poems
{
    public class GetPoems : IMessage
    {
        
    }

    public class ResponseGetPoems : IMessage
    {
        public List<Poem> Poems { get; set; }

        public Exception Error { get; set; }
    }
}