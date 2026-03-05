<%@ Page Title="Delete Product" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="LegacyInventory.Products.Delete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Product</h2>
    <asp:HiddenField ID="hdnId" runat="server" />

    <div class="alert alert-danger">
        <strong>Warning:</strong> Are you sure you want to delete this product?
        This cannot be undone.
    </div>

    <table class="table table-bordered" style="max-width: 500px;">
        <tr>
            <th style="width: 130px;">ID</th>
            <td><asp:Literal ID="litId" runat="server" /></td>
        </tr>
        <tr>
            <th>Name</th>
            <td><asp:Literal ID="litName" runat="server" /></td>
        </tr>
        <tr>
            <th>Category</th>
            <td><asp:Literal ID="litCategory" runat="server" /></td>
        </tr>
        <tr>
            <th>Price</th>
            <td>$<asp:Literal ID="litPrice" runat="server" /></td>
        </tr>
        <tr>
            <th>Stock</th>
            <td><asp:Literal ID="litStock" runat="server" /></td>
        </tr>
    </table>

    <asp:Button ID="btnDelete" runat="server" Text="Yes, Delete"
        CssClass="btn btn-danger" OnClick="btnDelete_Click" />
    <a href="/Products/List.aspx" class="btn btn-default">Cancel</a>

</asp:Content>
