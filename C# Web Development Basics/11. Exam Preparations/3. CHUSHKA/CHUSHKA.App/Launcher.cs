using SIS.Framework;

namespace CHUSHKA.App
{
    public class Launcher
    {
        public static void Main()
        {
            WebHost.Start(new StartUp());
        }
    }
}
