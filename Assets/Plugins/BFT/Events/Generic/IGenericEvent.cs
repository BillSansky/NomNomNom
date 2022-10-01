namespace BFT
{
    public interface IGenericEvent<T> : IEvent
    {
        void RaiseEvent(T param);
        void RegisterListener(IGenericEventListener<T> listener);
        void UnRegisterListener(IGenericEventListener<T> listener);
    }
}
