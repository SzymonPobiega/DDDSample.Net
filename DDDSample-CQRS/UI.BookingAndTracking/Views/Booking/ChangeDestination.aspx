<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DDDSample.Reporting.Cargo>" %>
<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Models"%>

<asp:Content ID="changeCargoDestinationTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Change destination of cargo
</asp:Content>

<asp:Content ID="changeCargoDestinationContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change destination for cargo <%= Model.TrackingId %></h2>
    <p>
        Use the form below to change destination of a cargo.
    </p>
    
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Cargo information</legend>
                <p>
                    <label for="origin">Current destination:</label>
                    <%= Model.Destination %>                    
                </p>
                <p>
                    <label for="destination">New destination:</label>
                    <%= Html.DropDownList("destination")%>                    
                </p>                
                <p>
                    <input type="submit" value="Change destination" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
