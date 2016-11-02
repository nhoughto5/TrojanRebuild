<%@ Page Title="" Language="C#" MasterPageFile="~/Application/Categorization/Categorization.Master" AutoEventWireup="true" CodeBehind="AttributeDetails.aspx.cs" Inherits="TrojanWebRebuild.Application.Categorization.AttributeDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="AttributeDetail" runat="server" ItemType="Trojan.Models.Attribute" SelectMethod ="GetAttribute" RenderOuterTable="false">
        <ItemTemplate>
            <div>
                <h1><%#:Item.AttributeName %></h1>
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <img src="/Catalog/Images/<%#:Item.ImagePath %>" style="border:solid; height:150px; margin-right: 30px" alt="<%#:Item.AttributeName %>"/>
                    </td>
                    <td>&nbsp;</td>  
                    <td style="vertical-align: top; text-align:left;">
                        <b>Description:</b><br /><%#:Item.Description %>
                        <br />
                        <span><b>F_in:</b>&nbsp;<%#: String.Format("{0:d}", Item.F_in) %></span>
                        <span><b>F_out:</b>&nbsp;<%#: String.Format("{0:d}", Item.F_out) %></span>
                        <br />
                        <span><b>Attribute Number:</b>&nbsp;<%#:Item.AttributeId %></span>
                        <br />
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
