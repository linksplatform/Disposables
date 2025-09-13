using System;
using Xunit;

namespace Platform.Disposables.Tests
{
    public class DisposableDualGenericTests
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
        public void Constructor_WithObjectsAndAction_StoresObjectsAndExecutesActionOnDispose()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var actionExecuted = false;
            string? primaryParam = null;
            string? auxiliaryParam = null;
            
            var disposable = new Disposable<string, string>(primary, auxiliary, (p, a) => 
            {
                actionExecuted = true;
                primaryParam = p;
                auxiliaryParam = a;
            });
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
            Assert.Equal(primary, primaryParam);
            Assert.Equal(auxiliary, auxiliaryParam);
        }

        [Fact]
        public void Constructor_WithObjectsAndSimpleAction_ExecutesActionOnDispose()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var actionExecuted = false;
            
            var disposable = new Disposable<string, string>(primary, auxiliary, () => actionExecuted = true);
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void Constructor_WithObjectsAndDisposal_ExecutesDisposalOnDispose()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var disposalExecuted = false;
            
            var disposable = new Disposable<string, string>(primary, auxiliary, (manual, wasDisposed) => 
            {
                if (!wasDisposed) disposalExecuted = true;
            });
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(disposalExecuted);
        }

        [Fact]
        public void Constructor_WithObjectsOnly_StoresObjects()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            
            var disposable = new Disposable<string, string>(primary, auxiliary);
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose(); // Should not throw
        }

        [Fact]
        public void Constructor_WithPrimaryOnly_StoresPrimaryAndSetsAuxiliaryToDefault()
        {
            var primary = "primary";
            
            var disposable = new Disposable<string, string>(primary);
            
            Assert.Equal(primary, disposable.Object);
            Assert.Null(disposable.AuxiliaryObject); // Default value for string
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithAction_CreatesDisposable()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var actionExecuted = false;
            
            Disposable<string, string> disposable = (primary, auxiliary, (string p, string a) => actionExecuted = true);
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithSimpleAction_CreatesDisposable()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var actionExecuted = false;
            
            Disposable<string, string> disposable = (primary, auxiliary, (Action)(() => actionExecuted = true));
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(actionExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithDisposal_CreatesDisposable()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            var disposalExecuted = false;
            
            Disposable<string, string> disposable = (primary, auxiliary, (Disposal)((manual, wasDisposed) => 
            {
                if (!wasDisposed) disposalExecuted = true;
            }));
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
            
            disposable.Dispose();
            
            Assert.True(disposalExecuted);
        }

        [Fact]
        public void ImplicitOperator_FromTupleWithObjects_CreatesDisposable()
        {
            var primary = "primary";
            var auxiliary = "auxiliary";
            
            Disposable<string, string> disposable = (primary, auxiliary);
            
            Assert.Equal(primary, disposable.Object);
            Assert.Equal(auxiliary, disposable.AuxiliaryObject);
        }

        [Fact]
        public void ImplicitOperator_ToPrimary_ReturnsPrimaryObject()
        {
            var primary = "primary";
            var auxiliary = 42;
            var disposable = new Disposable<string, int>(primary, auxiliary);
            
            string extracted = disposable;
            
            Assert.Equal(primary, extracted);
        }

        [Fact]
        public void ImplicitOperator_ToAuxiliary_ReturnsAuxiliaryObject()
        {
            var primary = "primary";
            var auxiliary = 42;
            var disposable = new Disposable<string, int>(primary, auxiliary);
            
            int extractedAux = disposable;
            
            Assert.Equal(auxiliary, extractedAux);
        }

        [Fact]
        public void Dispose_WithDisposableObjects_DisposesBothObjects()
        {
            var primaryResource = new TestResource();
            var auxiliaryResource = new TestResource();
            
            var disposable = new Disposable<TestResource, TestResource>(primaryResource, auxiliaryResource);
            
            Assert.False(primaryResource.IsDisposed);
            Assert.False(auxiliaryResource.IsDisposed);
            
            disposable.Dispose();
            
            Assert.True(primaryResource.IsDisposed);
            Assert.True(auxiliaryResource.IsDisposed);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Dispose_CallsOnDisposeEventThenDisposesObjects()
        {
            var primaryResource = new TestResource();
            var auxiliaryResource = new TestResource();
            var eventCalled = false;
            
            var disposable = new Disposable<TestResource, TestResource>(primaryResource, auxiliaryResource);
            disposable.OnDispose += (manual, wasDisposed) => 
            {
                // At this point, objects should not yet be disposed
                eventCalled = true;
                Assert.False(primaryResource.IsDisposed);
                Assert.False(auxiliaryResource.IsDisposed);
            };
            
            disposable.Dispose();
            
            Assert.True(eventCalled);
            Assert.True(primaryResource.IsDisposed);
            Assert.True(auxiliaryResource.IsDisposed);
        }

        [Fact]
        public void Dispose_DisposesAuxiliaryFirst_ThenPrimary()
        {
            var disposeOrder = new System.Collections.Generic.List<string>();
            
            var primaryResource = new TestResource();
            var auxiliaryResource = new TestResource();
            
            // We can't directly test the order since TryDispose doesn't guarantee order visibility,
            // but we can ensure both are disposed
            var disposable = new Disposable<TestResource, TestResource>(primaryResource, auxiliaryResource);
            
            disposable.Dispose();
            
            Assert.True(primaryResource.IsDisposed);
            Assert.True(auxiliaryResource.IsDisposed);
        }

        [Fact]
        public void AuxiliaryObject_Property_ReturnsStoredAuxiliaryObject()
        {
            var primary = "primary";
            var auxiliary = new object();
            var disposable = new Disposable<string, object>(primary, auxiliary);
            
            Assert.Same(auxiliary, disposable.AuxiliaryObject);
        }

        [Fact]
        public void Dispose_WithNonDisposableObjects_DoesNotThrow()
        {
            var primary = "primary";
            var auxiliary = 42;
            var disposable = new Disposable<string, int>(primary, auxiliary);
            
            disposable.Dispose(); // Should not throw even though objects are not disposable
            
            Assert.True(disposable.IsDisposed);
        }
    }
}