using System;

namespace LegacyInventory
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Pure ADO.NET — no ORM initializer needed.
            // Run DatabaseSetup.sql in SQL Server Management Studio before first use.
        }
    }
}
