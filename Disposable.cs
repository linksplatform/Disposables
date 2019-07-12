using System;

namespace Platform.Disposables
{
    public delegate void DisposedDelegate(bool manual);

    public partial class Disposable : DisposableBase
    {
        private static readonly DisposedDelegate EmptyDelegate = manual => { };

        public event DisposedDelegate OnDispose = EmptyDelegate;

        public Disposable()
        {
        }

        public Disposable(Action disposed) => OnDispose = m => disposed();

        public Disposable(DisposedDelegate disposed) => OnDispose = disposed;

        protected override void DisposeCore(bool manual, bool wasDisposed) => OnDispose(manual);
    }

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
}