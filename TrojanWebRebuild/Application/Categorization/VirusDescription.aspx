<%@ Page Title="" Language="C#" MasterPageFile="~/Application/Categorization/Categorization.Master" AutoEventWireup="true" CodeBehind="VirusDescription.aspx.cs" Inherits="TrojanWebRebuild.VirusDescription" %>
<asp:Content ID="DescriptionContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="../../Scripts/d3/d3.js"></script>
    <script type="text/javascript" src="JavaScript/DirectedGraph.js"></script>
    <style>

        .Chip{
        /*fill: red;*/
        fill: yellow;
        stroke: black;
        stroke-width: 2px;
    }
    .Abstraction{
        /*fill: orange;*/
        fill: yellow;
        stroke: black;
        stroke-width: 2px;
    }
    .Properties{
        /*fill: lightgreen;*/
        fill: yellow;
        stroke: black;
        stroke-width: 2px;
    }
    .Location{
        fill: yellow;
        stroke: black;
        stroke-width: 2px;
    }
    path.link {
	  fill: none;
	  stroke: #000;
	  stroke-width: 3px;
	  cursor: default;
	}

	.circle{
		fill: transparent;
		stroke : black;
		stroke-width: 2px;
	}

    </style>
    <link rel="stylesheet" type="text/css" href="Content/visualizer.css" />
    <link rel="stylesheet" type="text/css" href="Content/Site.css" />
    <div id="VirusDescriptionTitle" runat="server" class="ContentHead"><h1>Virus Description</h1></div>
    <div id="NoSelected" align="center" runat="server" class="ContentHead"><h1>&nbsp&nbsp</h1><h3>** No Attributes Selected **</h3></div>
    <asp:GridView ID="DescriptionList" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Vertical" CellPadding="4"
        ItemType="TrojanWebRebuild.Models.Virus_Item" SelectMethod="GetVirusDescription" 
        CssClass="table table-striped table-bordered" EnableCaching ="true">   
        <Columns>
        <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />        
        <asp:BoundField DataField="Attribute.AttributeName" HeaderText="Name" />        
        <asp:BoundField DataField="Attribute.CategoryName" HeaderText="Category" />
        <asp:BoundField DataField="Attribute.F_in" HeaderText="F_in" />
        <asp:BoundField DataField="Attribute.F_out" HeaderText="F_out" />     
        <asp:TemplateField   HeaderText="Select Attribute">            
                <ItemTemplate>
                    <asp:CheckBox id="On_Off_CheckBox" runat="server"></asp:CheckBox>
                </ItemTemplate>        
        </asp:TemplateField >
        <asp:TemplateField HeaderText="On/Off">
            <ItemTemplate>
                <asp:Label ID ="Attrib_OnOff" runat="server" Text='<%#Item.On_Off ? "On" : "Off" %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Remove Item">            
                <ItemTemplate>
                    <asp:CheckBox id="Remove" runat="server"></asp:CheckBox>
                </ItemTemplate>        
        </asp:TemplateField>    
        </Columns>    
    </asp:GridView>
