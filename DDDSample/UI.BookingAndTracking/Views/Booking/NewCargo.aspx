<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Booking>" %>
<%@ Import Namespace="UI.BookingAndTracking.Models"%>

<asp:Content ID="bookNewCargoTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Book new cargo
</asp:Content>

<asp:Content ID="bookNewCargoContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Book new cargo</h2>
    <p>
        Use the form below to book a new cargo. 
    </p>
    
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Cargo information</legend>
                <p>
                    <label for="origin">Origin:</label>
                    <%= Html.DropDownList("origin", Model.Locations) %>                    
                </p>
                <p>
                    <label for="destination">Destination:</label>
                    <%= Html.DropDownList("destination", Model.Locations) %>                    
                </p>
                <p>
                    <label for="arrivalDeadline">Arrival deadline:</label>
                    <%= Html.TextBox("arrivalDeadline") %>
                    <%= Html.ValidationMessage("arrivalDeadline")%>
                </p>
                <p>
                    <input type="submit" value="Book" />
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
