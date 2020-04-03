namespace Platform::Disposables
{
    class Disposable : public DisposableBase
    {
        private: inline static const Disposal _emptyDelegate = (manual, wasDisposed) { return { }; }

        public: Platform::Delegates::MulticastDelegate<Disposal> OnDispose;

        public: Disposable(std::function<void()> action)
        {
            OnDispose = (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action();
                }
            };
        }

        public: Disposable(Disposal disposal) { OnDispose = disposal; }

        public: Disposable() { OnDispose = _emptyDelegate; }

        public: static implicit operator Disposable(std::function<void()> action) { return Disposable(action); }

        public: static implicit operator Disposable(Disposal disposal) { return Disposable(disposal); }

        protected: void Dispose(bool manual, bool wasDisposed) override { this->RaiseOnDisposeEvent(manual, wasDisposed); }

        protected: void RaiseOnDisposeEvent(bool manual, bool wasDisposed) { this->OnDispose(manual, wasDisposed); }

        public: template <typename T> static bool TryDisposeAndResetToDefault(T* object)
        {
            auto result = object.TryDispose();
            if (result)
            {
                object = 0;
            }
            return result;
        }
    };
}