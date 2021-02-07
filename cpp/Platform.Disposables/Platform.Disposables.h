#pragma once

#ifndef PLATFORM_DISPOSABLES
#define PLATFORM_DISPOSABLES


//TODO: this real includes
    //#include <Platform.Converters.h>
    //#include <Platform.Hashing.h>
//

//TODO: this test includes
    #include "../../../Delegates/cpp/Platform.Delegates/Platform.Delegates.h"
    #include "../../../Exceptions/cpp/Platform.Exceptions/Platform.Exceptions.h"
//

#include "System.IDisposable.h"

#include "Disposal.h"
#include "IDisposable.h"
#include "Disposable.h"
#include "Disposable[T].h"
#include "Disposable[TPrimary, TAuxiliary].h"
#include "DisposableBase.h"
#include "EnsureExtensions.h"
#include "GenericObjectExtensions.h"
#include "IDisposableExtensions.h"

#endif
