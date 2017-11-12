using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace HttpServer
{
    public class HttpServer
    {
        HttpListener listener;

        public HttpServer(string uri)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    Receiver(context);
                }
                catch (Exception e)
                {

                }
            }

        }

        private void Receiver(HttpListenerContext context)
        {
            string a = "";
            string b = "";
            string op = "";

            if (context.Request.HttpMethod == "POST")
            {
                string[] param = new StreamReader(context.Request.InputStream).ReadToEnd().Split('=', '&');
                a = param[1];
                b = param[3];
                op = param[5];
            }
            else
            {
                a = context.Request.QueryString["a"];
                b = context.Request.QueryString["b"];
                op = context.Request.QueryString["op"];
            }

            string data = Calc(a, b, op).ToString();

            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentLength64 = data.Length;
            context.Response.OutputStream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            context.Response.OutputStream.Close();
        }

        public int Calc(string a, string b, string op)
        {
            int res = 0;
            int n1 = Convert.ToInt32(a);
            int n2 = Convert.ToInt32(b);
            switch (op)
            {
                case "-": res = n1 - n2; break;
                case "+": res = n1 + n2; break;
                case "*": res = n1 * n2; break;
                case "/": res = n1 / n2; break;
            }
            return res;
        }
    }
}