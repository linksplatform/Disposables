namespace Platform.Disposables
{
    /// <summary>
    /// Представляет расширенный интерфейс IDisposable.
    /// Represents an extended IDisposable interface.
    /// </summary>
    public interface IDisposable : System.IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the object was disposed.
        /// Возвращает значение определяющее был ли высвобожден объект.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources without throwing any exceptions.
        /// Выполняет определенные пользователем задачи, связанные с освобождением, высвобождением или сбросом неуправляемых ресурсов без выбрасывания исключений.
        /// </summary>
        /// <remarks>
        /// Should be called only from classes destructors, or in case exceptions should be not thrown.
        /// Должен вызываться только из деструкторов классов, или в случае, если исключения выбрасывать нельзя.
        /// </remarks>
        void Destruct();
    }
}