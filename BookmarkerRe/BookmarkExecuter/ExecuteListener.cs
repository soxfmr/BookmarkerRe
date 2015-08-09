namespace BookmarkerRe
{
    public delegate void Callback();
    public delegate void ActionCallback(string message);

    public class ExecuteListener
    {
        public Callback OnStart;
        public Callback OnFinish;
        public ActionCallback OnProcess;
        public ActionCallback OnError;
    }
}
