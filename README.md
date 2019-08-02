[![Codacy Badge](https://api.codacy.com/project/badge/Grade/3fdafa7bb9334ea4ac4ce242039d278a)](https://app.codacy.com/app/drakonard/Disposables?utm_source=github.com&utm_medium=referral&utm_content=linksplatform/Disposables&utm_campaign=Badge_Grade_Dashboard)
[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# Disposables ([русская версия](https://github.com/LinksPlatform/Disposables/blob/master/README.ru.md))

This library helps to make objects disposable in fast and easy ways. It also extends the `System.IDisposable` with `IsDisposed` property and `Destruct` method.

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

        private void Disposed(bool manual)
        {
            // Dispose logic
        }
    }
}
```

If you do not have access to the internal structure of the object's class. You can use a disposable object container. The container is convertable to the passed object back at any time.

```C#
using Platform.Disposables;

namespace Examples
{
    public class Examples
    {
        public static void UseAndDispose()
        {
            var array = new int[] { 1, 2, 3 };
            void onArrayDispose(bool manual, bool wasDisposed)
            {
                if (!wasDisposed)
                {
                    array = null;
                }
            }
            var disposableArray = new Disposable<int[]>(array, onArrayDispose);
            WorkWithObjectAndDispose(disposableArray);
            // here array == null
        }

        private static void WorkWithObjectAndDispose(Disposable<int[]> disposableArray)
        {
            using (disposableArray)
            {
                int[] array = disposableArray;

                // use your object here

            } // call to onArrayDispose here
        }
    }
}
```
