#ifndef DISPOSABLES_IDISPOSABLE_H
#define DISPOSABLES_IDISPOSABLE_H

#include "System.IDisposable.h"

namespace Platform::Disposables
{
    class IDisposable : public System::IDisposable
    {
    public:
        virtual bool IsDisposed() = 0;

        virtual void Destruct() = 0;
    };
}

#endif