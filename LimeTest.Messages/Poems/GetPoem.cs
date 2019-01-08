using NServiceBus;

namespace LimeTest.Messages.Poems
{
    public class GetPoem : IEvent
    {
        public int PeopleId { get; set; }
    }
}