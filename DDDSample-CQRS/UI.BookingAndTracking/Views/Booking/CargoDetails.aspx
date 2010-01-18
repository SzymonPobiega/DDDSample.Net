<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CargoRoutingDTO>" %>

<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Facade" %>
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
            <p>
                <% if (Model.Legs.Count == 0)
                   { %>
                <label>
                    Not routed -
                    <%= Html.ActionLink("Route this cargo", "AssignToRoute", new { Model.TrackingId })%></label>
                <% } %>
                <% if (Model.Misrouted)
                   { %>
                <label>
                    Cargo is misrouted -
                    <%= Html.ActionLink("Reroute this cargo", "AssignToRoute", new { Model.TrackingId })%></label>
                <% } %>
            </p>
        </fieldset>
    </div>
    <% if (Model.Legs.Count != 0)
       { %>
    <div>
        <table>
            <caption>
                Itinerary
                <thead>
                    <tr>
                        <td>
                            Voyage
                        </td>
                        <td>
                            From
                        </td>
                        <td>
                        </td>
                        <td>
                            To
                        </td>
                        <td>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <%                   
                        foreach (LegDTO leg in Model.Legs)
                        {%>
                    <tr>
                        <td>
                            <%=leg.VoyageNumber%>
                        </td>
                        <td>
                            <%=leg.From%>
                        </td>
                        <td>
                            <%=leg.LoadTime%>
                        </td>
                        <td>
                            <%=leg.To%>
                        </td>
                        <td>
                            <%=leg.UnloadTime%>
                        </td>
                    </tr>
                    <%
                        }%>
                </tbody>
        </table>
    </div>
    <% } %>
</asp:Content>
