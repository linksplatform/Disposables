using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.Disposables
{
    /// <summary>
    /// Represents disposable object that contains OnDispose event which is raised when the object itself is disposed.
    /// Представляет высвобождаемый объект, который содержит событие OnDispose, которое возникает при высвобождении самого объекта.
    /// </summary>
    public class Disposable : DisposableBase
    {
        private static readonly Disposal _emptyDelegate = (manual, wasDisposed) => { };

        public event Disposal OnDispose;

        public Disposable(Action disposed)
        {
            OnDispose = (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    disposed();
                }
            };
        }

        public Disposable(Disposal disposed) => OnDispose = disposed;

        public Disposable() => OnDispose = _emptyDelegate;

        public static implicit operator Disposable(Action action) => new Disposable(action);

        public static implicit operator Disposable(Disposal disposal) => new Disposable(disposal);

        protected override void Dispose(bool manual, bool wasDisposed) => OnDispose(manual, wasDisposed);

        protected void RaiseOnDisposeEvent(bool manual, bool wasDisposed) => OnDispose(manual, wasDisposed);

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