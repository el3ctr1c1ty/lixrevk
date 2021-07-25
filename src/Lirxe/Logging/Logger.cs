namespace Lirxe.Logging
{
    public abstract class Logger
    {
        public abstract void Log(string l);
        public abstract void Error(string e);
        public abstract void Warn(string w);
        public abstract void Info(string i);
        public abstract void Debug(string d);
        public abstract void StartJob(string j);
        public abstract void FinishJob(string j);

        public void L(object l) => Log(l.ToString());
        public void E(object e) => Error(e.ToString());
        public void W(object w) => Warn(w.ToString());
        public void I(object i) => Info(i.ToString());
        public void D(object d) => Debug(d.ToString());

        public void Sj(object j) => StartJob(j.ToString());
        public void Fj(object j) => FinishJob(j.ToString());

        // ReSharper disable InconsistentNaming
        public void l(object l) => L(l);
        public void e(object e) => E(e);
        public void w(object w) => W(w);
        public void i(object i) => I(i);
        public void d(object d) => D(d);

        public void sj(object j) => Sj(j);

        public void fj(object j) => Fj(j);
        // ReSharper restore InconsistentNaming
    }
}