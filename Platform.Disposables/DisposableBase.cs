using System;
using System.Collections.Concurrent;
using System.Threading;
using Platform.Exceptions;

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

        /// <summary>
        /// <para>Gets a value indicating whether the object was disposed.</para>
        /// <para>Возвращает значение определяющее был ли высвобожден объект.</para>
        /// </summary>
        public bool IsDisposed => _disposed > 0;

        /// <summary>
        /// <para>Gets the name of an object or a unique string describing this object.</para>
        /// <para>Возвращает имя объекта или уникальную строку описывающую этот объект.</para>
        /// </summary>
        protected virtual string ObjectName => GetType().Name;

        /// <summary>
        /// <para>Gets a value indicating whether multiple attempts to dispose this object are allowed.</para>
        /// <para>Возвращает значение определяющие разрешено ли выполнять несколько попыток высвободить этот объект.</para>
        /// </summary>
        protected virtual bool AllowMultipleDisposeAttempts => false;

        /// <summary>
        /// <para>Gets a value indicating whether it is allowed to call this object disposal multiple times.</para>
        /// <para>Возвращает значение определяющие разрешено ли несколько раз вызывать высвобождение этого объекта.</para>
        /// </summary>
        protected virtual bool AllowMultipleDisposeCalls => false;

        static DisposableBase() => _currentDomain.ProcessExit += OnProcessExit;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DisposableBase"/> class.</para>
        /// <para>Инициализирует новый экземпляр класса <see cref="DisposableBase"/>.</para>
        /// </summary>
        protected DisposableBase()
        {
            _disposed = 0;
            _disposablesWeekReferencesStack.Push(new WeakReference<DisposableBase>(this, false));
        }

        /// <summary>
        /// <para>Performs any necessary final clean-up when a class instance is being collected by the garbage collector.</para>
        /// <para>Выполняет любую необходимую окончательную очистку, когда сборщик мусора собирает экземпляр класса.</para>
        /// </summary>
        ~DisposableBase() => Destruct();

        /// <summary>
        /// <para>Disposes unmanaged resources.</para>
        /// <para>Высвобождает неуправляемые ресурсы.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from the developer.</para>
        /// <para>Значение определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A value that determines whether the object was released before calling this method.</para>
        /// <para>Значение определяющие был ли высвобожден объект до вызова этого метода.</para>
        /// </param>
        protected abstract void Dispose(bool manual, bool wasDisposed);

        /// <summary>
        /// <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</para>
        /// <para>Выполняет определенные пользователем задачи, связанные с освобождением, высвобождением или сбросом неуправляемых ресурсов.</para>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources without throwing any exceptions.</para>
        /// <para>Выполняет определенные пользователем задачи, связанные с освобождением, высвобождением или сбросом неуправляемых ресурсов без выбрасывания исключений.</para>
        /// </summary>
        /// <remarks>
        /// <para>Should be called only from classes destructors, or in case exceptions should be not thrown.</para>
        /// <para>Должен вызываться только из деструкторов классов, или в случае, если исключения выбрасывать нельзя.</para>
        /// </remarks>
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
                Ensure.Always.NotDisposed(this, ObjectName, "Multiple dispose calls are not allowed. Override AllowMultipleDisposeCalls property to modify behavior.");
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