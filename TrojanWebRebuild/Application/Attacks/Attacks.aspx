﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attacks.aspx.cs" Inherits="TrojanWebRebuild.Application.Attacks.Attacks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%--    <script type="text/javascript" src="http://d3js.org/d3.v3.min.js"></script>--%>
    <script type="text/javascript" src="../../Scripts/d3/d3.js"></script>
    <script type="text/javascript" src="../../JavaScript/x3dom-full.js"></script>

<%--    <script type="text/javascript" src="../../JavaScript/AttackGraph.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="../../Content/x3dom.css"/>
    <div class="jumbotron"  style="text-align:center" runat="server">
        <table class="table table-striped table-bordered">
                <col width="230">
                <col width="130">
                <col width="130">
                <col width="130">
                <col width="230">
            <tr>
                <th></th>
                <th style="text-align:center">Accessibility</th>
                <th style="text-align:center">Time</th>
                <th style="text-align:center">Resources</th>
                <th></th>
            </tr>
            <tr>
                <td></td>
                <td><asp:DropDownList runat="server" ID="aDropList"></asp:DropDownList></td>
                <td><asp:DropDownList runat="server" ID="tDropList"></asp:DropDownList></td>
                <td><asp:DropDownList runat="server" ID="rDropList"></asp:DropDownList></td>
                <td></td>
            </tr>
        </table>
        <asp:Button class="btn btn-primary" ID="VisualGo_Btn" text="Visualize" runat="server" OnClick="VisualGo_Btn_Click" />
    </div>
    
