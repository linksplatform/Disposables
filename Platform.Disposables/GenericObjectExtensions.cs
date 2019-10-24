using System;
using System.Runtime.CompilerServices;
using Platform.Exceptions;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Provides a set of static methods that help dispose a generic objects.</para>
    /// <para>Предоставляет набор статических методов которые помогают высвободить универсальные объекты.</para>
    /// </summary>
    static public class GenericObjectExtensions
    {
        /// <summary>
        /// <para>Attempts to dispose the specified object.</para>
        /// <para>Выполняет попытку высвободить указанный объект.</para>
        /// </summary>
        /// <typeparam name="T"><para>Type of the specified object.</para><para>Тип указанного объекта.</para></typeparam>
        /// <param name="object"><para>The object to dispose.</para><para>Объект, который необходимо высвободить.</para></param>
        /// <returns><para>A value that determines whether the attempt to release the specified object was successful.</para><para>Значение, определяющие удачно ли была выполнена попытка высвободить указанный объект.</para></returns>
        public static bool TryDispose<T>(this T @object)
        {
            try
            {
                if (@object is DisposableBase disposableBase)
                {
                    disposableBase.DisposeIfNotDisposed();
                }
                else if (@object is System.IDisposable disposable)
                {
                    disposable.Dispose();
                }
                return true;
            }
            catch (Exception exception)
            {
                exception.Ignore();
            }
            return false;
        }

        /// <summary>
        /// <para>Attempts to dispose the specified object.</para>
        /// <para>Выполняет попытку высвободить указанный объект.</para>
        /// </summary>
        /// <typeparam name="T"><para>Type of the specified object.</para><para>Тип указанного объекта.</para></typeparam>
        /// <param name="object"><para>The object to dispose.</para><para>Объект, который необходимо высвободить.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeIfPossible<T>(this T @object) => TryDispose(@object);
    }
}
