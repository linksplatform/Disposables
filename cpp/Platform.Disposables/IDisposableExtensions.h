namespace Platform::Disposables
{
    class IDisposableExtensions
    {
        public: static void DisposeIfNotDisposed(IDisposable &disposable)
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }
    };
}
