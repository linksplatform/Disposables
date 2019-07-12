# Disposables ([русская версия](https://github.com/LinksPlatform/Disposables/blob/master/README.ru.md))

Forked from: https://github.com/Konard/LinksPlatform/tree/708f6143645333781adae0cad7ae998fefcd6317/Platform/Platform.Helpers/Disposables

Namespace: Platform.Disposables

Package at NuGet: https://www.nuget.org/packages/Platform.Disposables

## Examples

```C#
using Platform.Disposables;

namespace Examples
{
    internal class DisposableBaseUsageExample : DisposableBase
    {
        protected override void DisposeCore(bool manual, bool wasDisposed)
        {
            // Dispose logic
        }
    }
}
```

```C#
using Platform.Disposables;

namespace Examples
{
    internal class DisposableUsageExample : System.IDisposable
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
