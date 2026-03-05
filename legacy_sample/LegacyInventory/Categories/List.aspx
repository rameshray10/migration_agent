<%@ Page Title="Categories" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="LegacyInventory.Categories.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Categories</h2>
    <a href="/Categories/Create.aspx" class="btn btn-success" style="margin-bottom: 15px;">
        + Add Category
    </a>

    <table class="table table-bordered table-striped table-hover">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Description</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptCategories" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Id") %></td>
                        <td><%# Eval("Name") %></td>
                        <td><%# Eval("Description") %></td>
                        <td>
                            <a href='/Categories/Edit.aspx?id=<%# Eval("Id") %>'
                               class="btn btn-xs btn-warning">Edit</a>
                            <a href='/Categories/Delete.aspx?id=<%# Eval("Id") %>'
                               class="btn btn-xs btn-danger">Delete</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

</asp:Content>
