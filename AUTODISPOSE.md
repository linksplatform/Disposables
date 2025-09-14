# AutoDispose Source Generator

This feature implements automatic dispose code generation for classes inheriting from `DisposableBase`.

## Usage

1. Apply the `[AutoDispose]` attribute to your class
2. Make the class `partial`
3. The source generator will automatically create a `Dispose(bool manual, bool wasDisposed)` method that:
   - Finds all fields implementing `IDisposable` or `Platform.Disposables.IDisposable`
   - Calls `Dispose()` on each field using null-conditional operator (`?.`)
   - Sets non-readonly fields to `null` after disposal

## Example

```csharp
[AutoDispose]
public partial class MyClass : DisposableBase
{
    private readonly FileStream _readOnlyField;
    private MemoryStream _writableField;

    // Generated Dispose method will be:
    // protected override void Dispose(bool manual, bool wasDisposed)
    // {
    //     if (!wasDisposed)
    //     {
    //         _readOnlyField?.Dispose();
    //         _writableField?.Dispose();
    //         _writableField = null;
    //     }
    // }
}
```

## Benefits

- Reduces boilerplate code
- Prevents memory leaks from forgotten disposals
- Automatically handles null checks
- Respects readonly field constraints
- Compile-time code generation for zero runtime overhead