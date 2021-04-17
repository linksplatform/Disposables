using System;
using System.Runtime.CompilerServices;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Represents disposable container that disposes contained object when the container itself is disposed.</para>
    /// <para>Представляет высвобождаемый контейнер, который высвобождает содержащийся в нём объект при высвобождении самого контейнера.</para>
    /// </summary>
    public class Disposable<T> : Disposable
    {
        /// <summary>
        /// <para>Gets the object.</para>
        /// <para>Возвращает объект.</para>
        /// </summary>
        public T Object
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{T}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{T}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The object.</para><para>Объект.</para></param>
        /// <param name="action"><para>The <see cref="Action{T}"/> delegate.</para><para>Делегат <see cref="Action{T}"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(T @object, Action<T> action)
        {
            Object = @object;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action(Object);
                }
            };
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{T}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{T}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The object.</para><para>Объект.</para></param>
        /// <param name="action"><para>The <see cref="Action"/> delegate.</para><para>Делегат <see cref="Action"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(T @object, Action action) : base(action) => Object = @object;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{T}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{T}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The object.</para><para>Объект.</para></param>
        /// <param name="disposal"><para>The <see cref="Disposal"/> delegate.</para><para>Делегат <see cref="Disposal"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(T @object, Disposal disposal) : base(disposal) => Object = @object;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{T}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{T}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The object.</para><para>Объект.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(T @object) => Object = @object;

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{T}"/> object initialized with <see cref="ValueTuple{T, TDisposal}.Item1"/> as <see cref="Disposable{T}.Object"/> and <see cref="ValueTuple{T, TAction}.Item2"/> as delegate <see cref="Action{T}"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{T}"/>, инициализированную с помощью <see cref="ValueTuple{T, TDisposal}.Item1"/> как <see cref="Disposable{T}.Object"/> и <see cref="ValueTuple{T, TAction}.Item2"/> как делегат <see cref="Action{T}"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<T>(ValueTuple<T, Action<T>> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{T}"/> object initialized with <see cref="ValueTuple{T, TDisposal}.Item1"/> as <see cref="Disposable{T}.Object"/> and <see cref="ValueTuple{T, TAction}.Item2"/> as delegate <see cref="Action"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{T}"/>, инициализированную с помощью <see cref="ValueTuple{T, TDisposal}.Item1"/> как <see cref="Disposable{T}.Object"/> и <see cref="ValueTuple{T, TAction}.Item2"/> как делегат <see cref="Action"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<T>(ValueTuple<T, Action> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{T}"/> object initialized with <see cref="ValueTuple{T, TDisposal}.Item1"/> as <see cref="Disposable{T}.Object"/> and <see cref="ValueTuple{T, TDisposal}.Item2"/> as delegate <see cref="Disposal"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{T}"/>, инициализированную с помощью <see cref="ValueTuple{T, TDisposal}.Item1"/> как <see cref="Disposable{T}.Object"/> и <see cref="ValueTuple{T, TDisposal}.Item2"/> как делегат <see cref="Disposal"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<T>(ValueTuple<T, Disposal> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{T}"/> object initialized with specified object as <see cref="Disposable{T}.Object"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{T}"/>, инициализированную с помощью указанного объекта как <see cref="Disposable{T}.Object"/>.</para>
        /// </summary>
        /// <param name="object"><para>The object.</para><para>Объект.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<T>(T @object) => new Disposable<T>(@object);

        /// <summary>
        /// <para>Creates a new copy of the primary object (<see cref="Disposable{T}.Object"/>).</para>
        /// <para>Создаёт новую копию основного объекта (<see cref="Disposable{T}.Object"/>).</para>
        /// </summary>
        /// <param name="disposableContainer"><para>The disposable container.</para><para>Высвобождаемый контейнер.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(Disposable<T> disposableContainer) => disposableContainer.Object;

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
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            base.Dispose(manual, wasDisposed);
            Object.TryDispose();
        }
    }
}
