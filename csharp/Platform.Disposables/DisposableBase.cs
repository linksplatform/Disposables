using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using Platform.Exceptions;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Represents a base implementation for <see cref="Platform.Disposables.IDisposable"/> interface with the basic logic necessary to increase the likelihood of correct unmanaged resources release.</para>
    /// <para>Представляет базовую реализацию для интерфейса <see cref="Platform.Disposables.IDisposable"/> с основной логикой необходимой для повышения вероятности корректного высвобождения неуправляемых ресурсов.</para>
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
        public bool IsDisposed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _disposed > 0;
        }

        /// <summary>
        /// <para>Gets the name of an object or a unique string describing this object.</para>
        /// <para>Возвращает имя объекта или уникальную строку описывающую этот объект.</para>
        /// </summary>
        protected virtual string ObjectName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetType().Name;
        }

        /// <summary>
        /// <para>Gets a value indicating whether multiple attempts to dispose this object are allowed.</para>
        /// <para>Возвращает значение определяющие разрешено ли выполнять несколько попыток высвободить этот объект.</para>
        /// </summary>
        protected virtual bool AllowMultipleDisposeAttempts
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        /// <summary>
        /// <para>Gets a value indicating whether it is allowed to call this object disposal multiple times.</para>
        /// <para>Возвращает значение определяющие разрешено ли несколько раз вызывать высвобождение этого объекта.</para>
        /// </summary>
        protected virtual bool AllowMultipleDisposeCalls
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static DisposableBase() => _currentDomain.ProcessExit += OnProcessExit;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DisposableBase"/> class.</para>
        /// <para>Инициализирует новый экземпляр класса <see cref="DisposableBase"/>.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected DisposableBase()
        {
            _disposed = 0;
            _disposablesWeekReferencesStack.Push(new WeakReference<DisposableBase>(this, false));
        }

        /// <summary>
        /// <para>Performs any necessary final clean-up when a class instance is being collected by the garbage collector.</para>
        /// <para>Выполняет любую необходимую окончательную очистку, когда сборщик мусора собирает экземпляр класса.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract void Dispose(bool manual, bool wasDisposed);

        /// <summary>
        /// <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</para>
        /// <para>Выполняет определенные пользователем задачи, связанные с освобождением, высвобождением или сбросом неуправляемых ресурсов.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources without throwing any exceptions.</para>
        /// <para>Выполняет определенные пользователем задачи, связанные с освобождением, высвобождением или сбросом неуправляемых ресурсов без выбрасывания исключений.</para>
        /// </summary>
        /// <remarks>
        /// <para>Should be called only from classes destructors, or in case exceptions should be not thrown.</para>
        /// <para>Должен вызываться только из деструкторов классов, или в случае, если исключения выбрасывать нельзя.</para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// <para>Disposes unmanaged resources.</para>
        /// <para>Высвобождает неуправляемые ресурсы.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from the developer.</para>
        /// <para>Значение определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        protected virtual void Dispose(bool manual)
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
