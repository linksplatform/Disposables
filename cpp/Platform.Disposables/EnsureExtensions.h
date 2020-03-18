namespace Platform::Disposables
{
    class EnsureExtensions
    {
        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable disposable, std::string objectName, std::string message)
        {
            if (disposable.IsDisposed)
            {
                throw ObjectDisposedException(objectName, message);
            }
        }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable disposable, std::string objectName) { NotDisposed(root, disposable, objectName, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable disposable) { NotDisposed(root, disposable, {}, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable disposable, std::string objectName, std::string message) { Ensure.Always.NotDisposed(disposable, objectName, message); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable disposable, std::string objectName) { Ensure.Always.NotDisposed(disposable, objectName, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable disposable) { Ensure.Always.NotDisposed(disposable, {}, {}); }
    };
}
