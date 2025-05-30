namespace Nikomata.Runtime
{
    public interface IRequestListener<TPayload, TResult> where TPayload : struct, IRequestPayload<TResult>
    {
        TResult Request(TPayload payload);
    }
}