using System;
using System.Data;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Products
{
    public partial class Edit : System.Web.UI.Page
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

                LoadCategories();
                LoadProduct(id);
            }
        }

        private void LoadCategories()
        {
            var dt = new DataTable();

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT Id, Name FROM Categories ORDER BY Name", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                conn.Open();
                adapter.Fill(dt);
            }

            ddlCategory.DataSource     = dt;
            ddlCategory.DataTextField  = "Name";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();
        }

        private void LoadProduct(int id)
        {
            const string sql =
                "SELECT Id, Name, Description, Price, Stock, CategoryId FROM Products WHERE Id = @Id";

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

                    hdnId.Value         = reader["Id"].ToString();
                    txtName.Text        = reader["Name"].ToString();
                    txtDescription.Text = reader["Description"].ToString();
                    txtPrice.Text       = Convert.ToDecimal(reader["Price"]).ToString("F2");
                    txtStock.Text       = reader["Stock"].ToString();

                    // Pre-select the product's current category
                    ddlCategory.SelectedValue = reader["CategoryId"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            const string sql = @"
                UPDATE Products
                SET    Name        = @Name,
                       Description = @Description,
                       Price       = @Price,
                       Stock       = @Stock,
                       CategoryId  = @CategoryId
                WHERE  Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name",        txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Price",       decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@Stock",       int.Parse(txtStock.Text));
                cmd.Parameters.AddWithValue("@CategoryId",  int.Parse(ddlCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@Id",          int.Parse(hdnId.Value));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Products/List.aspx");
        }
    }
}
