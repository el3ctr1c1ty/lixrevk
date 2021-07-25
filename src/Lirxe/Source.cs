using System;
using System.Threading.Tasks;
using Lirxe.Model;
namespace Lirxe
{
    public abstract class Source
    {
        public delegate void RequestEvent(ActionContext actionCtx);
        public abstract event RequestEvent Request;

        public abstract event EventHandler OnRun;
        protected abstract void Run();

        public async Task RunAsync() => await Task.Run(Run);
    }
}