using System;
using System.Collections.Concurrent;
using System.Threading;
using Platform.Exceptions;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Provides a base implementation for IDisposable interface with the basic logic necessary to increase the likelihood of correct unmanaged resources release.</para>
    /// <para>Предоставляет базовую реализацию для интерфейса IDisposable с основной логикой необходимой для повышения вероятности корректного высвобождения неуправляемых ресурсов.</para>
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private static readonly AppDomain _currentDomain = AppDomain.CurrentDomain;
        private static readonly ConcurrentStack<WeakReference<DisposableBase>> _disposablesWeekReferencesStack = new ConcurrentStack<WeakReference<DisposableBase>>();

        private volatile int _disposed;

        public bool IsDisposed => _disposed > 0;

        protected virtual string ObjectName => GetType().Name;

        protected virtual bool AllowMultipleDisposeAttempts => false;

        protected virtual bool AllowMultipleDisposeCalls => false;

        static DisposableBase() => _currentDomain.ProcessExit += OnProcessExit;

        protected DisposableBase()
        {
            _disposed = 0;
            _disposablesWeekReferencesStack.Push(new WeakReference<DisposableBase>(this, false));
        }

        ~DisposableBase() => Destruct();

        protected abstract void Dispose(bool manual, bool wasDisposed);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public void Destruct()
        {
            try
            {
                if (!IsDisposed)
                {
                    Dispose(false);
                }
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
        }

        private void Dispose(bool manual)
        {
            var originalDisposedValue = Interlocked.CompareExchange(ref _disposed, 1, 0);
            var wasDisposed = originalDisposedValue > 0;
            if (wasDisposed && !AllowMultipleDisposeCalls && manual)
            {
                Ensure.Always.NotDisposed(this, ObjectName, "Multiple dispose calls are now allowed. Override AllowMultipleDisposeCalls property to modify behaviour.");
            }
            if (AllowMultipleDisposeAttempts || !wasDisposed)
            {
                Dispose(manual, wasDisposed);
            }
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            while (_disposablesWeekReferencesStack.TryPop(out WeakReference<DisposableBase> weakReference))
            {
                if (weakReference.TryGetTarget(out DisposableBase disposable))
                {
                    GC.SuppressFinalize(disposable);
                    disposable.Destruct();
                }
            }
            UnsubscribeFromProcessExitedEventIfPossible();
        }

        private static void UnsubscribeFromProcessExitedEventIfPossible()
        {
            try
            {
                if (_currentDomain != null)
                {
                    _currentDomain.ProcessExit -= OnProcessExit;
                }
                else
                {
                    AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
                }
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
        }
    }
}