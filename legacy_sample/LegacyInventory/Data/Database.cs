using System.Configuration;
using System.Data.SqlClient;

namespace LegacyInventory.Data
{
    /// <summary>
    /// Pure ADO.NET connection factory.
    /// Reads the connection string from Web.config (name="InventoryDB").
    ///
    /// Usage pattern in every code-behind:
    ///   using (var conn = Database.GetConnection())
    ///   using (var cmd  = new SqlCommand("SELECT ...", conn))
    ///   {
    ///       conn.Open();
    ///       // execute cmd ...
    ///   }
    /// </summary>
    public static class Database
    {
        public static SqlConnection GetConnection()
        {
            string connStr = ConfigurationManager
                .ConnectionStrings["InventoryDB"]
                .ConnectionString;

            return new SqlConnection(connStr);
        }
    }
}
