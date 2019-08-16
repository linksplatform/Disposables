#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Disposables
{
    public static class DisposableBaseExtensions
    {
        public static void DisposeIfNotDisposed(this DisposableBase disposable)
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }
    }
}
