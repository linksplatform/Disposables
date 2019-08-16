using System;

namespace Platform.Disposables
{
    /// <summary>
    /// Represents disposable container that disposes contained object when the container itself is disposed.
    /// Представляет высвобождаемый контейнер, который высвобождает содержащийся в нём объект при высвобождении самого контейнера.
    /// </summary>
    public class Disposable<T> : Disposable
    {
        public T Object { get; }

        public Disposable(T @object, Action<T> disposed)
        {
            Object = @object;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    disposed(Object);
                }
            };
        }

        public Disposable(T @object, Action disposed) : base(disposed) => Object = @object;

        public Disposable(T @object, Disposal disposed) : base(disposed) => Object = @object;

        public Disposable(T @object) => Object = @object;

        public static implicit operator Disposable<T>(ValueTuple<T, Action<T>> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        public static implicit operator Disposable<T>(ValueTuple<T, Action> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        public static implicit operator Disposable<T>(ValueTuple<T, Disposal> tuple) => new Disposable<T>(tuple.Item1, tuple.Item2);

        public static implicit operator Disposable<T>(T @object) => new Disposable<T>(@object);

        public static implicit operator T(Disposable<T> disposable) => disposable.Object;

        protected override void Dispose(bool manual, bool wasDisposed)
        {
            base.Dispose(manual, wasDisposed);
            Object.TryDispose();
        }
    }
}