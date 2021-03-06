﻿namespace Platform::Disposables
{
    class DisposableBase : public IDisposable
    {
        private: static readonly ConcurrentStack<WeakReference<DisposableBase>> _disposablesWeekReferencesStack = ConcurrentStack<WeakReference<DisposableBase>>();

        private: volatile std::int32_t _disposed;

        public: bool IsDisposed()
        {
            return _disposed > 0;
        }

        protected: virtual std::string ObjectName()
        {
            return typeid(this).name();
        }

        protected: virtual bool AllowMultipleDisposeAttempts()
        {
            return false;
        }

        protected: virtual bool AllowMultipleDisposeCalls()
        {
            return false;
        }

        static DisposableBase() { std::atexit(OnProcessExit); }

        protected: DisposableBase()
        {
            _disposed = 0;
            _disposablesWeekReferencesStack.Push(WeakReference<DisposableBase>(this, false));
        }

        ~DisposableBase() { Destruct(); }

        protected: virtual void Dispose(bool manual, bool wasDisposed) = 0;

        public: void Dispose()
        {
            this->Dispose(true);
            GC.SuppressFinalize(this);
        }

        public: void Destruct()
        {
            try
            {
                if (!IsDisposed)
                {
                    this->Dispose(false);
                }
            }
            catch (const std::exception& exception)
            {
                Platform::Exceptions::ExceptionExtensions::Ignore(exception);
            }
        }

        protected: virtual void Dispose(bool manual)
        {
            auto originalDisposedValue = Interlocked.CompareExchange(ref _disposed, 1, 0);
            auto wasDisposed = originalDisposedValue > 0;
            if (wasDisposed && !AllowMultipleDisposeCalls && manual)
            {
                Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, this, ObjectName, "Multiple dispose calls are not allowed. Override AllowMultipleDisposeCalls property to modify behavior.");
            }
            if (AllowMultipleDisposeAttempts || !wasDisposed)
            {
                this->Dispose(manual, wasDisposed);
            }
        }

        private: static void OnProcessExit()
        {
            while (_disposablesWeekReferencesStack.TryPop(out WeakReference<DisposableBase> weakReference))
            {
                if (weakReference.TryGetTarget(out DisposableBase disposable))
                {
                    GC.SuppressFinalize(disposable);
                    disposable.Destruct();
                }
            }
        }
    };
}
