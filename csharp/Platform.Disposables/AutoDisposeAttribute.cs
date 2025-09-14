using System;

namespace Platform.Disposables
{
    /// &lt;summary&gt;
    /// &lt;para&gt;Indicates that the class should have automatic dispose code generation for all disposable fields.&lt;/para&gt;
    /// &lt;para&gt;Указывает, что для класса должен быть автоматически сгенерирован код для высвобождения всех высвобождаемых полей.&lt;/para&gt;
    /// &lt;/summary&gt;
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AutoDisposeAttribute : Attribute
    {
    }
}