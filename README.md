[![Codacy Badge](https://api.codacy.com/project/badge/Grade/3fdafa7bb9334ea4ac4ce242039d278a)](https://app.codacy.com/app/drakonard/Disposables?utm_source=github.com&utm_medium=referral&utm_content=linksplatform/Disposables&utm_campaign=Badge_Grade_Dashboard)
[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# Disposables ([русская версия](https://github.com/LinksPlatform/Disposables/blob/master/README.ru.md))

This library helps to make objects disposable in a fast, short, easy and safe way. `Platform.Disposables.DisposableBase` tries to dispose the object even if `Dispose` method was not called at both on instance destruction and `OnProcessExit` whatever comes first. And takes care your disposal logic executes only once by default, and if you really need it, you can allow multiple calls and attempts to dispose, by overriding corresponding properties. The `Platform.Disposables.IDisposable` interface extends the `System.IDisposable` with `IsDisposed` property and `Destruct` method. The `Destruct` method is designed to never throw exceptions, that makes it safe to use in class destructors. All ignored exceptions are available at `Platform.Exceptions.IgnoredExceptions` if you need to debug them.

Namespace: Platform.Disposables

Forked from: [Konard/LinksPlatform/Platform/Platform.Helpers/Disposables](https://github.com/Konard/LinksPlatform/tree/708f6143645333781adae0cad7ae998fefcd6317/Platform/Platform.Helpers/Disposables)

Package at NuGet: [Platform.Disposables](https://www.nuget.org/packages/Platform.Disposables)

## Examples

If you can use inheritance in your class. For example if you don`t have other base class inherited.

```C#
using Platform.Disposables;

namespace Examples
{
    public class DisposableBaseUsageExample : DisposableBase
    {
        protected override void DisposeCore(bool manual, bool wasDisposed)
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

If you do not have access to the internal structure of the object's class. You can use a disposable object container. The container is convertable back to the passed object at any time.

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
            // here array == null
        }

        private static void WorkWithObjectAndDispose(Disposable<int[]> disposableArray)
        {
            using (disposableArray)
            {
                int[] array = disposableArray;

                // use your object here

            } // call to () => array = null here
        }
    }
}
```
