namespace Platform::Disposables
{
    interface IDisposable : System.IDisposable
    {
        bool IsDisposed
        {
            get;
        }

        virtual void Destruct() = 0;
    }
}