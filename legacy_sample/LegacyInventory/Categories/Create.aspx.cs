using System;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Categories
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            const string sql =
                "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name",        txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Categories/List.aspx");
        }
    }
}
