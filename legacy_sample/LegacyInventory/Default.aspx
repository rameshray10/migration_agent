<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LegacyInventory.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Welcome to the Inventory System</h2>
    <p class="lead">Manage your product categories and inventory using the links below.</p>

    <div class="row" style="margin-top: 30px;">
        <div class="col-md-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Categories</h3>
                </div>
                <div class="panel-body">
                    <p>Create and manage product categories.</p>
                    <a href="/Categories/List.aspx" class="btn btn-primary">View Categories</a>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="panel panel-success">
                <div class="panel-heading">
                    <h3 class="panel-title">Products</h3>
                </div>
                <div class="panel-body">
                    <p>Manage inventory products and stock levels.</p>
                    <a href="/Products/List.aspx" class="btn btn-success">View Products</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
