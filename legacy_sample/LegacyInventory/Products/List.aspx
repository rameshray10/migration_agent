<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="LegacyInventory.Products.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Products</h2>
    <a href="/Products/Create.aspx" class="btn btn-success" style="margin-bottom: 15px;">
        + Add Product
    </a>

    <table class="table table-bordered table-striped table-hover">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Category</th>
                <th>Price</th>
                <th>Stock</th>
                <th>Created</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptProducts" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Id") %></td>
                        <td><%# Eval("Name") %></td>
                        <td><%# Eval("CategoryName") %></td>
                        <td>$<%# Eval("Price", "{0:F2}") %></td>
                        <td><%# Eval("Stock") %></td>
                        <td><%# Eval("CreatedAt", "{0:dd/MM/yyyy}") %></td>
                        <td>
                            <a href='/Products/Edit.aspx?id=<%# Eval("Id") %>'
                               class="btn btn-xs btn-warning">Edit</a>
                            <a href='/Products/Delete.aspx?id=<%# Eval("Id") %>'
                               class="btn btn-xs btn-danger">Delete</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

</asp:Content>
