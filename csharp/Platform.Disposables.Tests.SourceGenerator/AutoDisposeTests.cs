using System;
using System.IO;
using Xunit;

namespace Platform.Disposables.Tests.SourceGenerator
{
    public class AutoDisposeTests
    {
        [Fact]
        public void AutoDispose_DisposesAllFields_WhenObjectIsDisposed()
        {
            var testDisposable = new TestAutoDispose();
            
            Assert.False(testDisposable.IsDisposed);
            Assert.False(testDisposable.DisposableField1.IsDisposed);
            Assert.False(testDisposable.DisposableField2.IsDisposed);
            Assert.False(testDisposable.SystemDisposableField.WasDisposed);
            
            testDisposable.Dispose();
            
            Assert.True(testDisposable.IsDisposed);
            Assert.True(testDisposable.DisposableField1.IsDisposed);
            Assert.True(testDisposable.DisposableField2.IsDisposed);
            Assert.True(testDisposable.SystemDisposableField.WasDisposed);
        }

        [Fact]
        public void AutoDispose_HandlesNullFields_Gracefully()
        {
            var testDisposable = new TestAutoDisposeWithNulls();
            
            Assert.False(testDisposable.IsDisposed);
            
            // Should not throw when disposing null fields
            testDisposable.Dispose();
            
            Assert.True(testDisposable.IsDisposed);
        }
    }

    [AutoDispose]
    public partial class TestAutoDispose : DisposableBase
    {
        public readonly Disposable DisposableField1;
        public readonly Disposable DisposableField2;
        public readonly TestSystemDisposable SystemDisposableField;

        public TestAutoDispose()
        {
            DisposableField1 = new Disposable();
            DisposableField2 = new Disposable();
            SystemDisposableField = new TestSystemDisposable();
        }
    }

    [AutoDispose]
    public partial class TestAutoDisposeWithNulls : DisposableBase
    {
        public readonly Disposable? DisposableField1 = null;
        public readonly TestSystemDisposable? SystemDisposableField = null;
    }

    public class TestSystemDisposable : System.IDisposable
    {
        public bool WasDisposed { get; private set; }

        public void Dispose()
        {
            WasDisposed = true;
        }
    }
}