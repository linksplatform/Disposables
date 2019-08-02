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
