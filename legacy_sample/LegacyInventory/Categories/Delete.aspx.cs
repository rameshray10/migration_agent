using System;
using System.Data.SqlClient;
using LegacyInventory.Data;

namespace LegacyInventory.Categories
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
                    litId.Text          = reader["Id"].ToString();
                    litName.Text        = reader["Name"].ToString();
                    litDescription.Text = reader["Description"].ToString();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            const string sql = "DELETE FROM Categories WHERE Id = @Id";

            using (var conn = Database.GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", int.Parse(hdnId.Value));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("/Categories/List.aspx");
        }
    }
}
