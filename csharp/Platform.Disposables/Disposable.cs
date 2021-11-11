using System;
using System.Runtime.CompilerServices;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Represents disposable object that contains OnDispose event which is raised when the object itself is disposed.</para>
    /// <para>Представляет высвобождаемый объект, который содержит событие OnDispose, которое возникает при высвобождении самого объекта.</para>
    /// </summary>
    public class Disposable : DisposableBase
    {
        private static readonly Disposal _emptyDelegate = (manual, wasDisposed) => { };

        /// <summary>
        /// <para>Occurs when the object is being disposed.</para>
        /// <para>Возникает, когда объект высвобождается.</para>
        /// </summary>
        public event Disposal OnDispose;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable"/>.</para>
        /// </summary>
        /// <param name="action"><para>The <see cref="Action"/> delegate.</para><para>Делегат <see cref="Action"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(Action action)
        {
            OnDispose = (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action();
                }
            };
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable"/>.</para>
        /// </summary>
        /// <param name="disposal"><para>The <see cref="Disposal"/> delegate.</para><para>Делегат <see cref="Disposal"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(Disposal disposal) => OnDispose = disposal;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable"/>.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable() => OnDispose = _emptyDelegate;

        /// <summary>
        /// <para>Creates a new <see cref="Disposable"/> object initialized with specified delegate <see cref="Action"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable"/>, инициализированную с помощью указанного делегата <see cref="Action"/>.</para>
        /// </summary>
        /// <param name="action"><para>The <see cref="Action"/> delegate.</para><para>Делегат <see cref="Action"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable(Action action) => new Disposable(action);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable"/> object initialized with specified delegate <see cref="Disposal"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable"/>, инициализированную с помощью указанного делегата <see cref="Disposal"/>.</para>
        /// </summary>
        /// <param name="disposal"><para>The <see cref="Disposal"/> delegate.</para><para>Делегат <see cref="Disposal"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable(Disposal disposal) => new Disposable(disposal);

        /// <summary>
        /// <para>Disposes unmanaged resources.</para>
        /// <para>Высвобождает неуправляемые ресурсы.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from a developer.</para>
        /// <para>Значение определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A value that determines whether the object was released before calling this method.</para>
        /// <para>Значение определяющие был ли высвобожден объект до вызова этого метода.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Dispose(bool manual, bool wasDisposed) => RaiseOnDisposeEvent(manual, wasDisposed);

        /// <summary>
        /// <para>Raises an unmanaged resource dispose event.</para>
        /// <para>Генерирует событие высвобождения неуправляемых ресурсов.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from a developer.</para>
        /// <para>Значение определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A value that determines whether the object was released before calling this method.</para>
        /// <para>Значение определяющие был ли высвобожден объект до вызова этого метода.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaiseOnDisposeEvent(bool manual, bool wasDisposed) => OnDispose(manual, wasDisposed);

        /// <summary>
        /// <para>Attempts to dispose the specified object, as well as set the value of the variable containing this object to the default value.</para>
        /// <para>Выполняет попытку высвободить указанный объект, а так же установить значение переменной содержащей этот объект в значение по умолчанию.</para>
        /// </summary>
        /// <typeparam name="T"><para>Type of the specified object.</para><para>Тип указанного объекта.</para></typeparam>
        /// <param name="object"><para>The object to dispose.</para><para>Объект, который необходимо высвободить.</para></param>
        /// <returns><para>A value that determines whether the attempt to release the specified object was successful.</para><para>Значение, определяющие удачно ли была выполнена попытка высвободить указанный объект.</para></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryDisposeAndResetToDefault<T>(ref T @object)
        {
            var result = @object.TryDispose();
            if (result)
            {
                @object = default;
            }
            return result;
        }
    }
}
