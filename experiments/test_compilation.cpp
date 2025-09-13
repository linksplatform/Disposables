#include <iostream>
#include <atomic>
#include <string>
#include <typeinfo>
#include <cstdint>

// Mock System::IDisposable
namespace System
{
    class IDisposable
    {
    public:
        virtual void Dispose() = 0;
    };
}

// Mock Platform::Disposables::IDisposable
namespace Platform::Disposables
{
    class IDisposable : public System::IDisposable
    {
    public:
        virtual bool IsDisposed() = 0;
        virtual void Destruct() = 0;
    };
}

// Mock Platform::Exceptions::ExceptionExtensions
namespace Platform::Exceptions
{
    class ExceptionExtensions
    {
    public:
        static void Ignore(const std::exception& exception)
        {
            // Mock implementation
        }
    };

    class Ensure
    {
    public:
        static Ensure Always;
    };

    Ensure Ensure::Always;
}

// Mock Platform::Disposables::EnsureExtensions
namespace Platform::Disposables
{
    class EnsureExtensions
    {
    public:
        static void NotDisposed(Platform::Exceptions::Ensure& ensure, void* obj, const std::string& name, const std::string& message)
        {
            // Mock implementation - throw an exception
            throw std::runtime_error(message);
        }
    };
}

// Include the actual DisposableBase
#include "../cpp/Platform.Disposables/DisposableBase.h"

// Test implementation
class TestDisposable : public Platform::Disposables::DisposableBase
{
protected:
    void Dispose(bool manual, bool wasDisposed) override
    {
        std::cout << "TestDisposable::Dispose called with manual=" << manual << ", wasDisposed=" << wasDisposed << std::endl;
    }
};

int main()
{
    TestDisposable test;
    std::cout << "IsDisposed before: " << test.IsDisposed() << std::endl;
    test.Platform::Disposables::DisposableBase::Dispose();
    std::cout << "IsDisposed after: " << test.IsDisposed() << std::endl;
    return 0;
}