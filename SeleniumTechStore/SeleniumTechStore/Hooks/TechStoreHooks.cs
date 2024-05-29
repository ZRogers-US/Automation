using SeleniumTechStore.Drivers;
using SeleniumTechStore.PageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTechStore.Hooks
{
    [Binding]
    public class TechStoreHooks
    {
        private TechStorePageObjectModel _techStorePageObjectModel;

        public TechStoreHooks(BrowserDriver webDriver)
        {
            _techStorePageObjectModel = new TechStorePageObjectModel(webDriver.Current);
        }

        [BeforeScenario]//("Signup to TechStore")
        public void NewUserSignup()
        {
            //_techStorePageObjectModel.SelectSignUpNavButton(); // not selecting but whilst in hook
            Console.WriteLine("hello thsi is a hook test");
        }
    }
}
