using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeIfNotDisposed(this IDisposable disposable)
        {
            if (!disposable.IsDisposed)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// <para>Wraps a <see cref="System.IDisposable"/> object in a <see cref="Disposable"/> wrapper.</para>
        /// <para>Оборачивает объект <see cref="System.IDisposable"/> в обёртку <see cref="Disposable"/>.</para>
        /// </summary>
        /// <param name="disposable"><para>The <see cref="System.IDisposable"/> object to wrap.</para><para>Объект <see cref="System.IDisposable"/> для оборачивания.</para></param>
        /// <returns><para>A new <see cref="Disposable"/> wrapper.</para><para>Новая обёртка <see cref="Disposable"/>.</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Disposable AsDisposable(this System.IDisposable disposable) => Disposable.Create(disposable);

        /// <summary>
        /// <para>Wraps a <see cref="System.IDisposable"/> object in a <see cref="Disposable{T}"/> wrapper.</para>
        /// <para>Оборачивает объект <see cref="System.IDisposable"/> в обёртку <see cref="Disposable{T}"/>.</para>
        /// </summary>
        /// <param name="disposable"><para>The <see cref="System.IDisposable"/> object to wrap.</para><para>Объект <see cref="System.IDisposable"/> для оборачивания.</para></param>
        /// <returns><para>A new <see cref="Disposable{T}"/> wrapper.</para><para>Новая обёртка <see cref="Disposable{T}"/>.</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Disposable<System.IDisposable> AsDisposableContainer(this System.IDisposable disposable) => Disposable<System.IDisposable>.Create(disposable);
    }
}
