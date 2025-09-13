using System;
using System.IO;
using Xunit;

namespace Platform.Disposables.Tests
{
    public class DisposableGenericTests
    {
        private class TestResource : System.IDisposable
        {
            public bool IsDisposed { get; private set; }
            
            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        [Fact]
        public void Constructor_WithObjectAndAction_StoresObjectAndExecutesActionOnDispose()
        {
            var resource = "test";
            var actionExecuted = false;
            var actionParameter = "";
            
            var disposable = new Disposable<string>(resource, obj => 
            {
                actionExecuted = true;
                actionParameter = obj;
            });
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
            Assert.Equal(resource, actionParameter);
        }

        [Fact]
        public void Constructor_WithObjectAndSimpleAction_ExecutesActionOnDispose()
        {
            var resource = "test";
            var actionExecuted = false;
            
            var disposable = new Disposable<string>(resource, () => actionExecuted = true);
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void Constructor_WithObjectAndDisposal_ExecutesDisposalOnDispose()
        {
            var resource = "test";
            var disposalExecuted = false;
            
            var disposable = new Disposable<string>(resource, (manual, wasDisposed) => 
            {
                if (!wasDisposed) disposalExecuted = true;
            });
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(disposalExecuted);
        }

        [Fact]
        public void Constructor_WithObjectOnly_StoresObject()
        {
            var resource = "test";
            var disposable = new Disposable<string>(resource);
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose(); // Should not throw
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithAction_CreatesDisposable()
        {
            var resource = "test";
            var actionExecuted = false;
            
            Disposable<string> disposable = (resource, (string obj) => actionExecuted = true);
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithSimpleAction_CreatesDisposable()
        {
            var resource = "test";
            var actionExecuted = false;
            
            Disposable<string> disposable = (resource, (Action)(() => actionExecuted = true));
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithDisposal_CreatesDisposable()
        {
            var resource = "test";
            var disposalExecuted = false;
            
            Disposable<string> disposable = (resource, (Disposal)((manual, wasDisposed) => 
            {
                if (!wasDisposed) disposalExecuted = true;
            }));
            
            Assert.Equal(resource, disposable.Object);
            
            disposable.Dispose();
            
            Assert.True(disposalExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromObject_CreatesDisposable()
        {
            var resource = "test";
            Disposable<string> disposable = resource;
            
            Assert.Equal(resource, disposable.Object);
        }

        [Fact]
        public void ImplicitOperator_ToObject_ReturnsObject()
        {
            var resource = "test";
            var disposable = new Disposable<string>(resource);
            
            string extracted = disposable;
            
            Assert.Equal(resource, extracted);
        }

        [Fact]
        public void Dispose_WithDisposableObject_DisposesContainedObject()
        {
            var resource = new TestResource();
            var disposable = new Disposable<TestResource>(resource);
            
            Assert.False(resource.IsDisposed);
            
            disposable.Dispose();
            
            Assert.True(resource.IsDisposed);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_WithNonDisposableObject_DoesNotThrow()
        {
            var resource = "test";
            var disposable = new Disposable<string>(resource);
            
            disposable.Dispose(); // Should not throw even though string is not disposable
            
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_CallsBaseDisposeAndThenTriesToDisposeObject()
        {
            var baseDisposeCalled = false;
            var resource = new TestResource();
            
            var disposable = new Disposable<TestResource>(resource, obj => baseDisposeCalled = true);
            
            disposable.Dispose();
            
            Assert.True(baseDisposeCalled); // Base dispose (action) should be called
            Assert.True(resource.IsDisposed); // Object should also be disposed
        }

        [Fact]
        public void Dispose_WithActionThatChecksWasDisposed_ExecutesOnlyWhenNotPreviouslyDisposed()
        {
            var resource = "test";
            var executionCount = 0;
            
            var disposable = new Disposable<string>(resource, obj => executionCount++);
            
            disposable.Dispose();
            disposable.Destruct(); // Should not increment count as it was already disposed
            
            Assert.Equal(1, executionCount);
        }

        [Fact]
        public void Object_Property_ReturnsStoredObject()
        {
            var resource = new object();
            var disposable = new Disposable<object>(resource);
            
            Assert.Same(resource, disposable.Object);
        }
    }
}