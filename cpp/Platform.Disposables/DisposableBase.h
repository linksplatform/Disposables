#ifndef DISPOSABLES_DISPOSABLE_BASE_H
#define DISPOSABLES_DISPOSABLE_BASE_H

#include <stack>




#include "IDisposable.h"
#include "EnsureExtensions.h"
#include "../../../Exceptions/cpp/Platform.Exceptions/IgnoredExceptions.h"
#include "../../../Exceptions/cpp/Platform.Exceptions/ExceptionExtensions.h"
#include "../../../Exceptions/cpp/Platform.Exceptions/Ensure.h"


namespace Platform::Disposables
{
    class DisposableBase : public IDisposable
    {
        private: static std::stack<std::weak_ptr<DisposableBase>> _disposablesWeekReferencesStack;

    private: volatile std::atomic<std::int32_t> _disposed;

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


        protected: DisposableBase()
        {
            _disposed = 0;
            _disposablesWeekReferencesStack.push(std::weak_ptr<DisposableBase>(std::shared_ptr<DisposableBase>(this)));
            std::atexit(OnProcessExit);
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
            int compare_value = 1;
            bool originalDisposedValue = _disposed.compare_exchange_weak(compare_value, 0);
            auto wasDisposed = originalDisposedValue > 0;
            if (wasDisposed && !AllowMultipleDisposeCalls() && manual)
            {
                Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, *this, ObjectName(), "Multiple dispose calls are not allowed. Override AllowMultipleDisposeCalls property to modify behavior.");
            }
            if (AllowMultipleDisposeAttempts() || !wasDisposed)
            {
                this->Dispose(manual, wasDisposed);
            }
        }

        private: static void OnProcessExit()
        {
            while (!_disposablesWeekReferencesStack.empty())
            {
                auto weakReference = _disposablesWeekReferencesStack.top();
                _disposablesWeekReferencesStack.pop();

                if (auto disposable = weakReference.lock())
                {
                    disposable->Destruct();
                }
            }
        }
    };
}

#endif