using System;
using System.Data;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Products
{
    public partial class Create : System.Web.UI.Page
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            const string sql = @"
                INSERT INTO Products (Name, Description, Price, Stock, CategoryId, CreatedAt)
                VALUES (@Name, @Description, @Price, @Stock, @CategoryId, @CreatedAt)";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name",        txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Price",       decimal.Parse(txtPrice.Text));
                cmd.Parameters.AddWithValue("@Stock",       int.Parse(txtStock.Text));
                cmd.Parameters.AddWithValue("@CategoryId",  int.Parse(ddlCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@CreatedAt",   DateTime.Now);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Products/List.aspx");
        }
    }
}
