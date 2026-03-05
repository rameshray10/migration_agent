<%@ Page Title="Delete Category" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="LegacyInventory.Categories.Delete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete Category</h2>
    <asp:HiddenField ID="hdnId" runat="server" />

    <div class="alert alert-danger">
        <strong>Warning:</strong> Are you sure you want to delete this category?
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
            <th>Description</th>
            <td><asp:Literal ID="litDescription" runat="server" /></td>
        </tr>
    </table>

    <asp:Button ID="btnDelete" runat="server" Text="Yes, Delete"
        CssClass="btn btn-danger" OnClick="btnDelete_Click" />
    <a href="/Categories/List.aspx" class="btn btn-default">Cancel</a>

</asp:Content>
