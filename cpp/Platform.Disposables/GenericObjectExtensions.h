namespace Platform::Disposables
{
    class GenericObjectExtensions
    {
        public: template <typename T> static bool TryDispose(T object)
        {
            try
            {
                if (object is DisposableBase disposableBase)
                {
                    disposableBase.DisposeIfNotDisposed();
                }
                else if (object is System::IDisposable &disposable)
                {
                    disposable.Dispose();
                }
                return true;
            }
            catch (const std::exception& exception)
            {
                Platform::Exceptions::ExceptionExtensions::Ignore(exception);
            }
            return false;
        }

        public: template <typename T> static void DisposeIfPossible(T object) { TryDispose(object); }
    };
}
