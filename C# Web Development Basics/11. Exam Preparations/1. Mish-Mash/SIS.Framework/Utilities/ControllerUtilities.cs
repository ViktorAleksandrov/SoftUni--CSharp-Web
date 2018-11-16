namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
        public static string GetControllerName(object controller)
        {
            return controller.GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty);
        }
    }
}
