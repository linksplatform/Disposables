namespace Platform::Disposables
{
    template <typename ...> class Disposable;
    template <typename TPrimary, typename TAuxiliary> class Disposable<TPrimary, TAuxiliary> : public Disposable<TPrimary>
    {
        public: TAuxiliary AuxiliaryObject
        {
            get;
        }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, Action<TPrimary, TAuxiliary> action)
            : base(object)
        {
            AuxiliaryObject = auxiliaryObject;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    this->action(Object, AuxiliaryObject);
                }
            };
        }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, std::function<void()> action) : base(object, action) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, Disposal disposal) : base(object, disposal) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject) : base(object) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object) : base(object) { }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Action<TPrimary, TAuxiliary>> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Action> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Disposal> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple)); }

        public: static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> disposableContainer) { return disposableContainer.Object; }

        public: static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> disposableContainer) { return disposableContainer.AuxiliaryObject; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            this->RaiseOnDisposeEvent(manual, wasDisposed);
            AuxiliaryObject.TryDispose();
            Object.TryDispose();
        }
    };
}
