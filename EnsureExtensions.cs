using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform.Exceptions;

#pragma warning disable IDE0060 // Remove unused parameter

namespace Platform.Disposables
{
    public static class EnsureExtensions
    {
        #region Always

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable) => NotDisposed(root, disposable, null, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable, string objectName) => NotDisposed(root, disposable, objectName, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotDisposed(this EnsureAlwaysExtensionRoot root, IDisposable disposable, string objectName, string message)
        {
            if (disposable.IsDisposed)
            {
                throw new ObjectDisposedException(objectName, message);
            }
        }

        #endregion

        #region OnDebug

        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable) => Ensure.Always.NotDisposed(disposable, null, null);

        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable, string objectName) => Ensure.Always.NotDisposed(disposable, objectName, null);

        [Conditional("DEBUG")]
        public static void NotDisposed(this EnsureOnDebugExtensionRoot root, IDisposable disposable, string objectName, string message) => Ensure.Always.NotDisposed(disposable, objectName, message);

        #endregion
    }
}
