using System;

namespace Platform.Disposables
{
    public partial class Disposable<T> : Disposable
    {
        protected readonly T Object;

        public Disposable(T @object) => Object = @object;

        public Disposable(T @object, Action disposed) : base(disposed) => Object = @object;

        public Disposable(T @object, Action<T> disposed)
        {
            Object = @object;
            OnDispose += manual => disposed(Object);
        }

        public Disposable(T @object, DisposedDelegate disposed) : base(disposed) => Object = @object;

        public static implicit operator Disposable<T>(T @object) => new Disposable<T>(@object);

        public static implicit operator T(Disposable<T> disposable) => disposable.Object;

        protected override void DisposeCore(bool manual, bool wasDisposed)
        {
            base.DisposeCore(manual, wasDisposed);

            TryDispose(Object);
        }
    }

    public partial class Disposable<T>
    {
        public static Disposable<T> Create(T value, Action<T> dispose) => new Disposable<T>(value, dispose);
    }
}
