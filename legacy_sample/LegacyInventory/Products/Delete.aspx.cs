using System;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Products
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;
                if (!int.TryParse(Request.QueryString["id"], out id))
                {
                    Response.Redirect("/Products/List.aspx");
                    return;
                }

                LoadProduct(id);
            }
        }

        private void LoadProduct(int id)
        {
            const string sql = @"
                SELECT p.Id, p.Name, p.Price, p.Stock, c.Name AS CategoryName
                FROM   Products   p
                INNER JOIN Categories c ON p.CategoryId = c.Id
                WHERE  p.Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Response.Redirect("/Products/List.aspx");
                        return;
                    }

                    hdnId.Value      = reader["Id"].ToString();
                    litId.Text       = reader["Id"].ToString();
                    litName.Text     = reader["Name"].ToString();
                    litCategory.Text = reader["CategoryName"].ToString();
                    litPrice.Text    = Convert.ToDecimal(reader["Price"]).ToString("F2");
                    litStock.Text    = reader["Stock"].ToString();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            const string sql = "DELETE FROM Products WHERE Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", int.Parse(hdnId.Value));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Products/List.aspx");
        }
    }
}
