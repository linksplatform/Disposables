using System;
using System.Runtime.CompilerServices;

namespace Platform.Disposables
{
    /// <summary>
    /// <para>Represents disposable container that disposes two contained objects when the container itself is disposed.</para>
    /// <para>Представляет высвобождаемый контейнер, который высвобождает два содержащихся в нём объекта при высвобождении самого контейнера.</para>
    /// </summary>
    /// <typeparam name="TPrimary"><para>The primary object type.</para><para>Тип основного объекта.</para></typeparam>
    /// <typeparam name="TAuxiliary"><para>The auxiliary object type.</para><para>Тип вспомогательного объекта.</para></typeparam>
    public class Disposable<TPrimary, TAuxiliary> : Disposable<TPrimary>
    {
        /// <summary>
        /// <para>Gets the auxiliary object.</para>
        /// <para>Возвращает вспомогательный объект.</para>
        /// </summary>
        public TAuxiliary AuxiliaryObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{TPrimary, TAuxiliary}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The primary object.</para><para>Основной объект.</para></param>
        /// <param name="auxiliaryObject"><para>The auxiliary object.</para><para>Вспомогательный объект.</para></param>
        /// <param name="action"><para>The <see cref="Action{TPrimary, TAuxiliary}"/> delegate.</para><para>Делегат <see cref="Action{TPrimary, TAuxiliary}"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Action<TPrimary, TAuxiliary> action)
            : base(@object)
        {
            AuxiliaryObject = auxiliaryObject;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action(Object, AuxiliaryObject);
                }
            };
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{TPrimary, TAuxiliary}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The primary object.</para><para>Основной объект.</para></param>
        /// <param name="auxiliaryObject"><para>The auxiliary object.</para><para>Вспомогательный объект.</para></param>
        /// <param name="action"><para>The <see cref="Action"/> delegate.</para><para>Делегат <see cref="Action"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Action action) : base(@object, action) => AuxiliaryObject = auxiliaryObject;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{TPrimary, TAuxiliary}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The primary object.</para><para>Основной объект.</para></param>
        /// <param name="auxiliaryObject"><para>The auxiliary object.</para><para>Вспомогательный объект.</para></param>
        /// <param name="disposal"><para>The <see cref="Disposal"/> delegate.</para><para>Делегат <see cref="Disposal"/>.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject, Disposal disposal) : base(@object, disposal) => AuxiliaryObject = auxiliaryObject;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{TPrimary, TAuxiliary}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The primary object.</para><para>Основной объект.</para></param>
        /// <param name="auxiliaryObject"><para>The auxiliary object.</para><para>Вспомогательный объект.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject) : base(@object) => AuxiliaryObject = auxiliaryObject;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Disposable{TPrimary, TAuxiliary}"/> object.</para>
        /// <para>Инициализирует новый экземпляр объекта <see cref="Disposable{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="object"><para>The primary object.</para><para>Основной объект.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(TPrimary @object) : base(@object) { }

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{TPrimary, TAuxiliary}"/> object initialized with <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item1"/> as <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item2"/> as <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> and <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item3"/> as delegate <see cref="Action{TPrimary, TAuxiliary}"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{TPrimary, TAuxiliary}"/>, инициализированную с помощью <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item1"/> как <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item2"/> как <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> и <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item3"/> как делегат <see cref="Action{TPrimary, TAuxiliary}"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Action<TPrimary, TAuxiliary>> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{TPrimary, TAuxiliary}"/> object initialized with <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item1"/> as <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item2"/> as <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> and <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item3"/> as delegate <see cref="Action"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{TPrimary, TAuxiliary}"/>, инициализированную с помощью <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item1"/> как <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item2"/> как <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> и <see cref="ValueTuple{TPrimary, TAuxiliary, TAction}.Item3"/> как делегат <see cref="Action"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Action> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{TPrimary, TAuxiliary}"/> object initialized with <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item1"/> as <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item2"/> as <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> and <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item3"/> as delegate <see cref="Disposal"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{TPrimary, TAuxiliary}"/>, инициализированную с помощью <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item1"/> как <see cref="Disposable{TPrimary}.Object"/>, <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item2"/> как <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/> и <see cref="ValueTuple{TPrimary, TAuxiliary, TDisposal}.Item3"/> как делегат <see cref="Disposal"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary, Disposal> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2, tuple.Item3);

        /// <summary>
        /// <para>Creates a new <see cref="Disposable{TPrimary, TAuxiliary}"/> object initialized with <see cref="ValueTuple{TPrimary, TAuxiliary}.Item1"/> as <see cref="Disposable{TPrimary}.Object"/> and <see cref="ValueTuple{TPrimary, TAuxiliary}.Item2"/> as <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/>.</para>
        /// <para>Создает новый объект <see cref="Disposable{TPrimary, TAuxiliary}"/>, инициализированную с помощью <see cref="ValueTuple{TPrimary, TAuxiliary}.Item1"/> как <see cref="Disposable{TPrimary}.Object"/> и <see cref="ValueTuple{TPrimary, TAuxiliary}.Item2"/> как <see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/>.</para>
        /// </summary>
        /// <param name="tuple"><para>The tuple.</para><para>Кортеж.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Disposable<TPrimary, TAuxiliary>(ValueTuple<TPrimary, TAuxiliary> tuple) => new Disposable<TPrimary, TAuxiliary>(tuple.Item1, tuple.Item2);

        /// <summary>
        /// <para>Creates a new copy of the primary object (<see cref="Disposable{TPrimary}.Object"/>).</para>
        /// <para>Создаёт новую копию основного объекта (<see cref="Disposable{TPrimary}.Object"/>).</para>
        /// </summary>
        /// <param name="disposableContainer"><para>The disposable container.</para><para>Высвобождаемый контейнер.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> disposableContainer) => disposableContainer.Object;

        /// <summary>
        /// <para>Creates a new copy of the auxiliary object (<see cref="Disposable{TPrimary}.Object"/>).</para>
        /// <para>Создаёт новую копию вспомогательного объекта (<see cref="Disposable{TPrimary, TAuxiliary}.AuxiliaryObject"/>).</para>
        /// </summary>
        /// <param name="disposableContainer"><para>The disposable container.</para><para>Высвобождаемый контейнер.</para></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> disposableContainer) => disposableContainer.AuxiliaryObject;

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
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            RaiseOnDisposeEvent(manual, wasDisposed);
            AuxiliaryObject.TryDispose();
            Object.TryDispose();
        }
    }
}