<%--    <div id="labels" runat="server">
        <p></p>
        <strong>
            <asp:Label ID="LabelTotalText" runat="server" Text="Total Attributes: "></asp:Label>
            <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div>
    <div>
        <strong>
            <asp:Label ID="LabelTotalF_in" runat="server" Text="Total F_in: "></asp:Label>
            <asp:Label ID="lblTotalF_in" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div>
    <div>
        <strong>
            <asp:Label ID="LabelTotalF_out" runat="server" Text="Total F_out: "></asp:Label>
            <asp:Label ID="lblTotalF_out" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div>--%>
    <br />
    <div id="startOverDiv" runat="server" align="center" class="Content">
        <div id ="noResult" runat="server" align="center" class="Content">
            <h4>** This Combination Has No Result **</h4>
            <br />
        </div>
        <asp:Button ID="startOverBtn" class="btn btn-primary" runat="server" Text="Start Over" OnClick="ClearBtn_Click" />
    </div>
    <br />
    <table id="buttonTable" runat="server" style="margin: 0 auto; border-collapse: separate; border-spacing: 5px; font-weight: bold;"> 
    <tr>
        <td>
        <asp:Button ID="UpdateBtn" class="btn btn-primary" runat="server" Text="Update" OnClick="UpdateBtn_Click" />
        </td>
        <td>
            <asp:Button ID="BuildCombo" class="btn btn-primary" runat="server" Text="Build Combo" OnClick="BuildComboBtn_Click" />
        </td>
        <td>
            <asp:Button ID="BuildRow" class="btn btn-primary" runat="server" Text="Build Row" OnClick="BuildRowBtn_Click" />
        </td>
        <td>
            <asp:Button ID="BuildCol" class="btn btn-primary" runat="server" Text="Build Column" OnClick="BuildColumnBtn_Click" />
        </td>
        <td>
            <asp:Button ID="ClearBtn" class="btn btn-primary" runat="server" Text="Clear" OnClick="ClearBtn_Click" />
        </td>
        <%--<td>
            <asp:Button ID="VisualizeBtn" class="btn btn-primary" runat="server" Text="Visualize" OnClick="VisualizeBtn_Click" />
        </td>--%>
    </tr>
    </table>
    
    <div id="abstractionResults" runat="server" class="ContentHead"><h1>Abstraction Results</h1></div>
    <asp:GridView ID="abstractionGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>
    <div id="direct" runat="server" class="ContentHead"><h1>Direct Connections to Insertion</h1></div>
    <asp:GridView ID="directGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>
    <div id="indirect" runat="server" class="ContentHead"><h1>Indirect Connections to Insertion</h1></div>
    
    <asp:GridView ID="indirectGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>


    <div id="nodesDiv" runat="server" class="ContentHead"><h1>Attributes</h1></div>
    
    <asp:GridView ID="nodesGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>

    <div id="RowResults" runat="server" class="ContentHead"><h1>Row Analysis Results</h1><h4>Attributes shared by each row</h4></div>
    <asp:GridView ID="RowGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>
    <div id="ColumnResults" runat="server" class="ContentHead"><h1>Column Analysis Results</h1><h4>Attributes shared by each column</h4></div>
    <asp:GridView ID="ColumnGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>
    <div id="ConnectionsResults" runat="server" class="ContentHead"><h1>Connections</h1></div>
    <asp:GridView ID="ConnectionsGrid" runat="server"  AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" CssClass="table table-striped table-bordered" ItemType="TrojanWebRebuild.Models.Connection">
        <Columns>
            <asp:BoundField DataField="source" HeaderText="Source" SortExpression="source" />
            <asp:BoundField DataField="target" HeaderText="Target" />        
            <asp:BoundField DataField="direct" HeaderText="Direct Connection" />
        </Columns>
    </asp:GridView>
    <div id="Locationlbl" runat="server" class="ContentHead"><h1>Location Results</h1></div>
    <asp:GridView ID="LocationGrid" runat="server" AutoGenerateColumns="false" ShowFooter="false" GridLines="Vertical" CellPadding="4" ItemType="TrojanWebRebuild.Models.Attribute" CssClass="table table-striped table-bordered">
        <Columns>
            <asp:BoundField DataField="AttributeId" HeaderText="ID" SortExpression="AttributeId" />
            <asp:BoundField DataField="AttributeName" HeaderText="Name" />        
            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
        </Columns>
    </asp:GridView>
    <div>&nbsp</div>
    
    <div id="notes" runat="server" class="Content"><h2>**Notes**</h2></div>
    <div id="canNot" runat="server" class="Content"><h4>Select attributes to build a virus</h4></div>
    <div id="abstractionEmpty" runat="server" class="Content"><h4>There are no access points to Abstraction Category</h4></div>
    <div id="notBuilt" runat="server" class="Content"><h4>Virus must be built before visualization</h4></div>
    <div id="directNone" runat="server" class="Content"><h4>No Direct Connections to Insertion</h4></div>
    <div id="indirectNone" runat="server" class="Content"><h4>No Indirect Connections to Insertion</h4></div>
    <div id="abstractionNone" runat="server" class="Content"><h4>No Results for Abstraction Category</h4></div>

    <div id="visualRepLbl" runat="server">
        <h2>Visual Representation</h2>
    </div>
    <div id="jumboWrap" runat="server" >
        <div id="visJumboContainer" style="text-align:center" >
            
            <div id="visrep" ></div>
        </div>
        <div id="chkBxDiv" runat="server" class="panel panel-primary" style="width:960px; margin-left:auto; margin-right:auto;">
            <div class="panel-heading" style="font-weight:bold;">Attribute Removal</div>
            <div class="panel-body" style="text-align:center">
                <asp:DropDownList ID="attrDropDownList" runat="server"></asp:DropDownList>
                <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Remove Selected Attribute" style="font-weight:bold;" OnClick="removeAttrBtn_Click"/>
            </div>
        </div>
        <br />
        <h3>Severity Rating</h3>
        <hr />
        <div class="well">
            <%--<asp:GridView ID="sevGrid" runat="server" AutoGenerateColumns="true" CssClass="table table-striped table-bordered"></asp:GridView>--%>
            <asp:Table runat="server" ID="Table3" CssClass="table table-striped table-bordered">
                    <asp:TableHeaderRow runat="server" ID="TableHeaderRow3">
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Insertion">I<sub>R</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Abstraction">I<sub>A</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Effect">I<sub>E</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Logic Type">I<sub>L</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Functionality">I<sub>F</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Activation">I<sub>C</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Physical Layout">I<sub>P</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="border-right-width: medium; text-align:center" Font-Size="Large"><span title="Location">I<sub>O</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Insertion">C<sub>R</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Abstraction">C<sub>A</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Effect">C<sub>E</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Logic Type">C<sub>L</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Functionality">C<sub>F</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Activation">C<sub>C</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Physical Layout">C<sub>P</sub></span></asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align:center" Font-Size="Large"><span title="Location">C<sub>O</sub></span></asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow runat="server" ID="TableRow3">
                        <asp:TableCell style="text-align:center" ID="trjnCelliR">
                            <label id="trjnCelliR_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliA">
                            <label id="trjnCelliA_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliE">
                            <label id="trjnCelliE_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliL">
                            <label id="trjnCelliL_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliF">
                            <label id="trjnCelliF_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliC">
                            <label id="trjnCelliC_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCelliP">
                            <label id="trjnCelliP_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell ID="trjnCelliO" Style="border-right-width: medium; text-align:center">
                            <label id="trjnCelliO_lbl" runat="server"></label>
                        </asp:TableCell>


                        <asp:TableCell style="text-align:center" ID="trjnCellcR">
                            <label id="trjnCellcR_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcA">
                            <label id="trjnCellcA_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcE">
                            <label id="trjnCellcE_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcL">
                            <label id="trjnCellcL_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcF">
                            <label id="trjnCellcF_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcC">
                            <label id="trjnCellcC_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcP">
                            <label id="trjnCellcP_lbl" runat="server"></label>
                        </asp:TableCell>
                        <asp:TableCell style="text-align:center" ID="trjnCellcO">
                            <label id="trjnCellcO_lbl" runat="server"></label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
        </div>
        <asp:UpdatePanel runat="server" ID="updatePnl" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="text-align:center;">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="SavedMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="savedText" />
                        </p>
                    </asp:PlaceHolder>
                    Give your work a nickname &nbsp;
                    <asp:TextBox ID="saveNameTxtBx" Columns="30" runat="server"></asp:TextBox>
                    <hr />
                    <asp:Button ID="saveBtn" runat="server" Text="Save Virus" CssClass="btn btn-primary" OnClick="saveBtn_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <script src="http://marvl.infotech.monash.edu/webcola/cola.v3.min.js"></script>
    <script type="text/JavaScript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <script type="text/javascript">

        var width = 960, height = 500, colors = d3.scale.category10();
        var svg = null, force = null;
        var nod, edges; //C# puts data in here
        var circle = null, path = null;
        var nodes = null, links = null;
        var nodesArray = null, linkArray = null;
        var virusId = null;
        var circleGroup = null; var radius = 12;
        var markerWidth = 6, markerHeight = 4,
        refX = radius + (markerWidth) + 1,
        refY = 0;
        var step = 35, numberOfNodes = 0;

        function initVis (element, numNodes, numEdges) {
            numberOfNodes = numNodes;
            svg.selectAll('*').remove();

            svg.append('svg:defs').append('svg:marker')
			.attr("id", "arrow")
			.attr("viewBox", "0 -5 10 10")
			.attr("refX", 9)
			.attr("refY", refY)
			.attr("markerWidth", markerWidth)
			.attr("markerHeight", markerHeight)
			.attr("orient", "auto")
			.append("svg:path")
			.attr("d", "M0,-5 L10,0L0,5").attr('fill', "#000");

            svg.append('svg:defs').append('svg:marker')
                .attr("id", "arrow2")
                .attr("viewBox", "0 -5 10 10")
                .attr("refX", 9)
                .attr("refY", 0)
                .attr("markerWidth", markerWidth)
                .attr("markerHeight", markerHeight)
                .attr("orient", "auto")
                .append("svg:path")
                .attr("d", "M0,-5 L10,0L0,5").attr('fill', "#000");


            var numOnLine = 0, numProperties = 0, numLocations = 0;
            for (var j = 0; j < numNodes; j++) {
                if ((nod[j].Category == "Chip Life Cycle") || (nod[j].Category == "Abstraction")) ++numOnLine;
                else if (nod[j].Category == "Properties") ++numProperties;
                else {
                    ++numLocations;
                }
            }
            var i = 0; var L = 40, r = 12, lineLimit = 10;
            var d = 2 * r + L;
            var R = (numOnLine - 1) * d;
            var m = width / 2;
            var lineHeight = height / 3;
            var X, Y;
            var propertiesRadius = 0.5 * (radius * numProperties);
            var locationRadius = 0.5 * (radius * numLocations);
            
            var dropDown = (5 * step);
            var W = 0, Z = 0;

            var locationCentX = width / 3;
            var propertiesCentX = 2 * locationCentX;
            var propertiesCentY = (lineHeight) + dropDown;
            var locationCentY = propertiesCentY;
            var lowestID = 100, maxX = 0;
            //Create Node Data
            nodes = d3.range(numNodes+1).map(function () {
                if (i == 0) {
                    ++i;
                    return {
                        x: 0,
                        y: 0,
                        id: 0,
                        reflexive: true,
                        r: radius,
                        Fin: 0,
                        Fout: 0,
                        Category: "Filler",
                        Description: "Filler"
                    }
                }
                else if (nod[i - 1].Category == "Properties") {
                    if (numProperties == 1) {
                        X = propertiesCentX;
                    }
                    else {
                        X = propertiesCentX + (propertiesRadius * Math.cos((2 * W * Math.PI) / numProperties));
                    }
                    
                    Y = (lineHeight + dropDown + (propertiesRadius * Math.sin((2 * W * Math.PI) / numProperties)));
                    ++W;
                }
                else if (nod[i - 1].Category == "Location") {
                    if (numLocations == 1) {
                        X = locationCentX;
                    }
                    else {
                        X = (locationCentX + (locationRadius * Math.cos((2 * Z * Math.PI) / numLocations)));
                    }
                    Y = (lineHeight + dropDown + (locationRadius * Math.sin((2 * Z * Math.PI) / numLocations)));
                    ++Z;
                }
                //Insertion or Abstraction
                else {
                    X = m - (R / 2) + (i * d) - 100;
                    Y = lineHeight;
                }
                if (nod[i - 1].nodeID < lowestID) lowestID = nod[i - 1].nodeID;
                if(X > maxX) maxX = X;
                ++i;
                return {
                    x: X,
                    y: Y,
                    id: nod[i - 2].nodeID,
                    reflexive: true,
                    r: radius,
                    Fin: nod[i - 2].F_in,
                    Fout: nod[i - 2].F_out,
                    Category: nod[i - 2].Category,
                    name: nod[i - 2].nodeName,
                    Description: nod[i-2].Description
                };
            });
            //for (var i = 0; i < numEdges; ++i) {
            //    d3.select(element).append("h3").text("Node " + i + ": " + nodes[i].id);
            //}
            lowestID = lowestID - 1;


            //Create Link Data
            var sources = []; var targets = [];
            i = 0;
            var under = false;
            links = d3.range(numEdges).map(function () {
                i++;
                if(edges[i - 1].direct == true){
                    if (!isPresent(sources, edges[i - 1].source)) {
                        sources.push(edges[i - 1].source);
                        under = false;
                    }
                    else {
                        under = true;
                    }
                    if (!isPresent(targets, edges[i - 1].target)) {
                        targets.push(edges[i - 1].target);
                        under = false;
                    }
                    else {
                        under = true;
                    }
                }
                if ((getNode(edges[i - 1].source).Category == "Properties") || (getNode(edges[i - 1].target).Category == "Properties")) {
                    
                    return {
                        source: 0,
                        target: 0,
                        direct: false,
                        left: false,
                        right: true,
                        under: under
                    }
                }
                else {
                    return {
                        source: getNode(edges[i - 1].source),
                        target: getNode(edges[i - 1].target),
                        direct: edges[i - 1].direct,
                        left: false,
                        right: true,
                        under: under
                    }
                }
                
            });
            var propOuterRadius = (propertiesRadius + (radius * 2));
            var locationOuterRadius = (locationRadius + (radius * 2));
            //Add the paths between nodes to the page
            var path = svg.append("svg:g").selectAll("path").data(links).enter().append("svg:path").attr("class", "link").attr("d", function (d) { return linkPath(d) }).attr('marker-end', 'url(#arrow)').append("svg:title").text(function (d) { return pathTitle(d);});
            var propertiesCircle = svg.append("circle").attr("class", "circle").attr("cx", propertiesCentX).attr("cy", propertiesCentY).attr("r", propOuterRadius);
            var locationCircle = svg.append("circle").attr("class", "circle").attr("cx", locationCentX).attr("cy", locationCentY).attr("r", locationOuterRadius);
            //Add Nodes to page
            var circleGroup = svg.selectAll("g").data(nodes);
            var groupEnter = circleGroup.enter().append("g").attr("transform", function (d) {
                return "translate(" + [d.x, d.y] + ")";
            }).style("cursor", "pointer");
            var circle = groupEnter.append("circle").attr("cx", 0).attr("cy", 0).attr("r", radius).attr("class", function (d) { return classSelector(d) }).append("svg:title").text(function (d) { return labelGen(d); });
            var label = circleGroup.append("text").attr("y", 2).attr("x", 0).text(function (d) { return d.id; }).attr({ "alignment-baseline": "middle", "text-anchor": "middle" }).style("class", "id");

            //var propOuterRadius = (propertiesRadius + (radius * 2));
            //var locationOuterRadius = (locationRadius + (radius * 2));

            //Add highlight circle for properties category
            var propertiesPath = svg.append("svg:g").append("svg:path")
                                    .attr("class", "link").attr("d", function () {
                                        var turnPoint = pathDistanceCalc(propOuterRadius, nodes[numOnLine].x + radius, propertiesCentX)
                                        var start = "M " + (nodes[numOnLine].x + radius) + ", " + nodes[numOnLine].y;
                                        var p1 = "L " + turnPoint + ", " + nodes[numOnLine].y;
                                        var p2 = "L " + turnPoint + ", " + (propertiesCentY);
                                        var end = "L " + (propertiesCentX + (propertiesRadius + (radius * 2))) + ", " + propertiesCentY;
                                        var str = start + p1 + p2 + end;
                                        //console.log("Properties String: " + str);
                                        return str;
                                    }).attr('marker-end', 'url(#arrow2)').append("svg:title").text("Source: Abstraction \nTarget: Properties");

            //Add highlight circle for Location category
            
            var locationPath = svg.append("svg:g").append("svg:path").attr("class", "link").attr("d", function (d) {
                var start = "M " + (propertiesCentX - propOuterRadius) + "," + locationCentY;
                var end = "L " + (locationCentX + locationOuterRadius) + "," + locationCentY
                return (start + end);
            }).attr('marker-end', 'url(#arrow2)').append("svg:title").text("Source: Properties \nTarget: Locations");
            

        }
        function pathDistanceCalc(propOuterRadius, lastNodesEdge, propertiesCentX) {
            var propertyCircleEdge = propOuterRadius + propertiesCentX;
            if (propertyCircleEdge > lastNodesEdge) {
                return propertyCircleEdge + 2 * radius;
            }
            else {
                return lastNodesEdge + 50;
            }
        }
        //Create the labels for mouse-over
        function labelGen(d) {
            var str = "Attribute: " + d.name + "\n" + "Category: " + d.Category + "\n"+ "F_in: " + d.Fin + "\n" + "F_out: " + d.Fout + "\n" + "Description: " + d.Description;
            return str;
        }
        function pathTitle(d){
            var str = "Source: " + d.source.id + "\nTarget: " + d.target.id + "\nDirect: " + d.direct;
            return str;
        }
        var direction = 1;
        var numOfUps = 1; var numOfDowns = 1;
        var linksThatGoUnder = false;

        function getNode(d) {
            for (var i = 0; i < numberOfNodes; ++i) {
                if (d == nodes[i].id) {
                    return nodes[i];
                }
            }
        }

        //Generate the string required to direct paths
        function linkPath(d) {
            var str;
            if (d.direct != true) {
                str = "M " + d.source.x + ", " + d.source.y + " L " + (d.target.x - radius) + ", " + d.target.y;
            }
            else {
                
                var distance;
                if(d.under == true){
                    direction = 1; //Goes under the line
                    distance = step * calcStep(numOfDowns);
                    numOfDowns++;
                }
                else {
                    direction = -1;
                    distance = step * calcStep(numOfUps);
                    ++numOfUps;
                }
                var dy = direction * distance;
                var height = d.source.y + dy;
                var p1 = "M " + d.source.x + ", " + (d.source.y + (radius * direction));
                var p2 = " L " + d.source.x + ", " + (height);
                var p3 = " L " + d.target.x + ", " + height;
                var p4 = " L " + d.target.x + ", " + (d.target.y + (direction * radius));

                //M(source.x, source.y) + L(source.x, height) + L(target.x, height) + L(target.x, target.y);
                str = p1 + p2 + p3 + p4;
            }
            return str;
        }

        //Calculate the height of each link
        function calcStep(x){
            if(x == 1){
                return x;
            }
            else if (x == 2) {
                return 1.5;
            }
            else if(x == 3){
                return 2;
            }
            else if(x == 4){
                return 2.5;
            }
            else if (x == 5){
                return 3;
            }
            else if (x == 6){
                return 3.5;
            }
            else if (x == 7) {
                return 4;
            }
            else if (x == 8) {
                return 4.5;
            }
            else return 5;
        }

        //Check to see if a particular node is in an array
        function isPresent(list, x) {
            for (var i = 0; i < list.length; ++i) {
                if (list[i] == x) return true;
            }
            return false;
        }

        //Return the css class desired
        function classSelector(d) {
            if (d.Category == "Chip Life Cycle") {
                return "Chip";
            }
            else if (d.Category == "Abstraction") {
                return "Abstraction";
            }
            else if (d.Category == "Properties") {
                return "Properties";
            }
            else {
                return "Location";
            }
        }
        //Called by Server - Starts Javascript         
        function visualize(element, numEdges, numNodes) {
            svg = d3.selectAll(element).append('svg').attr('width', width).attr('height', height);
            initVis(element, numNodes, numEdges);
        }
    </script>
</asp:Content>