<%--    <asp:UpdatePanel runat="server" ID="visPanel" UpdateMode="Conditional">
        <ContentTemplate>
                
        </ContentTemplate>
        <Triggers>
                <asp:AsyncPostBackTrigger ControlID="VisualGo_Btn" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <div class="jumbotron" runat="server">
    <div style="text-align:center">
        <div id="divPlot"></div>
    </div></div>
    <script>
        // Create a 3d scatter plot within d3 selection parent.
        var input;
        function scatterPlot3d(parent) {
            d3.select('#divPlot').style('width', "1000px").style('height', "650px");

            var x3d = parent
              .append("x3d")
                .style("width", parseInt(parent.style("width")) + "px")
                .style("height", parseInt(parent.style("height")) + "px")
                .style("border", "none")

            var scene = x3d.append("scene")

            scene.append("orthoviewpoint")
               .attr("centerOfRotation", [5, 5, 5])
               .attr("fieldOfView", [-5, -5, 15, 15])
               .attr("orientation", [-0.5, 1, 0.2, 1.12 * Math.PI / 4])
               .attr("position", [8, 4, 15])

            var rows = initializeDataGrid();
            var axisRange = [0, 10]; //Zoom
            var scales = [];
            var initialDuration = 0;
            var defaultDuration = 800;
            var ease = 'linear';
            var time = 0;
            var axisKeys = ["r", "t", "a"]

            // Helper functions for initializeAxis() and drawAxis()
            function axisName(name, axisIndex) {
                return ['r', 't', 'a'][axisIndex] + name;
            }

            function constVecWithAxisValue(otherValue, axisValue, axisIndex) {
                var result = [otherValue, otherValue, otherValue];
                result[axisIndex] = axisValue;
                return result;
            }

            // Used to make 2d elements visible
            function makeSolid(selection, color) {
                selection.append("appearance")
                  .append("material")
                     .attr("diffuseColor", color || "black")
                return selection;
            }

            // Initialize the axes lines and labels.
            function initializePlot() {
                initializeAxis(0);
                initializeAxis(1);
                initializeAxis(2);
            }

            function initializeAxis(axisIndex) {
                var key = axisKeys[axisIndex];
                drawAxis(axisIndex, key, initialDuration);

                var scaleMin = axisRange[0];
                var scaleMax = axisRange[1];

                // the axis line
                var newAxisLine = scene.append("transform")
                     .attr("class", axisName("Axis", axisIndex))
                     .attr("rotation", ([[0, 0, 0, 0], [0, 0, 1, Math.PI / 2], [0, 1, 0, -Math.PI / 2]][axisIndex]))
                  .append("shape")
                newAxisLine
                  .append("appearance")
                  .append("material")
                    .attr("emissiveColor", "lightgray")
                newAxisLine
                  .append("polyline2d")
                     // Line drawn along y axis does not render in Firefox, so draw one
                     // along the x axis instead and rotate it (above).
                    .attr("lineSegments", "0 0," + scaleMax + " 0")

                // axis labels
                var newAxisLabel = scene.append("transform")
                    .attr("class", axisName("AxisLabel", axisIndex))
                    .attr("translation", constVecWithAxisValue(0, scaleMin + 1.1 * (scaleMax - scaleMin), axisIndex))

                var newAxisLabelShape = newAxisLabel
                  .append("billboard")
                    .attr("axisOfRotation", "0 0 0") // face viewer
                  .append("shape")
                  .call(makeSolid)

                var labelFontSize = 0.6;

                newAxisLabelShape
                  .append("text")
                    .attr("class", axisName("AxisLabelText", axisIndex))
                    .attr("solid", "true")
                    .attr("string", key)
                 .append("fontstyle")
                    .attr("size", labelFontSize)
                    .attr("family", "SANS")
                    .attr("justify", "END MIDDLE")
            }

            // Assign key to axis, creating or updating its ticks, grid lines, and labels.
            function drawAxis(axisIndex, key, duration) {

                var scale = d3.scale.linear()
                  .domain([0, 3]) // demo data range
                  .range(axisRange)

                scales[axisIndex] = scale;

                var numTicks = 8;
                var tickSize = 0.1;
                var tickFontSize = 0.5;

                // ticks along each axis
                var ticks = scene.selectAll("." + axisName("Tick", axisIndex))
                   .data(scale.ticks(numTicks));
                var newTicks = ticks.enter()
                  .append("transform")
                    .attr("class", axisName("Tick", axisIndex));
                newTicks.append("shape").call(makeSolid)
                  .append("box")
                    .attr("size", tickSize + " " + tickSize + " " + tickSize);
                // enter + update
                ticks.transition().duration(duration)
                  .attr("translation", function (tick) {
                      return constVecWithAxisValue(0, scale(tick), axisIndex);
                  })
                ticks.exit().remove();

                // tick labels
                var tickLabels = ticks.selectAll("billboard shape text")
                  .data(function (d) { return [d]; });
                var newTickLabels = tickLabels.enter()
                  .append("billboard")
                     .attr("axisOfRotation", "0 0 0")
                  .append("shape")
                  .call(makeSolid)
                newTickLabels.append("text")
                  .attr("string", scale.tickFormat(10))
                  .attr("solid", "true")
                  .append("fontstyle")
                    .attr("size", tickFontSize)
                    .attr("family", "SANS")
                    .attr("justify", "END MIDDLE");
                tickLabels // enter + update
                  .attr("string", scale.tickFormat(10))
                tickLabels.exit().remove();

                // base grid lines
                if (axisIndex == 0 || axisIndex == 2) {

                    var gridLines = scene.selectAll("." + axisName("GridLine", axisIndex))
                       .data(scale.ticks(numTicks));
                    gridLines.exit().remove();

                    var newGridLines = gridLines.enter()
                      .append("transform")
                        .attr("class", axisName("GridLine", axisIndex))
                        .attr("rotation", axisIndex == 0 ? [0, 1, 0, -Math.PI / 2] : [0, 0, 0, 0])
                      .append("shape")

                    newGridLines.append("appearance")
                      .append("material")
                        .attr("emissiveColor", "gray")
                    newGridLines.append("polyline2d");

                    gridLines.selectAll("shape polyline2d").transition().duration(duration)
                      .attr("lineSegments", "0 0, " + axisRange[1] + " 0")

                    gridLines.transition().duration(duration)
                       .attr("translation", axisIndex == 0
                          ? function (d) { return scale(d) + " 0 0"; }
                          : function (d) { return "0 0 " + scale(d); }
                        )
                }
            }

            // Update the data points (spheres) and stems.
            function plotData(duration) {

                if (!rows) {
                    console.log("no rows to plot.")
                    return;
                }

                var r = scales[0], t = scales[1], a = scales[2];
                var sphereRadius = 0.2;

                // Draw a sphere at each x,y,z coordinate.
                var datapoints = scene.selectAll(".datapoint").data(rows);
                datapoints.exit().remove()

                var newDatapoints = datapoints.enter()
                  .append("transform")
                    .attr("class", "datapoint")
                    .attr("scale", [sphereRadius, sphereRadius, sphereRadius])
                  .append("shape");
                newDatapoints
                  .append("appearance")
                  .append("material");
                newDatapoints
                  .append("sphere")
                // Does not work on Chrome; use transform instead
                //.attr("radius", sphereRadius)

                datapoints.selectAll("shape appearance material")
                    .attr("diffuseColor", 'steelblue')

                datapoints.transition().ease(ease).duration(duration)
                    .attr("translation", function (row) {
                        return r(row[axisKeys[0]]) + " " + t(row[axisKeys[1]]) + " " + a(row[axisKeys[2]])
                    })

                // Draw a stem from the x-z plane to each sphere at elevation y.
                // This convention was chosen to be consistent with x3d primitive ElevationGrid. 
                var stems = scene.selectAll(".stem").data(rows);
                stems.exit().remove();

                var newStems = stems.enter()
                  .append("transform")
                    .attr("class", "stem")
                  .append("shape");
                newStems
                  .append("appearance")
                  .append("material")
                    .attr("emissiveColor", "gray")
                newStems
                  .append("polyline2d")
                    .attr("lineSegments", function (row) { return "0 1, 0 0"; })

                stems.transition().ease(ease).duration(duration)
                    .attr("translation",
                       function (row) { return r(row[axisKeys[0]]) + " 0 " + a(row[axisKeys[2]]); })
                    .attr("scale",
                       function (row) { return [1, t(row[axisKeys[1]])]; })
            }

            //Generates the data Points
            function initializeDataGrid() {
                //var rows = [];
                // Follow the convention where y(x,z) is elevation.
                //for (var x = 0; x <= 3; x += 1) {
                //    for (var z = 0; z <= 3; z += 1) {
                //        rows.push({ x: x, y: z, z: z });
                //    }
                //}
                return input;
            }

            function updateData() {
                time += Math.PI / 8;
                if (x3d.node() && x3d.node().runtime) {
                    for (var rw = 0; rw < rows.length; ++rw) {
                        var r = rows[rw].r;
                        var a = rows[rw].a;
                        //rows[r].y = 5*( Math.sin(0.5*x + time) * Math.cos(0.25*z + time));
                        rows[rw].t = rows[rw].t;
                    }
                    plotData(defaultDuration);
                } else {
                    console.log('x3d not ready.');
                }
            }
            setInterval(updateData, defaultDuration);
            initializeDataGrid();
            initializePlot();
        }

        function start() {
            scatterPlot3d(d3.select('#divPlot'));
        }
    </script>
</asp:Content>
