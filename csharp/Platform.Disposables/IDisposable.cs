using System.Runtime.CompilerServices;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Представляет расширенный интерфейс <see cref="System.IDisposable"/>.</para>
    /// <para>Represents an extended <see cref="System.IDisposable"/> interface.</para>
    /// </summary>
    public interface IDisposable : System.IDisposable
    {
        /// <summary>
        /// <para>Gets a value indicating whether the object was disposed.</para>
        /// <para>Возвращает значение определяющее был ли высвобожден объект.</para>
        /// </summary>
        bool IsDisposed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
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
        void Destruct();
    }
}