namespace Platform::Disposables::Tests
{
    TEST_CLASS(SystemTests)
    {
        public: TEST_METHOD(UsingSupportsNullTest)
        {
            using auto disposable = {} as IDisposable;
            Assert::IsTrue(disposable == nullptr);
        }
    };
}
