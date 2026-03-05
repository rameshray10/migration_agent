using System;
using System.Data;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Categories
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadCategories();
        }

        private void LoadCategories()
        {
            var dt = new DataTable();

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT Id, Name, Description FROM Categories ORDER BY Name", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }

            rptCategories.DataSource = dt;
            rptCategories.DataBind();
        }
    }
}
