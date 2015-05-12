using PhotoshopTypeLibrary;

namespace PhotoshopAuto
{
    namespace Extensions
    {
        public static class Extensions
        {
            
        }
    }

    public class ProxyLog
    {
        System.IO.StreamWriter os;

        public ProxyLog()
        {
            os = System.IO.File.CreateText("d:\\dump.txt");
        }

        ~ProxyLog()
        {
        }

        public void Write(string s)
        {
            os.Write(s);
            os.Flush();
        }

        public void Write(string s, params object[] items)
        {
            string ss = string.Format(s, items);
            os.Write(ss);
            os.Flush();
        }
        public void WriteLine(string s)
        {
            os.WriteLine(s);
            os.Flush();
        }

        public void WriteLine(string s, params object[] items)
        {
            string ss = string.Format(s, items);
            os.WriteLine(ss);
            os.Flush();
        }

    }
    public class PhotoshoProxyError : System.Exception
    {
        public PhotoshoProxyError(string msg) :
            base("PhotoshopProxyError: " + msg)
        {

        }


    }
}


