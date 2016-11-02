<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TrojanWebRebuild.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><%:Title %></h2>
        
        <div id="listBlock" style="float:left; width: 30%">
            <div>&nbsp</div>
            <div class="row">
                <div class="col-xs-11">
                    <ul class="list-group">
                        <li class="list-group-item" style="cursor:pointer" data-toggle="modal" data-target="#nickModal">Nick Houghton</li>
                        <li class="list-group-item" style="cursor:pointer" data-toggle="modal" data-target="#samerModal">Samer Moein</li>
                        <li class="list-group-item" style="cursor:pointer" data-toggle="modal" data-target="#fayezModal">Fayez Gebali</li>
                    </ul>
                </div>
            </div>
        </div>

        <div id="rightContent" class="jumbotron" style="float:right; width: 70%">
        <h4>History</h4>
            <p>The hardware security system was developed at the University of Victoria in 2015 by Nick Houghton and Samer Moein under the supervision of Dr. Fayez Gebali</p>
            <a id="uvicLink" href="https://www.uvic.ca/" >
                <img ID="uvicImage" src="Images/logo-uvic.jpg" style="float:right; width:60%"/>
            </a>
            <a id="engLink" href="http://www.uvic.ca/engineering/ece/" >
                <img id="engImg" src="Images/engBuild.jpg" style="float:left; width:35%" />
            </a>
        </div>
    </div>
    

    <!-- Nick Modal -->
    <div class="modal fade" id="nickModal" role="dialog" draggable="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Nicholas Houghton</h4>
                </div>
                <div class="modal-body" style="overflow:auto">
                    <div class="row-fluid">
                        <div id="address" style="float:left; width:60%">
                            <p>Nick Houghton recieved his bachelors in computer engineering in the spring of 2015 from the University of Victoria. He is currently completing his masters of applies science under the supervision
                                of Dr. Fayez Gebali. </p>
                        </div>
                        <img id="nickImage" src="Images/Nickimg.jpg" style="float:right; width:30%; height:30%"/>
                    </div>
                </div>
                <div class="modal-footer">
                    <div style="float:left">
                        <strong>Email: </strong><a href="mailto:nhoughto@uvic.ca?subject=Hardware Security System">nhoughto@uvic.ca</a>
                    </div>
                    
                    <a href="https://ca.linkedin.com/in/nghoughton" style="float:right">
                        <img id="linkedIn" src="Images/linkedIn.png" style="width:10%; height:10%"/>
                    </a>
                </div>
            </div>  
        </div>
    </div>

    <!-- Samer Modal -->
     <div class="modal fade" id="samerModal" role="dialog" draggable="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Samer Moein</h4>
                </div>
                <div class="modal-body" style="overflow:auto">
                    <div class="row-fluid">
                        <div id="addressSamer" style="float:left; width:60%">
                            <p>Samer Moein received his B.Sc. and M.Sc. in computer engineering from Kuwait University,
                                Kuwait. In 2013 he received a fellowship at the University of Victoria
                                and began his PhD in the Department of
                                Electrical and Computer Engineering. His research interests include hardware
                                security, encryption algorithms and encryption processors.</p>
                        </div>
                        <img id="samerImage" src="Images/smerimg.jpg" style="float:right; width:30%; height:30%"/>
                    </div>
                </div>
                <div class="modal-footer">
                    <div style="float:left">
                        <strong>Email: </strong><a href="mailto:samerm@uvic.ca?subject=Hardware Security System">samerm@uvic.ca</a>
                    </div>
                    
                    <a href="https://ca.linkedin.com/pub/samer-moein/80/b0/24a" style="float:right">
                        <img id="linkedInSamer" src="Images/linkedIn.png" style="width:10%; height:10%"/>
                    </a>
                </div>
            </div>  
        </div>
    </div>

    <!-- Fayez Modal -->
     <div class="modal fade" id="fayezModal" role="dialog" draggable="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Dr. Fayez Gebali</h4>
                </div>
                <div class="modal-body" style="overflow:auto">
                    <div class="row-fluid">
                        <div id="addressFayez" style="float:left; width:60%">
                            <p>Dr. Gebali received his B.Sc. in Electrical Engineering (first class honors) from Cairo University, his
                                B.Sc. in Mathematics (first class honors) from Ain Shams University, and his Ph.D. degree in Electrical
                                Engineering form the University of British Columbia where he was a holder of an NSERC postgraduate
                                scholarship. Dr. Gebali is a Professor and Chair of the Department of Electrical and Computer Engineering
                                at the University of Victoria. His research interests include parallel algorithms, networks-on-chip, threedimensional
                                integrated circuits, digital communications, and computer arithmetic</p>
                        </div>
                        <img id="fayezImage" src="Images/fayezImg.jpg" style="float:right; width:30%; height:30%"/>
                    </div>
                </div>
                <div class="modal-footer">
                    <div style="float:left">
                        <strong>Email: </strong><a href="mailto:fayez@uvic.ca?subject=Hardware Security System">fayez@uvic.ca</a>
                    </div>
                    
                    <a href="https://ca.linkedin.com/pub/fayez-gebali/1b/7a2/6a3" style="float:right">
                        <img id="linkedInFayez" src="Images/linkedIn.png" style="width:10%; height:10%"/>
                    </a>
                </div>
            </div>  
        </div>
    </div>
</asp:Content>
