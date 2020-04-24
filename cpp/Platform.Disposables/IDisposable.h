namespace Platform::Disposables
{
    class IDisposable : public System::IDisposable
    {
    public:
        virtual bool IsDisposed() = 0;

        virtual void Destruct() = 0;
    };
}