namespace Nikomata.Runtime
{
    public interface ISignalListener<TPayload> where TPayload : struct, ISignalPayload
    {
        void Signal(TPayload payload);
    }
}