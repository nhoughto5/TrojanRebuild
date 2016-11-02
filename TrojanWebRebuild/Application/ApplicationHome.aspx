<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationHome.aspx.cs" Inherits="TrojanWebRebuild.Application.ApplicationHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row top-margin-20" style="border: double black; padding: 2px" id="Categorization" runat="server">
        <div class="col-md-4" style="border: groove grey 2px">
            <h4>Categorization Visualization</h4>
            <img src="../Images/visualize2Screen.png" style="width: 300px" runat="server" />
        </div>
        <div class="col-md-8" runat="server">
            <p>
                To aid in the investigation of hardware trojan viruses a new technique for categorization and analysis has been developed; this technique however requires considerable computational efforts. This categorizaton tool automates the employment of this new technique by providing a clean, easy 
            to use user-interface and an intuitive visual representation.
            </p>
            <div runat="server">
                <a class="btn btn-primary" runat="server" href="~/Application/Categorization/CategorizationMain.aspx">Categorization Tool &raquo;</a>
            </div>
        </div>
    </div>

    <div class="row top-margin-20" style="border: double black; padding: 2px" id="Div1" runat="server">
        <div class="col-md-4" style="border: groove grey 2px">
            <h4>Detection</h4>
            <img src="~/Images/Detective.jpg" style="width: 200px" runat="server" />
        </div>
        <div class="col-md-8" runat="server">
            <p>
                When developers produce a new method of detecting hardware trojans they require some means of
                determining how it compares to currently known viruses and other detection methods. The ranking
                system described in <a href="../Detection.aspx" runat="server">here</a> provides a means for these methods to be compared and contrasted. In the
                HTS Detection application developers are able to investigate their methods systematically and
                derive a quantitative status value.
            </p>
            <div runat="server">
                <%--<asp:LinkButton id="detectionBtn" class="btn btn-primary" runat="server" OnClick="detectionBtn_Click">Detection Tool &raquo;</asp:LinkButton>--%>
                <a class="btn btn-primary" runat="server" href="~/Application/Detection/DetectionApplication.aspx">Detection Tool &raquo;</a>
            </div>
        </div>
    </div>

    <div class="row top-margin-20" style="border: double black; padding: 2px" id="Div2" runat="server">
        <div class="col-md-4" style="border: groove grey 2px">
            <h4>Hardware Attacks</h4>
            <img src="~/Images/ComingSoon.png" style="width: 300px" runat="server" />
        </div>
        <div class="col-md-8" runat="server">
            <p>Coming Soon</p>
            <div runat="server">
                <a class="btn btn-primary"  runat="server" href="~/Application/Attacks/Attacks.aspx">Attack Tool &raquo;</a>
                <%--<asp:LinkButton ID="attackBtn" class="btn btn-primary" runat="server" OnClick="attackBtn_Click">Attack Tool &raquo;</asp:LinkButton>--%>
            </div>
        </div>
    </div>
</asp:Content>
