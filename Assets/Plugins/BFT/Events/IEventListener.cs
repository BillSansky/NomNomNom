namespace BFT
{
    public interface IEventListener
    {
        void OnEventRaised(IEvent eventWatched);
    }
}
