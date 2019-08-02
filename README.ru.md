[![Codacy Badge](https://api.codacy.com/project/badge/Grade/3fdafa7bb9334ea4ac4ce242039d278a)](https://app.codacy.com/app/drakonard/Disposables?utm_source=github.com&utm_medium=referral&utm_content=linksplatform/Disposables&utm_campaign=Badge_Grade_Dashboard)
[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# Disposables ([english version](README.md))

Эта библиотека помогает сделать объекты высвобождаемыми быстрым, коротким, простым и безопасным способом. 

Абстрактный класс `Platform.Disposables.DisposableBase` пытается удалить объект, даже если метод `Dispose` нигде не вызывался пользователем, любо при вызове деструктора экземпляра, либо при возникновении события `OnProcessExit`, в зависимости от того, что произойдет раньше. И позаботится о том, чтобы логика удаления по умолчанию выполнялась только один раз, но, если она вам действительно нужна, вы можете разрешить несколько вызовов и попыток удаления, переопределив соответствующие свойства. 

Интерфейс `Platform.Disposables.IDisposable` расширяет `System.IDisposable` добавляя свойство `IsDisposed` и метод `Destruct`. Метод `Destruct` предназначен для того, чтобы не генерировать исключения, что делает его безопасным для использования в деструкторах классов. Все игнорируемые исключения доступны в `Platform.Exceptions.IgnoredExceptions`, если вам нужно их отладить.

Пространство имён: Platform.Disposables

Ответвление от: [Konard/LinksPlatform/Platform/Platform.Helpers/Disposables](https://github.com/Konard/LinksPlatform/tree/708f6143645333781adae0cad7ae998fefcd6317/Platform/Platform.Helpers/Disposables)

Пакет в NuGet: [Platform.Disposables](https://www.nuget.org/packages/Platform.Disposables)

## Примеры

Если вы можете использовать наследование в вашем классе. Например, если у вас нет другого наследуемого базового класса.

```C#
using Platform.Disposables;

namespace Examples
{
    public class DisposableBaseUsageExample : DisposableBase
    {
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            // Логика высвобождения
        }
    }
}
```

Если вы не можете использовать наследование в вашем классе. Например, если у вас есть другой базовый класс от которого унаследован ваш класс.

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
            // Логика высвобождения
        }
    }
}
```

Если у вас нет доступа к внутренней структуре класса объекта. Вы можете использовать высвобождаемый контейнер для вашего объекта. Контейнер может быть преобразован обратно в переданный объект в любое время.

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
            // Здесь array == null
        }

        private static void WorkWithObjectAndDispose(Disposable<int[]> disposableArray)
        {
            using (disposableArray)
            {
                int[] array = disposableArray;

                // Используйте ваш объект здесь

            } // Здесь вызывается () => array = null
        }
    }
}
```
