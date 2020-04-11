namespace Platform::Disposables
{
    interface IDisposable : System.IDisposable
    {
        const bool IsDisposed;

        virtual void Destruct() = 0;
    }
}