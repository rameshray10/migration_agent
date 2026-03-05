using System;
using System.Data;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Products
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadProducts();
        }

        private void LoadProducts()
        {
            const string sql = @"
                SELECT p.Id,
                       p.Name,
                       c.Name  AS CategoryName,
                       p.Price,
                       p.Stock,
                       p.CreatedAt
                FROM   Products   p
                INNER JOIN Categories c ON p.CategoryId = c.Id
                ORDER BY p.Name";

            var dt = new DataTable();

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }

            rptProducts.DataSource = dt;
            rptProducts.DataBind();
        }
    }
}
