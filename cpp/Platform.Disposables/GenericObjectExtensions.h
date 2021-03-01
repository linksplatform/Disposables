#ifndef DISPOSABLES_GENERIC_DISPOSABLE_H
#define DISPOSABLES_GENERIC_DISPOSABLE_H

namespace Platform::Disposables
{
    class GenericObjectExtensions
    {
        public: template <typename T> static bool TryDispose(T object)
        {
            try
            {
                if (DisposableBase* disposableBase = (DisposableBase*)object)
                {
                    //TODO: add this method
                    //disposableBase->DisposeIfNotDisposed();
                }
                else if (System::IDisposable* disposable = (System::IDisposable*)object)
                {
                    disposable->Dispose();
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

#endif