<%@ Page Title="Edit Category" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="LegacyInventory.Categories.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Category</h2>
    <asp:HiddenField ID="hdnId" runat="server" />

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label for="txtName">Name <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="100" />
                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                    ControlToValidate="txtName" Display="Dynamic"
                    CssClass="text-danger" ErrorMessage="Name is required." />
            </div>

            <div class="form-group">
                <label for="txtDescription">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="4" MaxLength="500" />
            </div>

            <asp:Button ID="btnSave" runat="server" Text="Save Changes"
                CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <a href="/Categories/List.aspx" class="btn btn-default">Cancel</a>
        </div>
    </div>

</asp:Content>
