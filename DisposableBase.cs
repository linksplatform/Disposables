using System;
using System.Diagnostics;
using System.Threading;
using Platform.Exceptions;

namespace Platform.Disposables
{
    /// <summary>
    /// Provides a base implementation for IDisposable interface with the basic logic necessary to increase the likelihood of correct unmanaged resources release.
    /// Предоставляет базовую реализацию для интерфейса IDisposable с основной логикой необходимой для повышения вероятности корректного высвобождения неуправляемых ресурсов.
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private static readonly Process _currentProcess = Process.GetCurrentProcess();

        private volatile int _disposed;

        public bool IsDisposed => _disposed > 0;

        protected virtual string ObjectName => GetType().Name;

        protected virtual bool AllowMultipleDisposeAttempts => false;

        protected virtual bool AllowMultipleDisposeCalls => false;

        protected DisposableBase()
        {
            _disposed = 0;
            _currentProcess.Exited += OnProcessExit;
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

        private void OnProcessExit(object sender, EventArgs e)
        {
            GC.SuppressFinalize(this);
            Destruct();
        }

        private void Dispose(bool manual)
        {
            var originalDisposedValue = Interlocked.CompareExchange(ref _disposed, 1, 0);
            var wasDisposed = originalDisposedValue > 0;
            if (!wasDisposed)
            {
                UnsubscribeFromProcessExitedEventIfPossible();
            }
            if (wasDisposed && !AllowMultipleDisposeCalls && manual)
            {
                Ensure.Always.NotDisposed(this, ObjectName, "Multiple dispose calls are now allowed. Override AllowMultipleDisposeCalls property to modify behaviour.");
            }
            if (AllowMultipleDisposeAttempts || !wasDisposed)
            {
                Dispose(manual, wasDisposed);
            }
        }

        private void UnsubscribeFromProcessExitedEventIfPossible()
        {
            try
            {
                if (_currentProcess != null)
                {
                    _currentProcess.Exited -= OnProcessExit;
                }
                else
                {
                    Process.GetCurrentProcess().Exited -= OnProcessExit;
                }
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
        }
    }
}