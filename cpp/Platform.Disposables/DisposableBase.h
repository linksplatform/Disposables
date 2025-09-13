#pragma once
#include <atomic>
#include <string>
#include <typeinfo>
#include <cstdint>

namespace Platform::Disposables
{
    class DisposableBase : public IDisposable
    {

        private: std::atomic<std::int32_t> _disposed;

        public: bool IsDisposed()
        {
            return _disposed.load() > 0;
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


        protected: DisposableBase()
        {
            _disposed = 0;
        }

        ~DisposableBase() { Destruct(); }

        protected: virtual void Dispose(bool manual, bool wasDisposed) = 0;

        public: void Dispose()
        {
            this->Dispose(true);
        }

        public: void Destruct()
        {
            try
            {
                if (!IsDisposed())
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
            std::int32_t expected = 0;
            auto wasDisposed = !_disposed.compare_exchange_strong(expected, 1);
            if (wasDisposed && !AllowMultipleDisposeCalls() && manual)
            {
                Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, this, ObjectName(), "Multiple dispose calls are not allowed. Override AllowMultipleDisposeCalls property to modify behavior.");
            }
            if (AllowMultipleDisposeAttempts() || !wasDisposed)
            {
                this->Dispose(manual, wasDisposed);
            }
        }

    };
}
