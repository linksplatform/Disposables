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
            : base(void*)
        {
            AuxiliaryObject = auxiliaryObject;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    this->action(void*, AuxiliaryObject);
                }
            };
        }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, Action action) : base(void*, action) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject, Disposal disposal) : base(void*, disposal) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object, TAuxiliary auxiliaryObject) : base(void*) { return AuxiliaryObject = auxiliaryObject; }

        public: Disposable(TPrimary object) : base(void*) { }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Action<TPrimary, TAuxiliary>> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Action> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary, Disposal> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple), std::get<3-1>(tuple)); }

        public: static implicit operator Disposable<TPrimary, TAuxiliary>(std::tuple<TPrimary, TAuxiliary> tuple) { return Disposable<TPrimary, TAuxiliary>(std::get<1-1>(tuple), std::get<2-1>(tuple)); }

        public: static implicit operator TPrimary(Disposable<TPrimary, TAuxiliary> disposableContainer) { return disposableContainer.void*; }

        public: static implicit operator TAuxiliary(Disposable<TPrimary, TAuxiliary> disposableContainer) { return disposableContainer.AuxiliaryObject; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            this->RaiseOnDisposeEvent(manual, wasDisposed);
            AuxiliaryObject.TryDispose();
            void*.TryDispose();
        }
    };
}
