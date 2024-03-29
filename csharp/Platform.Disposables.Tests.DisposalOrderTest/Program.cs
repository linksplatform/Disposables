using System;
using System.IO;
using Platform.IO;

#pragma warning disable IDE0067 // Dispose objects before losing scope

namespace Platform.Disposables.Tests.DisposalOrderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "the.log", "false" };
            }
            if (args.Length == 1)
            {
                args = new[] { args[0], "false" };
            }
            var logPath = args[0];
            var waitForCancellation = bool.Parse(args[1]);
            using var consoleCancellationHandler = new ConsoleCancellation();
            var disposable1 = new Disposable(() => { File.AppendAllText(logPath, "1"); }); //-V3114
            var disposable2 = new Disposable(() => { File.AppendAllText(logPath, "2"); }); //-V3114
            Console.WriteLine(disposable1.IsDisposed && disposable2.IsDisposed); // Ensure objects are not optimized away
            if (waitForCancellation)
            {
                consoleCancellationHandler.Wait();
            }
        }
    }
}
