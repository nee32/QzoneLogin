using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QzoneLogin
{
    public partial class Form1 : Form
    {
        private List<KVClass> dic = new List<KVClass>();

        string url = "https://xui.ptlogin2.qq.com/cgi-bin/xlogin?proxy_url=https%3A//qzs.qq.com/qzone/v6/portal/proxy.html&daid=5&&hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=22&target=self&s_url=https%3A%2F%2Fqzs.qzone.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&pt_qr_app=%E6%89%8B%E6%9C%BAQQ%E7%A9%BA%E9%97%B4&pt_qr_link=http%3A//z.qzone.com/download.html&self_regurl=https%3A//qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http%3A//z.qzone.com/download.html&pt_no_auth=1";
        private static int index = 0;

        public Form1()
        {
            InitializeComponent();
            var text = File.ReadAllLines(@"E:\qq.txt");
            foreach (var item in text)
            {
                var array = item.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                dic.Add(new KVClass() {
                     key= array[0],
                     value= array[1]
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(url);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
        }

        private void QQLogin(string QQ, string Pwd)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string QQPwd = Convert.ToBase64String(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pwd)));
            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = @"D:\Program Files (x86)\Tencent\QQ\Bin\QQ.exe";
            pro.StartInfo.Arguments = "/start QQUIN:" + QQ + " PWDHASH:" + QQPwd + " /stat:40"; pro.Start(); }

        private void button1_Click(object sender, EventArgs e)
        {
            Login(dic[index].key, dic[index].value);
            index++;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete && webBrowser1.IsBusy == false)
            {

            }


            //为了保险起见 我们在这再次判断是否加载完成
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete && webBrowser1.IsBusy == false)
            {
                HtmlDocument doc = webBrowser1.Document; //抓取网页
                while (doc == null)  //网络操作总是伴随着一些不可预知的异常，所以在这以防万一对象为空,我们继续判断
                {
                    Application.DoEvents();//如果为空，就转交控制权
                }


                for (int i = 0; i < dic.Count; i++)
                {
                    
                }
            }
            else
            {

            }

            if (webBrowser1.ReadyState < WebBrowserReadyState.Complete) return;

            if (e.Url.AbsoluteUri == "https://qzs.qq.com/ac/qzfl/release/resource/html/storage_helper.html")
            {
                Thread.Sleep(2000);
                webBrowser1.Navigate(url);
                return;
            }
            else
            {

            }
        }


        private void Login(string QQ, string Pwd)
        {
            HtmlDocument doc = this.webBrowser1.Document;
            doc.GetElementById("login_button")?.InvokeMember("click");
            doc.GetElementById("switcher_plogin")?.InvokeMember("click");//切换成账号密码登录
            Thread.Sleep(500);
            var user = doc.GetElementById("u");
            var pwd = doc.GetElementById("p");

            ////设置元素value属性值 (用户名 密码值)
            user.SetAttribute("value", QQ);
            pwd.SetAttribute("value", Pwd);
            //Thread.Sleep(1000);
            ////执行元素的方法：如click submit
            doc.GetElementById("login_button")?.InvokeMember("click");
        }

    }

    public class KVClass
    {
        public string key { get; set; }

        public string value { get; set; }
    }
}
