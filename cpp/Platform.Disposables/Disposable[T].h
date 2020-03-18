namespace Platform::Disposables
{
    template <typename ...> class Disposable;
    template <typename T> class Disposable<T> : public Disposable
    {
        public: T Object
        {
            get;
        }

        public: Disposable(T object, std::function<void(T)> action)
        {
            void* = void*;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action(void*);
                }
            };
        }

        public: Disposable(T object, Action action) : base(action) { return Object = void*; }

        public: Disposable(T object, Disposal disposal) : base(disposal) { return Object = void*; }

        public: Disposable(T object) { void* = void*; }

        public: Disposable(std::tuple<T, Action<T>> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(std::tuple<T, Action> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(std::tuple<T, Disposal> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(T object) : Disposable(void*) { }

        public: operator T() const { return this->void*; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            base.Dispose(manual, wasDisposed);
            void*.TryDispose();
        }
    };
}