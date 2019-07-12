namespace Platform.Disposables
{
    public class Disposable<TPrimary, TAuxiliary> : Disposable<TPrimary>
    {
        protected readonly TAuxiliary AuxiliaryObject;

        public Disposable(TPrimary @object)
            : base(@object)
        {
        }

        public Disposable(TPrimary @object, TAuxiliary auxiliaryObject) : this(@object) => AuxiliaryObject = auxiliaryObject;

        public static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> autoDisposable) => autoDisposable.Object;

        public static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> autoDisposable) => autoDisposable.AuxiliaryObject;

        protected override void DisposeCore(bool manual, bool wasDisposed)
        {
            base.DisposeCore(manual, wasDisposed);

            TryDispose(AuxiliaryObject);
        }
    }
}
