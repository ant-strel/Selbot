using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace Selbot
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)//Открыть браузер
        {
            Browser = new OpenQA.Selenium.Firefox.FirefoxDriver();
            Browser.Manage().Window.Maximize();
        }

        private void button2_Click(object sender, EventArgs e)//Закрыть браузер
        {
            Browser.Quit();
        }

        private void button1_Click(object sender, EventArgs e)//Открыть ВК + авторизация 
        {
            Browser = new OpenQA.Selenium.Firefox.FirefoxDriver();
            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://www.vk.com/");
            Thread.Sleep(GetRandom(200, 1000));
            IWebElement phoneinput = Browser.FindElement(By.Id("index_email"));
            phoneinput.SendKeys("89123456789");                                             // сюда логин
            Thread.Sleep(GetRandom(200, 1000));
            IWebElement passinput = Browser.FindElement(By.Id("index_pass"));
            passinput.SendKeys("pass");                                                     //сюда пароль
            Thread.Sleep(GetRandom(200, 1000));
            IWebElement buttoninput = Browser.FindElement(By.Id("index_login_button"));
            buttoninput.Click();

        }

        private void button3_Click(object sender, EventArgs e)//Открыть диалог с ботом
        {
            Browser.Navigate().GoToUrl("https://vk.com/im?sel=-000000000000");     //ссылка на диалог с ботом
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] allfiles = Directory.GetFiles(@"c:\images\");           //папка с изображениями(имя файла-артикул)
            foreach (string filename in allfiles)
            {

                IWebElement textsend = Browser.FindElement(By.ClassName("im_editable"));
                textsend.SendKeys("/getphoto-"+ Path.GetFileNameWithoutExtension(filename));
                Thread.Sleep(GetRandom(200, 1000));
                IWebElement attphoto = Browser.FindElement(By.Id("im_full_upload"));
                attphoto.SendKeys(filename);
                Thread.Sleep(GetRandom(4000, 7000));
                IWebElement sendbtn = Browser.FindElement(By.CssSelector("#content div.im-chat-input--txt-wrap._im_text_wrap > button"));
                sendbtn.Click();
            }

        }

        private int GetRandom(int from,int till)
        {
            Random rnd = new Random();

            return rnd.Next(from, till);
        }
    }
}
