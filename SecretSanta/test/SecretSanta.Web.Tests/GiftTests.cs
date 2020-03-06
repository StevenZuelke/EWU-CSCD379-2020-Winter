using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.Api;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class GiftTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }

        [NotNull]
        private IWebDriver? Driver { get; set; }

        string AppURL { get; } = "https://localhost:44394/";
        //string AppURL { get; } = "https://localhost:5001/";

        [TestInitialize]
        public void TestInitialize()
        {

            string browser = "Chrome";
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }
            Driver.Manage().Timeouts().ImplicitWait = new System.TimeSpan(0, 0, 10);

        }

        private static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name,
                        Microsoft.Extensions.Logging.LogLevel.Information);
            });
            return serviceCollection.BuildServiceProvider().
                GetService<ILoggerFactory>();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task CreateNewGiftAsync()
        {
            //Arrange
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44388/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            UserClient userClient = new UserClient(httpClient);
            Api.UserInput userInput = new Api.UserInput();
            userInput.FirstName = "Steve";
            userInput.LastName = "Zuelke";
            await userClient.PostAsync(userInput);
            httpClient.Dispose();
            Driver.Navigate().GoToUrl(new Uri(AppURL+"Gifts/"));

            //Act
            var rowsBefore = Driver.FindElements(By.CssSelector("tr"));
            Driver.FindElement(By.CssSelector("button[id='createNewGift']")).Click();
            string title = "Gift title";
            string desc = "Gift Desc";
            string url = "Gift url";
            Driver.FindElement(By.CssSelector("input[id='giftTitleInput']")).SendKeys(text: title);
            Driver.FindElement(By.CssSelector("input[id='giftDescriptionInput']")).SendKeys(text: desc);
            Driver.FindElement(By.CssSelector("input[id='giftUrlInput']")).SendKeys(text: url);
            Driver.FindElement(By.CssSelector("select[id='userIdSelector']")).Click();
            Driver.FindElement(By.CssSelector("option")).Click();
            Driver.FindElement(By.CssSelector("button[id='submit']")).Click();
            Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            screenshot.SaveAsFile("../../../giftScreenshot.png", ScreenshotImageFormat.Png);
            Thread.Sleep(4000);
            var rowsAfter = Driver.FindElements(By.CssSelector("tr"));

            //Assert
            Assert.AreEqual<int>(rowsAfter.Count - 1, rowsBefore.Count);
        }

        //public void EnterBingSearchText(string text)
        //{
        //    Driver.Navigate().GoToUrl(new Uri(AppURL + "/"));
        //    IWebElement element = Driver.FindElement(By.Id("sb_form_q"));
        //    element.SendKeys(text);
        //    Assert.AreEqual<string>(text, element.GetProperty("value"));
        //}
        //
        //[TestMethod]
        //public void BingSearch_UsingCSSSelector_Success()
        //{
        //    string searchString = "Inigo Montoya";
        //    EnterBingSearchText(searchString);
        //    Driver.FindElement(By.CssSelector("label[for='sb_form_go']")).Click();
        //    Assert.IsTrue(Driver.Title.Contains(searchString), "Verified title of the page");
        //}

        [TestCleanup()]
        public void TestCleanup()
        {
            Driver.Quit();
        }
    }
}
