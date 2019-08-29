namespace Platform.Disposables
{
    /// <summary>
    /// <para>Provides a set of extension methods for <see cref="IDisposable"/> objects.</para>
    /// <para>Предоставляет набор методов расширения для объектов <see cref="IDisposable"/>.</para>
    /// </summary>
    public static class IDisposableExtensions
    {
        /// <summary>
        /// <para>Attempts to dispose the specified object.</para>
        /// <para>Выполняет попытку высвободить указанный объект.</para>
        /// </summary>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        public static void DisposeIfNotDisposed(this IDisposable disposable)
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }
    }
}
