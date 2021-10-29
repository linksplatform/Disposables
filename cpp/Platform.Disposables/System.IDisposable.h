#pragma once

namespace System
{
    class IDisposable
    {
    public:
        virtual void Dispose() = 0;
    };
}