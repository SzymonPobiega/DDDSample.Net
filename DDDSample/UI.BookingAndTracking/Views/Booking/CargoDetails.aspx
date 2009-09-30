<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UI.BookingAndTracking.Facade.CargoRoutingDTO>" %>

<asp:Content ID="cargoDetailsTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Details for cargo
    <%= Model.TrackingId %>
</asp:Content>
<asp:Content ID="catgoDetailsContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Details for cargo
        <%= Model.TrackingId %></h2>
    <div>
        <fieldset>
            <legend>Cargo information</legend>
            <p>
                <label>
                    Origin:</label>
                <%= Model.Origin %>
            </p>
            <p>
                <label>
                    Destination:</label>
                <%= Model.Destination %>
                <br />
                <%= Html.ActionLink("Change destination", "ChangeDestination") %>
            </p>
            <p>
                <label>
                    Arrival deadline:</label>
                <%= Model.ArrivalDeadline %>
            </p>
        </fieldset>
    </div>
</asp:Content>