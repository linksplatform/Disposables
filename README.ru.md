[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# Disposables ([english version](https://github.com/LinksPlatform/Disposables/blob/master/README.md))

Пространство имён: Platform.Disposables

Ответвление от: [Konard/LinksPlatform/Platform/Platform.Helpers/Disposables](https://github.com/Konard/LinksPlatform/tree/708f6143645333781adae0cad7ae998fefcd6317/Platform/Platform.Helpers/Disposables)

Пакет в NuGet: [Platform.Disposables](https://www.nuget.org/packages/Platform.Disposables)

## Примеры

```C#
using Platform.Disposables;

namespace Examples
{
    public class DisposableBaseUsageExample : DisposableBase
    {
        protected override void DisposeCore(bool manual, bool wasDisposed)
        {
            // Логика высвобождения памяти
        }
    }
}
```

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
            // Логика высвобождения памяти
        }
    }
}
```
