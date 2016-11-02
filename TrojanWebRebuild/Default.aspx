<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrojanWebRebuild._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1><%: Title %></h1>
        <p class="lead">Welcome to the Uvic ECE hardware security project.</p>
        <p><a href="About.aspx" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Trojan Categorization</h2>
            <p>
                The hardware trojan categorization tool is an effective method for virus detection, prevention and strategic planning.
            </p>
            <p>
                <a class="btn btn-default" href="Categorization.aspx">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Virus Detection</h2>
            <p>
                A new method for detection of hardware viruses.
            </p>
            <p>
                <a class="btn btn-default" href="Detection.aspx">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Hardware Trojan Attacks</h2>
            <p>
                Analysis of strength of hardware attacks.
            </p>
            <p>
                <a class="btn btn-default" href="Attacks.aspx">Learn more &raquo;</a>
            </p>
        </div>
    </div>
</asp:Content>
