#ifndef DISPOSABLES_ENSURE_EXTENSION_H
#define DISPOSABLES_ENSURE_EXTENSION_H


namespace Platform::Disposables
{
    class EnsureExtensions
    {
        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable* disposable, std::string objectName, std::string message)
        {
            if (disposable->IsDisposed())
            {
                throw std::runtime_error(std::string("Attempt to access disposed object [").append(objectName).append("]: ").append(message).append(1, '.'));
            }
        }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable* disposable, std::string objectName) { NotDisposed(root, disposable, objectName, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureAlwaysExtensionRoot root, IDisposable* disposable) { NotDisposed(root, disposable, {}, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable* disposable, std::string objectName, std::string message) { Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, disposable, objectName, message); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable* disposable, std::string objectName) { Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, disposable, objectName, {}); }

        public: static void NotDisposed(Platform::Exceptions::ExtensionRoots::EnsureOnDebugExtensionRoot root, IDisposable* disposable) { Platform::Disposables::EnsureExtensions::NotDisposed(Platform::Exceptions::Ensure::Always, disposable, {}, {}); }
    };
}

#endif