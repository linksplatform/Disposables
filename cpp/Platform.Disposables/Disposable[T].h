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
            Object = object;
            OnDispose += (manual, wasDisposed) =>
            {
                if (!wasDisposed)
                {
                    action(Object);
                }
            };
        }

        public: Disposable(T object, std::function<void()> action) : base(action) { return Object = object; }

        public: Disposable(T object, Disposal disposal) : base(disposal) { return Object = object; }

        public: Disposable(T object) { Object = object; }

        public: Disposable(std::tuple<T, Action<T>> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(std::tuple<T, Action> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(std::tuple<T, Disposal> tuple) : Disposable(std::get<1-1>(tuple), std::get<2-1>(tuple)) { }

        public: Disposable(T object) : Disposable(object) { }

        public: operator T() const { return this->Object; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            base.Dispose(manual, wasDisposed);
            Object.TryDispose();
        }
    };
}