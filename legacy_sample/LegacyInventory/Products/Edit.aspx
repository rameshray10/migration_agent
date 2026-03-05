<%@ Page Title="Edit Product" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="LegacyInventory.Products.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Product</h2>
    <asp:HiddenField ID="hdnId" runat="server" />

    <div class="row">
        <div class="col-md-6">

            <div class="form-group">
                <label>Name <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="200" />
                <asp:RequiredFieldValidator ID="rfvName" runat="server"
                    ControlToValidate="txtName" Display="Dynamic"
                    CssClass="text-danger" ErrorMessage="Name is required." />
            </div>

            <div class="form-group">
                <label>Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"
                    TextMode="MultiLine" Rows="3" MaxLength="1000" />
            </div>

            <div class="form-group">
                <label>Price <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvPrice" runat="server"
                    ControlToValidate="txtPrice" Display="Dynamic"
                    CssClass="text-danger" ErrorMessage="Price is required." />
                <asp:RangeValidator ID="rvPrice" runat="server"
                    ControlToValidate="txtPrice" Display="Dynamic"
                    MinimumValue="0" MaximumValue="999999" Type="Double"
                    CssClass="text-danger" ErrorMessage="Price must be between 0 and 999999." />
            </div>

            <div class="form-group">
                <label>Stock <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="rfvStock" runat="server"
                    ControlToValidate="txtStock" Display="Dynamic"
                    CssClass="text-danger" ErrorMessage="Stock is required." />
                <asp:RangeValidator ID="rvStock" runat="server"
                    ControlToValidate="txtStock" Display="Dynamic"
                    MinimumValue="0" MaximumValue="2147483647" Type="Integer"
                    CssClass="text-danger" ErrorMessage="Stock must be 0 or greater." />
            </div>

            <div class="form-group">
                <label>Category <span class="text-danger">*</span></label>
                <asp:DropDownList ID="ddlCategory" runat="server"
                    CssClass="form-control"
                    DataTextField="Name" DataValueField="Id" />
            </div>

            <asp:Button ID="btnSave" runat="server" Text="Save Changes"
                CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <a href="/Products/List.aspx" class="btn btn-default">Cancel</a>
        </div>
    </div>

</asp:Content>
