﻿namespace Platform::Disposables
{
    template <typename ...> class Disposable;
    template <typename T> class Disposable<T> : public Disposable<>
    {
        public: const T Object;

        public: Disposable(T object, std::function<void(T)> action)
        {
            Object = object;
            OnDispose += [&](auto manual, auto wasDisposed)
            {
                if (!wasDisposed)
                {
                    action(Object);
                }
            };
        }

        public: Disposable(T object, std::function<void()> action) : Disposable<>(action) { return Object = object; }

        public: Disposable(T object, Disposal disposal) : Disposable<>(disposal) { return Object = object; }

        public: Disposable(T object) { Object = object; }

        public: Disposable(std::tuple<T, std::function<void(T)>> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple)) { }

        public: Disposable(std::tuple<T, std::function<void()>> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple)) { }

        public: Disposable(std::tuple<T, Disposal> tuple) : Disposable(std::get<0>(tuple), std::get<1>(tuple)) { }

        public: operator T() const { return this->Object; }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            base.Dispose(manual, wasDisposed);
            Object.TryDispose();
        }
    };
}