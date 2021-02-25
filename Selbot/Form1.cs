using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
            phoneinput.SendKeys("tel");                                             // сюда логин
            Thread.Sleep(GetRandom(200, 1000));
            IWebElement passinput = Browser.FindElement(By.Id("index_pass"));
            passinput.SendKeys("pass");                                                     //сюда пароль
            Thread.Sleep(GetRandom(200, 1000));
            IWebElement buttoninput = Browser.FindElement(By.Id("index_login_button"));
            buttoninput.Click();
            Thread.Sleep(2000);
            Browser.Navigate().GoToUrl("https://vk.com/market-85555931");

            IWebElement morecat = Browser.FindElement(By.Id("ui_albums_load_more"));
            morecat.Click();

        }

        private void button3_Click(object sender, EventArgs e)//Открыть диалог с ботом
        {
            Browser.Navigate().GoToUrl("https://vk.com/market-85555931");     //ссылка на диалог с ботом
            

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

        private void button6_Click(object sender, EventArgs e)//парсить категории
        {

            IList<IWebElement> catlist = Browser.FindElements(By.ClassName("market_album_name_link"));
            
            SqlConnection Con = new SqlConnection("");


            #region categoryadd
            /*   for (int i = 0; i<catlist.Count;i++)
               { 

                   SqlCommand addProduct = new SqlCommand("INSERT INTO CategoriesDostavka (categoryId, name, btnName) VALUES (@categoryId, @name, @btnName);", Con);
                   addProduct.Parameters.AddWithValue("@categoryId", i+1);
                   addProduct.Parameters.AddWithValue("@name", catlist[i].Text);
                   addProduct.Parameters.AddWithValue("@btnName", catlist[i].Text);

                   addProduct.ExecuteNonQuery();
               }*/
            #endregion

            #region updatecategory
            
            for(int j =0; j<catlist.Count;j++)
            {

                
                Con.Open();
                string catName = catlist[j].Text;
                SqlCommand getCatId = new SqlCommand("SELECT categoryId FROM CategoriesDostavka WHERE name = @name;", Con);
                getCatId.Parameters.AddWithValue("@name", catName);
                SqlDataReader rgetCatId = getCatId.ExecuteReader();
                rgetCatId.Read();
                int catId = Convert.ToInt32(rgetCatId["categoryId"]);
                rgetCatId.Close();

                catlist[j].Click();
                Thread.Sleep(3000);
                IJavaScriptExecutor js = (IJavaScriptExecutor)Browser;

                for (int k = 0; k < 10; k++)
                {
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                    // Browser.
                    Thread.Sleep(1000);
                }
                IList<IWebElement> prodlist = Browser.FindElements(By.ClassName("market_row"));
                foreach(var m in prodlist)
                {
                    string[] article = m.FindElement(By.TagName("a")).GetAttribute("href").Split('_');
                    string id = article[1];
                   // IWebElement link = m.FindElement(By.CssSelector("#market_item5399830 > div > div.market_row_name > a"));
                    SqlCommand updProd = new SqlCommand("UPDATE ProductsDostavka SET categoryId = @categoryId WHERE article = @article;", Con);
                    updProd.Parameters.AddWithValue("@categoryId", catId);
                    updProd.Parameters.AddWithValue("@article", id);
                    updProd.ExecuteNonQuery();
                }
                Thread.Sleep(1000);

                Browser.Navigate().GoToUrl("https://vk.com/market-85555931");
                Thread.Sleep(3000);
                IWebElement morecat = Browser.FindElement(By.Id("ui_albums_load_more"));
                morecat.Click();
                Thread.Sleep(3000);
                catlist = Browser.FindElements(By.ClassName("market_album_name_link"));
                Con.Close();
            }

            MessageBox.Show("Готово!");
            
            #endregion



        }
    }
}
