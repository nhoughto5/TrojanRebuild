<%@ Page Title="" Language="C#" MasterPageFile="~/Application/Categorization/Categorization.Master" AutoEventWireup="true" CodeBehind="AttributeList.aspx.cs" Inherits="TrojanWebRebuild.Application.Categorization.AttributeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>
            
            <div class="jumbotron" style="text-align:center">
                <asp:ListView ID="attributeList" runat="server" 
                DataKeyNames="AttributeID" style="align-items: center" GroupItemCount="4"
                ItemType="TrojanWebRebuild.Models.Attribute" SelectMethod="GetAttributes" >
                <EmptyDataTemplate>
                    <table >
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <EmptyItemTemplate>
                    <td/>
                </EmptyItemTemplate>
                <GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>
                <ItemTemplate>
                    <td runat="server">
                        <table style="margin: 0 auto">
                            <tr>
                                <td>
                                    <a href="AttributeDetails.aspx?AttributeId=<%#:Item.AttributeId%>">
                                        <image src='/Catalog/Images/Thumbs/<%#:Item.ImagePath%>' width="100" height="75" border="1" align="middle"/></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div runat="server" style="text-align:center">
                                        <a  href="AttributeDetails.aspx?AttributeId=<%#:Item.AttributeId%>">
                                            <%#:Item.AttributeName%>
                                        </a>
                                        <asp:CheckBox id="selectedChkBx" runat="server"/>
                                    </div>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        </p>
                    </td>
                </ItemTemplate>
                <LayoutTemplate>
                    <table style="width:100%;">
                        <tbody>
                            <tr>
                                <td>
                                    <table id="groupPlaceholderContainer" runat="server" style="width:100%">
                                        <tr id="groupPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
            <asp:Button ID="selectAttrs_Btn" CssClass="btn btn-primary" runat="server" Text="Add to Description" OnClick="selectAttrs_Btn_Click" />
            </div>
        </div>
    </section>
</asp:Content>
