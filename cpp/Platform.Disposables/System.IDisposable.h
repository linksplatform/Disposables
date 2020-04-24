#pragma once

#ifndef PLATFORM_SYSTEM_IDISPOSABLE
#define PLATFORM_SYSTEM_IDISPOSABLE

namespace System
{
    class IDisposable
    {
    public:
        virtual void Dispose() = 0;
    };
}

#endif