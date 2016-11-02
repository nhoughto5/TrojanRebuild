<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategorizationMain.aspx.cs" Inherits="TrojanWebRebuild.Application.Categorization.CategorizationMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div><h1>Categorization</h1></div>

    <div class="well" style="text-align:center">
        <%--<h4 style="display:inline"><a href="VirusDescription.aspx">Create a new Trojan Visualization</a></h4>--%>
        <asp:Button ID="newVirusBtn" class="btn btn-primary" runat="server" Text="Create a new Trojan Visualization" OnClick="newVirusBtn_Click" />
    </div>
    <div class="well" style="text-align:center">
        <h4 style="display:inline">Review a Previous Trojan</h4>
        <asp:DropDownList ID="trojanDrpDown" runat="server" style="display:inline" OnSelectedIndexChanged="trojanDrpDown_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    </div>
</asp:Content>
