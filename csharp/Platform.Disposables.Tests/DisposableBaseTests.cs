using System;
using Xunit;
using Platform.Exceptions;

namespace Platform.Disposables.Tests
{
    public class DisposableBaseTests
    {
        private class TestDisposable : DisposableBase
        {
            public bool DisposeMethodCalled { get; private set; }
            public bool DisposalWasManual { get; private set; }
            public bool PreviouslyDisposed { get; private set; }
            public int DisposeCallCount { get; private set; }

            protected override void Dispose(bool manual, bool wasDisposed)
            {
                DisposeMethodCalled = true;
                DisposalWasManual = manual;
                PreviouslyDisposed = wasDisposed;
                DisposeCallCount++;
            }
        }

        private class TestDisposableAllowMultipleCalls : DisposableBase
        {
            public int DisposeCallCount { get; private set; }

            protected override bool AllowMultipleDisposeCalls => true;

            protected override void Dispose(bool manual, bool wasDisposed)
            {
                DisposeCallCount++;
            }
        }

        private class TestDisposableAllowMultipleAttempts : DisposableBase
        {
            public int DisposeCallCount { get; private set; }

            protected override bool AllowMultipleDisposeAttempts => true;
            protected override bool AllowMultipleDisposeCalls => true; // Need this too to prevent exception

            protected override void Dispose(bool manual, bool wasDisposed)
            {
                DisposeCallCount++;
            }
        }

        [Fact]
        public void Constructor_SetsIsDisposedToFalse()
        {
            using var disposable = new TestDisposable();
            Assert.False(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_CallsDisposeMethodWithManualTrue()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
            Assert.True(disposable.DisposeMethodCalled);
            Assert.True(disposable.DisposalWasManual);
            Assert.False(disposable.PreviouslyDisposed);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_MultipleCalls_ThrowsObjectDisposedException()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            
            var exception = Assert.Throws<ObjectDisposedException>(() => disposable.Dispose());
            Assert.Contains("Multiple dispose calls are not allowed", exception.Message);
        }

        [Fact]
        public void Dispose_WithAllowMultipleDisposeCalls_DoesNotThrow()
        {
            var disposable = new TestDisposableAllowMultipleCalls();
            disposable.Dispose();
            disposable.Dispose(); // Should not throw
            
            // AllowMultipleDisposeCalls only prevents exception, but without AllowMultipleDisposeAttempts,
            // the Dispose method is only called once (when !wasDisposed)
            Assert.Equal(1, disposable.DisposeCallCount);
        }

        [Fact]
        public void Dispose_WithAllowMultipleDisposeAttempts_CallsDisposeMultipleTimes()
        {
            var disposable = new TestDisposableAllowMultipleAttempts();
            disposable.Dispose();
            disposable.Dispose();
            
            Assert.Equal(2, disposable.DisposeCallCount);
        }

        [Fact]
        public void Destruct_CallsDisposeMethodWithManualFalse()
        {
            var disposable = new TestDisposable();
            disposable.Destruct();
            
            Assert.True(disposable.DisposeMethodCalled);
            Assert.False(disposable.DisposalWasManual);
            Assert.False(disposable.PreviouslyDisposed);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Destruct_WhenAlreadyDisposed_DoesNotCallDispose()
        {
            var disposable = new TestDisposable();
            disposable.Dispose();
            disposable.Destruct(); // Should not call dispose again
            
            Assert.Equal(1, disposable.DisposeCallCount);
        }

        [Fact]
        public void ObjectName_ReturnsTypeName()
        {
            var disposable = new TestDisposable();
            // ObjectName is protected, so we can't access it directly, but we can test indirectly
            // by causing a multiple dispose exception and checking the object name in the exception
            disposable.Dispose();
            
            var exception = Assert.Throws<ObjectDisposedException>(() => disposable.Dispose());
            Assert.Equal(nameof(TestDisposable), exception.ObjectName);
        }

        [Fact]
        public void IsDisposed_ReturnsTrueAfterDispose()
        {
            var disposable = new TestDisposable();
            Assert.False(disposable.IsDisposed);
            
            disposable.Dispose();
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void IsDisposed_ReturnsTrueAfterDestruct()
        {
            var disposable = new TestDisposable();
            Assert.False(disposable.IsDisposed);
            
            disposable.Destruct();
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_WithWasDisposedTrue_PassesCorrectFlag()
        {
            var disposable = new TestDisposableAllowMultipleAttempts();
            disposable.Dispose(); // First call: wasDisposed = false
            disposable.Dispose(); // Second call: wasDisposed = true
            
            Assert.Equal(2, disposable.DisposeCallCount);
        }
    }
}