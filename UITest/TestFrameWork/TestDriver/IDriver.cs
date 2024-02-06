using System.Collections.ObjectModel;

using OpenQA.Selenium;

namespace UITest.TestDriver
{
    public interface IDriver
    {

        IWebElement FindElement(Enums.FINDBY By, string Locator);
        ReadOnlyCollection<IWebElement> FindElements(Enums.FINDBY By, string Locator);
        bool Navigate(string URL);
        bool Close();
        void WaitFor(Enums.FINDBY By, string Locator, int Seconds);

        void SwitchTo(int WindowHandle);

        void CloseTab(int TabNumber);

        string GetPageSourceForWindow();

        string GetCurrentURL();
    }
}
