using System;
using Platform.Exceptions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Disposables
{
    static public class GenericObjectExtensions
    {
        public static bool TryDispose<T>(this T @object)
        {
            try
            {
                if (@object is DisposableBase disposableBase)
                {
                    disposableBase.DisposeIfNotDisposed();
                }
                else if (@object is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
                return true;
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
            return false;
        }

        public static void DisposeIfPossible<T>(this T @object) => TryDispose(@object);
    }
}
