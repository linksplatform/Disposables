using System;
using Xunit;

namespace Platform.Disposables.Tests
{
    public class DisposableClassTests
    {
        [Fact]
        public void Constructor_WithAction_ExecutesActionOnDispose()
        {
            var actionCalled = false;
            var disposable = new Disposable(() => actionCalled = true);
            
            disposable.Dispose();
            
            Assert.True(actionCalled);
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Constructor_WithDisposal_ExecutesDisposalOnDispose()
        {
            var manualDisposal = false;
            var wasDisposed = true; // Will be set correctly by disposal delegate
            var disposable = new Disposable((manual, wasDisp) => 
            {
                manualDisposal = manual;
                wasDisposed = wasDisp;
            });
            
            disposable.Dispose();
            
            Assert.True(manualDisposal);
            Assert.False(wasDisposed); // First disposal, so wasDisposed should be false
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void Constructor_Empty_DoesNotThrowOnDispose()
        {
            var disposable = new Disposable();
            
            disposable.Dispose(); // Should not throw
            
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void ImplicitOperator_FromAction_CreatesDisposable()
        {
            var actionCalled = false;
            Disposable disposable = (Action)(() => actionCalled = true);
            
            disposable.Dispose();
            
            Assert.True(actionCalled);
        }

        [Fact]
        public void ImplicitOperator_FromDisposal_CreatesDisposable()
        {
            var disposalCalled = false;
            Disposable disposable = (Disposal)((manual, wasDisposed) => disposalCalled = true);
            
            disposable.Dispose();
            
            Assert.True(disposalCalled);
        }

        [Fact]
        public void TryDisposeAndResetToDefault_WithDisposableObject_DisposesAndResetsToNull()
        {
            var innerDisposable = new Disposable(() => { });
            
            var result = Disposable.TryDisposeAndResetToDefault(ref innerDisposable);
            
            Assert.True(result);
            Assert.Null(innerDisposable);
        }

        [Fact]
        public void TryDisposeAndResetToDefault_WithNullObject_ReturnsTrueAndLeavesNull()
        {
            Disposable? nullDisposable = null;
            
            var result = Disposable.TryDisposeAndResetToDefault(ref nullDisposable);
            
            Assert.True(result); // TryDispose returns true for null
            Assert.Null(nullDisposable); // Remains null (default value)
        }

        [Fact]
        public void TryDisposeAndResetToDefault_WithNonDisposableObject_ReturnsTrueAndResetsToDefault()
        {
            var intValue = 42;
            
            var result = Disposable.TryDisposeAndResetToDefault(ref intValue);
            
            Assert.True(result); // TryDispose returns true for non-disposable objects
            Assert.Equal(0, intValue); // Should be reset to default(int) = 0
        }

        [Fact]
        public void OnDispose_IsTriggeredOnlyOnce_WhenNotPreviouslyDisposed()
        {
            var callCount = 0;
            var disposable = new Disposable((manual, wasDisposed) => 
            {
                if (!wasDisposed) callCount++;
            });
            
            disposable.Dispose();
            disposable.Destruct(); // Should not increment counter as wasDisposed will be true
            
            Assert.Equal(1, callCount);
        }

        [Fact]
        public void Dispose_WithAction_OnlyExecutesWhenNotPreviouslyDisposed()
        {
            var callCount = 0;
            var disposable = new Disposable(() => callCount++);
            
            disposable.Dispose();
            disposable.Destruct(); // Should not execute action as it was already disposed
            
            Assert.Equal(1, callCount);
        }

        [Fact]
        public void RaiseOnDisposeEvent_IsProtectedAndCallsOnDispose()
        {
            var eventTriggered = false;
            var disposable = new Disposable();
            disposable.OnDispose += (manual, wasDisposed) => eventTriggered = true;
            
            disposable.Dispose();
            
            Assert.True(eventTriggered);
        }
    }
}