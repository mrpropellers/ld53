namespace LeftOut
{
    public interface INeedsInitialization<in T>
    {
        bool IsInitialized { get; }
        bool TryInitialize(T initializer);
    }
}
