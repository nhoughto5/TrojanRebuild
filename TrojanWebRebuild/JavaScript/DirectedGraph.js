function Visualizer(el) {

    // Add and remove elements on the graph object
    this.addNode = function (id, Cat) {
        nodes.push({ "id": id, "Category": Cat });
        update();
    }

    this.removeNode = function (id) {
        var i = 0;
        var n = findNode(id);
        while (i < links.length) {
            if ((links[i]['source'] === n) || (links[i]['target'] == n)) links.splice(i, 1);
            else i++;
        }
        var index = findNodeIndex(id);
        if (index !== undefined) {
            nodes.splice(index, 1);
            update();
        }
    }

    this.addLink = function (sourceId, targetId) {
        var sourceNode = findNode(sourceId);
        var targetNode = findNode(targetId);

        if ((sourceNode !== undefined) && (targetNode !== undefined)) {
            links.push({ "source": sourceNode, "target": targetNode });
            update();
        }
    }

    var findNode = function (id) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].id === id)
                return nodes[i]
        };
    }

    var findNodeIndex = function (id) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].id === id)
                return i
        };
    }

    // set up the D3 visualisation in the specified element
    var w = $(el).innerWidth(),
        h = $(el).innerHeight();

    var vis = this.vis = d3.select(el).append("svg:svg")
        .attr("width", w)
        .attr("height", h);

    var force = d3.layout.force()
        .gravity(.05)
        .distance(100)
        .charge(-100)
        .size([w, h]);

    var nodes = force.nodes(),
        links = force.links();

    var update = function () {

        var link = vis.selectAll("line.link")
            .data(links, function (d) { return d.source.id + "-" + d.target.id; });

        link.enter().insert("line")
            .attr("class", "link");

        link.exit().remove();

        var node = vis.selectAll("g.node")
            .data(nodes, function (d) { return d.id; });

        var nodeEnter = node.enter().append("g")
            .attr("class", "node").attr('r', 12).style('fill', function (d) {
                if (d.Category == 1) return "red";
                else if (d.Category == 2) return "blue";
                else if (d.Category == 3) return "yellow";
                else return "green";
            }).style("stroke", "black").classed('reflexive', function (d) { return d.reflexive; }).on('mouseover', function (d) {
                if (!mousedown_node || d === mousedown_node) return;
                // enlarge target node
                d3.select(this).attr('transform', 'scale(1.1)');
            })
            .on('mouseout', function (d) {
          if (!mousedown_node || d === mousedown_node) return;
          // unenlarge target node
          d3.select(this).attr('transform', '');
      })
            .on('mousedown', function (d) {
          if (d3.event.ctrlKey) return;

          // select node
          mousedown_node = d;
          if (mousedown_node === selected_node) selected_node = null;
          else selected_node = mousedown_node;
          selected_link = null;

          // reposition drag line
          drag_line
            .style('marker-end', 'url(#end-arrow)')
            .classed('hidden', false)
            .attr('d', 'M' + mousedown_node.x + ',' + mousedown_node.y + 'L' + mousedown_node.x + ',' + mousedown_node.y);

          restart();
      })
            .on('mouseup', function (d) {
          if (!mousedown_node) return;

          // needed by FF
          drag_line
            .classed('hidden', true)
            .style('marker-end', '');

          // check for drag-to-self
          mouseup_node = d;
          if (mouseup_node === mousedown_node) { resetMouseVars(); return; }

          // unenlarge target node
          d3.select(this).attr('transform', '');

          // add link to graph (update if exists)
          // NB: links are strictly source < target; arrows separately specified by booleans
          var source, target, direction;
          if (mousedown_node.id < mouseup_node.id) {
              source = mousedown_node;
              target = mouseup_node;
              direction = 'right';
          } else {
              source = mouseup_node;
              target = mousedown_node;
              direction = 'left';
          }

          var link;
          link = links.filter(function (l) {
              return (l.source === source && l.target === target);
          })[0];

          if (link) {
              link[direction] = true;
          } else {
              link = { source: source, target: target, left: false, right: false };
              link[direction] = true;
              links.push(link);
          }

          // select new link
          selected_link = link;
          selected_node = null;
          restart();
            });

        //show node IDs
        nodeEnter.append('svg:text').attr('x', 0).attr('y', 4).attr('class', 'id').text(function (d) { return d.id; });

        //remove old nodes
        node.exit().remove();

        force.on("tick", function () {
            link.attr("x1", function (d) { return d.source.x; })
                .attr("y1", function (d) { return d.source.y; })
                .attr("x2", function (d) { return d.target.x; })
                .attr("y2", function (d) { return d.target.y; });

            node.attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });
        });

        //Restart the force layout.
        force.start();
    }

    // Make it all go
    update();
}

//var nodeEdges = [1,2,3,4];


function visualize(element) {
    var nodeEdges = [1, 2, 3, 4];
    //graph = new Visualizer(element);
    //for (var i = 0; i < nodesEdges.length ; ++i) {
    //    graph.addLink(nodeEdges[i].source, nodeEdge[i].target);
    //}
    d3.select(element).selectAll("h2").data(nodeEdges).enter().append("h2").text(function (d) { return "We did it " + d;});
}
