namespace Platform::Disposables
{
    template <typename ...> class Disposable;
    template<> class Disposable<> : public DisposableBase
    {
        private: inline static std::function<Disposal> _emptyDelegate = [](auto manual, auto wasDisposed) { };

        public: Platform::Delegates::MulticastDelegate<Disposal> OnDispose;

        public: Disposable(std::function<void()> action)
        {
            OnDispose = [&](auto manual, auto wasDisposed)
            {
                if (!wasDisposed)
                {
                    action();
                }
            };
        }

        public: Disposable(std::function<Disposal> disposal) { OnDispose = disposal; }

        public: Disposable() { OnDispose = _emptyDelegate; }

        public: Disposable(std::function<void()> action) : Disposable(action) { }

        public: Disposable(std::function<Disposal> disposal) : Disposable(disposal) { }

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