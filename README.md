[![NuGet Version and Downloads count](https://buildstats.info/nuget/Platform.Disposables)](https://www.nuget.org/packages/Platform.Disposables)
[![Actions Status](https://github.com/linksplatform/Disposables/workflows/CD/badge.svg)](https://github.com/linksplatform/Disposables/actions?workflow=CD)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/3fdafa7bb9334ea4ac4ce242039d278a)](https://app.codacy.com/app/drakonard/Disposables?utm_source=github.com&utm_medium=referral&utm_content=linksplatform/Disposables&utm_campaign=Badge_Grade_Dashboard)
[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# [Disposables](https://github.com/linksplatform/Disposables) ([русская версия](README.ru.md))

This library helps to make objects disposable in a fast, short, easy and safe way.

`Platform.Disposables.DisposableBase` abstract class tries to dispose the object at both on instance destruction and `OnProcessExit` whatever comes first even if `Dispose` method was not called anywhere by user. And takes care your disposal logic executes only once by default, and if you really need it, you can allow multiple calls and attempts to dispose, by overriding corresponding properties.

The `Platform.Disposables.IDisposable` interface extends the `System.IDisposable` with `IsDisposed` property and `Destruct` method. The `Destruct` method is designed to never throw exceptions, that makes it safe to use in class destructors. All ignored exceptions are available at `Platform.Disposables.IgnoredDisposables` if you need to debug them.

Namespace: [Platform.Disposables](https://linksplatform.github.io/Disposables/csharp/api/Platform.Disposables.html)

Forked from: [Konard/LinksPlatform/Platform/Platform.Helpers/Disposables](https://github.com/Konard/LinksPlatform/tree/708f6143645333781adae0cad7ae998fefcd6317/Platform/Platform.Helpers/Disposables)

Package at NuGet: [Platform.Disposables](https://www.nuget.org/packages/Platform.Disposables)

## Examples

If you can use inheritance in your class. For example if you don't have other base class inherited.

```C#
using Platform.Disposables;

namespace Examples
{
    public class DisposableBaseUsageExample : DisposableBase
    {
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            // Dispose logic
        }
    }
}
```

If you cannot use inheritance in your class. For example if you have other base class inherited.

```C#
using Platform.Disposables;

namespace Examples
{
    public class DisposableUsageExample : System.IDisposable
    {
        private readonly Disposable _disposable;

        public DisposableUsageExample() => _disposable = new Disposable(Disposed);

        public void Dispose() => _disposable.Dispose();

        ~DisposableUsageExample() => _disposable.Destruct();

        private void Disposed(bool manual, bool wasDisposed)
        {
            // Dispose logic
        }
    }
}
```

If you do not have access to the internal structure of the object's class. You can use a disposable object container. The container is convertible back to the passed object at any time.

```C#
using Platform.Disposables;

namespace Examples
{
    public class Examples
    {
        public static void UseAndDispose()
        {
            var array = new int[] { 1, 2, 3 };
            Disposable<int[]> disposableArray = (array, () => array = null);
            WorkWithObjectAndDispose(disposableArray);
            // Here array == null
        }

        private static void WorkWithObjectAndDispose(Disposable<int[]> disposableArray)
        {
            using (disposableArray)
            {
                int[] array = disposableArray;

                // Use your object here

            } // Here there is call to () => array = null
        }
    }
}
```

## [Documentation](https://linksplatform.github.io/Disposables)
*   Interface [IDisposable](https://linksplatform.github.io/Disposables/csharp/api/Platform.Disposables.IDisposable.html).
*   Abstract class [DisposableBase](https://linksplatform.github.io/Disposables/csharp/api/Platform.Disposables.DisposableBase.html).
*   Class [Disposable](https://linksplatform.github.io/Disposables/csharp/api/Platform.Disposables.Disposable.html).
*   Class [Disposable\<T\>](https://linksplatform.github.io/Disposables/csharp/api/Platform.Disposables.Disposable-1.html).

[PDF file](https://linksplatform.github.io/Disposables/csharp/Platform.Disposables.pdf) with code for e-readers.

## Depend on
*   [Platform.Disposables](https://github.com/linksplatform/Disposables)

## Dependent libraries
*   [Platform.Collections](https://github.com/linksplatform/Collections)
