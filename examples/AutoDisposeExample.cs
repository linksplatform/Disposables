using System;
using System.IO;
using Platform.Disposables;

namespace Platform.Disposables.Examples
{
    /// &lt;summary&gt;
    /// Example demonstrating automatic dispose code generation
    /// &lt;/summary&gt;
    [AutoDispose]
    public partial class AutoDisposeExample : DisposableBase
    {
        private readonly FileStream _fileStream;
        private readonly Disposable _disposableObject;
        private MemoryStream _memoryStream;

        public AutoDisposeExample()
        {
            // These fields will be automatically disposed when this object is disposed
            _fileStream = new FileStream(Path.GetTempFileName(), FileMode.Create);
            _disposableObject = new Disposable(() =&gt; Console.WriteLine("Custom disposal logic executed"));
            _memoryStream = new MemoryStream();
        }

        // No need to override Dispose(bool manual, bool wasDisposed) method
        // The source generator will create it automatically, disposing:
        // - _fileStream?.Dispose();
        // - _disposableObject?.Dispose(); 
        // - _memoryStream?.Dispose();
        // - _memoryStream = null; (only for non-readonly fields)
    }
}