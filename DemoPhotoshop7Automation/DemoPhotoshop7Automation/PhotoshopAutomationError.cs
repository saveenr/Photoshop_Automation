namespace PhotoshopAutomation
{
    public class PhotoshopAutomationError : System.Exception
    {
        public PhotoshopAutomationError(string msg) :
            base("PhotoshopProxyError: " + msg)
        {

        }
    }
}