[![Версия NuGet и количество загрузок](https://buildstats.info/nuget/Platform.Disposables)](https://www.nuget.org/packages/Platform.Disposables)
[![Actions](https://github.com/linksplatform/Disposables/workflows/CD/badge.svg)](https://github.com/linksplatform/Disposables/actions?workflow=CD)
[![Codacy](https://api.codacy.com/project/badge/Grade/3fdafa7bb9334ea4ac4ce242039d278a)](https://app.codacy.com/app/drakonard/Disposables?utm_source=github.com&utm_medium=referral&utm_content=linksplatform/Disposables&utm_campaign=Badge_Grade_Dashboard)
[![CodeFactor](https://www.codefactor.io/repository/github/linksplatform/disposables/badge)](https://www.codefactor.io/repository/github/linksplatform/disposables)

# [Disposables](https://github.com/linksplatform/Disposables) ([english version](README.md))

Эта библиотека помогает сделать объекты высвобождаемыми быстрым, коротким, простым и безопасным способом. 

Абстрактный класс `Platform.Disposables.DisposableBase` пытается удалить объект, даже если метод `Dispose` нигде не вызывался пользователем, любо при вызове деструктора экземпляра, либо при возникновении события `OnProcessExit`, в зависимости от того, что произойдет раньше. И позаботится о том, чтобы логика удаления по умолчанию выполнялась только один раз, но, если она вам действительно нужна, вы можете разрешить несколько вызовов и попыток удаления, переопределив соответствующие свойства. 

Интерфейс `Platform.Disposables.IDisposable` расширяет `System.IDisposable` добавляя свойство `IsDisposed` и метод `Destruct`. Метод `Destruct` предназначен для того, чтобы не генерировать исключения, что делает его безопасным для использования в деструкторах классов. Все игнорируемые исключения доступны в `Platform.Disposables.IgnoredDisposables`, если вам нужно их отладить.

Пространство имён: [Platform.Disposables](https://linksplatform.github.io/Disposables/api/Platform.Disposables.html)

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

## [Документация](https://linksplatform.github.io/Disposables)
*   Интерфейс [IDisposable](https://linksplatform.github.io/Disposables/api/Platform.Disposables.IDisposable.html).
*   Абстрактный класс [DisposableBase](https://linksplatform.github.io/Disposables/api/Platform.Disposables.DisposableBase.html).
*   Класс [Disposable](https://linksplatform.github.io/Disposables/api/Platform.Disposables.Disposable.html).
*   Класс [Disposable\<T\>](https://linksplatform.github.io/Disposables/api/Platform.Disposables.Disposable-1.html).

[PDF файл](https://linksplatform.github.io/Disposables/Platform.Disposables.pdf) с кодом для электронных книг.

## Зависит от
*   [Platform.Disposables](https://github.com/linksplatform/Disposables)

## Зависимые библиотеки
*   [Platform.Collections](https://github.com/linksplatform/Collections)
