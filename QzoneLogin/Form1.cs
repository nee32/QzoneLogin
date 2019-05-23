using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QzoneLogin
{
    public partial class Form1 : Form
    {
        private static List<QQLoginModel> data = new List<QQLoginModel>();
        private static string url = "https://xui.ptlogin2.qq.com/cgi-bin/xlogin?proxy_url=https://qzs.qq.com/qzone/v6/portal/proxy.html&daid=5&&hide_title_bar=1&low_login=0&qlogin_auto_login=1&no_verifyimg=1&link_target=blank&appid=549000912&style=22&target=self&s_url=https:%2F%2Fqzs.qzone.qq.com%2Fqzone%2Fv5%2Floginsucc.html%3Fpara%3Dizone&pt_qr_app=%E6%89%8B%E6%9C%BAQQ%E7%A9%BA%E9%97%B4&pt_qr_link=http://z.qzone.com/download.html&self_regurl=https://qzs.qq.com/qzone/v6/reg/index.html&pt_qr_help_link=http://z.qzone.com/download.html&pt_no_auth=1";
        private static int index = 0;
        private static List<string> successList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            var text = File.ReadAllLines(@"E:\qq.txt");
            foreach (var item in text)
            {
                var array = item.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                data.Add(new QQLoginModel() { qq = array[0], pwd = array[1] });
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(url);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser1_DocumentCompleted);
            Login();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.ReadyState != WebBrowserReadyState.Complete) return;

            //再次判断是否加载完成
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete && webBrowser1.IsBusy == false)
            {
                //while (doc == null)  //网络操作总是伴随着一些不可预知的异常，所以在这以防万一对象为空,我们继续判断
                //{
                //    Application.DoEvents();//如果为空，就转交控制权
                //}

                if (e.Url.AbsoluteUri == "https://user.qzone.qq.com/" + data[index].qq)
                {
                    if (!successList.Contains(data[index].qq))
                    {
                        successList.Add(data[index].qq);
                        richTextBox1.AppendText($"==={data[index].qq}登录完成===" + "\n");
                        index++;
                    }
                    webBrowser1.Navigate(url);
                }

                else if (e.Url.AbsoluteUri == "https://qzs.qq.com/ac/qzfl/release/resource/html/storage_helper.html")
                {
                    Thread.Sleep(500);
                    webBrowser1.Navigate(url);
                    return;
                }
            }
            
        }

        private void Login()
        {
            HtmlDocument doc = webBrowser1.Document; //抓取网页
            doc.GetElementById("login_button")?.InvokeMember("click");
            doc.GetElementById("switcher_plogin")?.InvokeMember("click");//切换成账号密码登录
            Thread.Sleep(500);
            var user = doc.GetElementById("u");
            var pwd = doc.GetElementById("p");

            ////设置元素value属性值 (用户名 密码值)
            user.SetAttribute("value", data[index].qq);
            pwd.SetAttribute("value", data[index].pwd);
            //Thread.Sleep(1000);
            ////执行元素的方法：如click submit
            doc.GetElementById("login_button")?.InvokeMember("click");
        }
    }


    public class QQLoginModel
    {
        public string qq { get; set; }

        public string pwd { get; set; }
    }
}
