#ifndef DISPOSABLES_IDISPOSABLE_EXTENSIONS_H
#define DISPOSABLES_IDISPOSABLE_EXTENSIONS_H



namespace Platform::Disposables
{
    class IDisposableExtensions
    {
        public: static void DisposeIfNotDisposed(IDisposable &disposable)
        {
            if (!disposable.IsDisposed())
            {
                disposable.Dispose();
            }
        }
    };
}

#endif