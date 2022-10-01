namespace BFT
{
    public interface IGenericEventListener<T>
    {
        void OnEventRaised(T param);
    }
}
