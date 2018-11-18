using SIS.Framework;

namespace PANDA.App
{
    public class Launcher
    {
        static void Main(string[] args)
        {
            WebHost.Start(new StartUp());
        }
    }
}
