using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform.Exceptions;
using Platform.Exceptions.ExtensionRoots;

#pragma warning disable IDE0060 // Remove unused parameter

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Provides a set of extension methods for <see cref="Platform.Exceptions.ExtensionRoots.EnsureAlwaysExtensionRoot"/> and <see cref="Platform.Exceptions.ExtensionRoots.EnsureOnDebugExtensionRoot"/> objects.</para>
    /// <para>Предоставляет набор методов расширения для объектов <see cref="Platform.Exceptions.ExtensionRoots.EnsureAlwaysExtensionRoot"/> и <see cref="Platform.Exceptions.ExtensionRoots.EnsureOnDebugExtensionRoot"/>.</para>
    /// </summary>
    public static class EnsureExtensions
    {
        #region Always

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed regardless of the build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется внезависимости от конфигурации сборки.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        /// <param name="objectName"><para>The name of object.</para><para>Имя объекта.</para></param>
        /// <param name="message"><para>The message of the thrown exception.</para><para>Сообщение выбрасываемого исключения.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable, string objectName, string message)
        {
            if (disposable.IsDisposed)
            {
                throw new ObjectDisposedException(objectName, message);
            }
        }

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed regardless of the build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется внезависимости от конфигурации сборки.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        /// <param name="objectName"><para>The name of object.</para><para>Имя объекта.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable, string objectName) => NotDisposed(root, disposable, objectName, null);

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed regardless of the build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется внезависимости от конфигурации сборки.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable) => NotDisposed(root, disposable, null, null);

        #endregion

        #region OnDebug

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed only for DEBUG build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется только для конфигурации сборки DEBUG.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        /// <param name="objectName"><para>The name of object.</para><para>Имя объекта.</para></param>
        /// <param name="message"><para>The message of the thrown exception.</para><para>Сообщение выбрасываемого исключения.</para></param>
        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable, string objectName, string message) => Ensure.Always.NotDisposed(disposable, objectName, message);

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed only for DEBUG build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется только для конфигурации сборки DEBUG.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        /// <param name="objectName"><para>The name of object.</para><para>Имя объекта.</para></param>
        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable, string objectName) => Ensure.Always.NotDisposed(disposable, objectName, null);

        /// <summary>
        /// <para>Ensures that an object that implements the <see cref="IDisposable"/> interface has not been released. This check is performed only for DEBUG build configuration.</para>
        /// <para>Гарантирует, что объект, реализующий интерфейс <see cref="IDisposable"/> не был высвобожден. Эта проверка выполняется только для конфигурации сборки DEBUG.</para>
        /// </summary>
        /// <param name="root"><para>The extension root to which this method is bound.</para><para>Корень-расширения, к которому привязан этот метод.</para></param>
        /// <param name="disposable"><para>The object implementing the <see cref="IDisposable"/> interface.</para><para>Объект, реализующий интерфейс <see cref="IDisposable"/></para></param>
        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable) => Ensure.Always.NotDisposed(disposable, null, null);

        #endregion
    }
}
