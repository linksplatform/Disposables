using System;

namespace Platform.Disposables
{
    public partial class Disposable
    {
        public static bool TryDispose<T>(ref T @object)
        {
            try
            {
                if (@object is DisposableBase disposableBase)
                {
                    if (!disposableBase.IsDisposed)
                    {
                        disposableBase.Dispose();
                        @object = default;
                        return true;
                    }
                }
                else
                {
                    if (@object is System.IDisposable disposable)
                    {
                        disposable.Dispose();
                        @object = default;
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool TryDispose<T>(T @object) => TryDispose(ref @object);

        public static void DisposeIfDisposable<T>(T @object) => TryDispose(ref @object);
    }

    public partial class Disposable<T>
    {
        public static Disposable<T> Create(T value, Action<T> dispose) => new Disposable<T>(value, dispose);
    }
}
