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
                if (std::is_base_of<T, DisposableBase>::value) // до изменения задания: if(disposableBase = (DisposableBase*)object)
                {
                    auto disposableBase = (DisposableBase*)object;

                    //TODO: add this method
                    //disposableBase->DisposeIfNotDisposed();
                }
                else if (std::is_base_of<T, IDisposable>::value) // до изменения задания: if(disposable = (IDisposable*)object)
                {
                    auto disposable = (IDisposable*)object;
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