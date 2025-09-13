using System;
using Xunit;
using Platform.Exceptions;

namespace Platform.Disposables.Tests
{
    public class ExtensionTests
    {
        private class TestDisposable : DisposableBase
        {
            protected override void Dispose(bool manual, bool wasDisposed)
            {
                // Empty implementation for testing
            }
        }

        private class TestSystemDisposable : System.IDisposable
        {
            public bool IsDisposed { get; private set; }
            
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        #region IDisposableExtensions Tests

        [Fact]
        public void DisposeIfNotDisposed_WhenNotDisposed_CallsDispose()
        {
            var disposable = new TestDisposable();
            
            disposable.DisposeIfNotDisposed();
            
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void DisposeIfNotDisposed_WhenAlreadyDisposed_DoesNotCallDisposeAgain()
        {
            var disposable = new TestDisposable();
            disposable.Dispose(); // First disposal
            
            // This should not throw even though multiple dispose calls are not allowed by default
            disposable.DisposeIfNotDisposed();
            
            Assert.True(disposable.IsDisposed);
        }

        #endregion

        #region GenericObjectExtensions Tests

        [Fact]
        public void TryDispose_WithDisposableBase_ReturnsTrue()
        {
            var disposable = new TestDisposable();
            
            var result = disposable.TryDispose();
            
            Assert.True(result);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void TryDispose_WithSystemIDisposable_ReturnsTrue()
        {
            var disposable = new TestSystemDisposable();
            
            var result = disposable.TryDispose();
            
            Assert.True(result);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void TryDispose_WithNonDisposableObject_ReturnsTrue()
        {
            var obj = "test string";
            
            var result = obj.TryDispose();
            
            Assert.True(result); // Should return true even for non-disposable objects
        }

        [Fact]
        public void TryDispose_WithNull_ReturnsTrue()
        {
            string? nullObj = null;
            
            var result = nullObj.TryDispose();
            
            Assert.True(result); // Should return true for null
        }

        [Fact]
        public void DisposeIfPossible_WithDisposableObject_DisposesObject()
        {
            var disposable = new TestDisposable();
            
            disposable.DisposeIfPossible();
            
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void DisposeIfPossible_WithNonDisposableObject_DoesNotThrow()
        {
            var obj = "test string";
            
            obj.DisposeIfPossible(); // Should not throw
            
            // No assertion needed, just ensuring it doesn't throw
        }

        [Fact]
        public void TryDispose_WithExceptionThrowingDisposable_ReturnsFalse()
        {
            var disposable = new ThrowingDisposable();
            
            var result = disposable.TryDispose();
            
            Assert.False(result); // Should return false when exception is thrown and ignored
        }

        private class ThrowingDisposable : System.IDisposable
        {
            public void Dispose()
            {
                throw new InvalidOperationException("Test exception");
            }
        }

        #endregion

        #region EnsureExtensions Tests

        [Fact]
        public void EnsureAlways_NotDisposed_WithNotDisposedObject_DoesNotThrow()
        {
            var disposable = new TestDisposable();
            
            // Should not throw
            Ensure.Always.NotDisposed(disposable, "TestObject", "Test message");
        }

        [Fact]
        public void EnsureAlways_NotDisposed_WithDisposedObject_ThrowsObjectDisposedException()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.Always.NotDisposed(disposable, "TestObject", "Test message"));
            
            Assert.Equal("TestObject", exception.ObjectName);
            Assert.Contains("Test message", exception.Message);
        }

        [Fact]
        public void EnsureAlways_NotDisposed_WithObjectNameOnly_ThrowsWithCorrectObjectName()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.Always.NotDisposed(disposable, "TestObject"));
            
            Assert.Equal("TestObject", exception.ObjectName);
        }

        [Fact]
        public void EnsureAlways_NotDisposed_WithNoParameters_ThrowsWithEmptyObjectName()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.Always.NotDisposed(disposable));
            
            Assert.Equal("", exception.ObjectName);
        }

        [Fact]
        public void EnsureOnDebug_NotDisposed_InDebugMode_BehavesLikeEnsureAlways()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
#if DEBUG
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.OnDebug.NotDisposed(disposable, "TestObject", "Test message"));
            
            Assert.Equal("TestObject", exception.ObjectName);
            Assert.Contains("Test message", exception.Message);
#else
            // In release mode, should not throw
            Ensure.OnDebug.NotDisposed(disposable, "TestObject", "Test message");
#endif
        }

        [Fact]
        public void EnsureOnDebug_NotDisposed_WithObjectNameOnly()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
#if DEBUG
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.OnDebug.NotDisposed(disposable, "TestObject"));
            
            Assert.Equal("TestObject", exception.ObjectName);
#else
            // In release mode, should not throw
            Ensure.OnDebug.NotDisposed(disposable, "TestObject");
#endif
        }

        [Fact]
        public void EnsureOnDebug_NotDisposed_WithNoParameters()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
#if DEBUG
            var exception = Assert.Throws<ObjectDisposedException>(() => 
                Ensure.OnDebug.NotDisposed(disposable));
            
            Assert.Equal("", exception.ObjectName);
#else
            // In release mode, should not throw
            Ensure.OnDebug.NotDisposed(disposable);
#endif
        }

        #endregion
    }
}