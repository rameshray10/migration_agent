using System;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Categories
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
                    Response.Redirect("/Categories/List.aspx");
                    return;
                }

                LoadCategory(id);
            }
        }

        private void LoadCategory(int id)
        {
            const string sql =
                "SELECT Id, Name, Description FROM Categories WHERE Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Response.Redirect("/Categories/List.aspx");
                        return;
                    }

                    hdnId.Value         = reader["Id"].ToString();
                    txtName.Text        = reader["Name"].ToString();
                    txtDescription.Text = reader["Description"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            const string sql =
                "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Name",        txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Id",          int.Parse(hdnId.Value));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Categories/List.aspx");
        }
    }
}
