<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="DDDSample.Reporting"%>

<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Facade" %>
<asp:Content ID="listCargosTitle" ContentPlaceHolderID="TitleContent" runat="server">
    List of all cargos
</asp:Content>
<asp:Content ID="listCargosContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        List of all cargos. To book a new cargo <%=Html.ActionLink("click here", "NewCargo") %></h2>
    <table border="1" width="600">        
        <thead>
            <tr>
                <td>
                    Tracking ID
                </td>
                <td>
                    Origin
                </td>
                <td>
                    Destination
                </td>
                <td>
                    Routed
                </td>
            </tr>
        </thead>
        <tbody>
            <%
                foreach (Cargo cargo in (IList<Cargo>)ViewData["cargos"])
                {
            %>
            <tr>
                <td>
                    <%=Html.ActionLink(cargo.TrackingId, "CargoDetails", new {trackingId=cargo.TrackingId}) %>
                </td>
                <td>
                    <%=cargo.Origin %>
                </td>
                <td>
                    <%=cargo.Destination %>
                </td>
                <td>
                    <%=cargo.CurrentInformation.RoutingStatus == RoutingStatus.Misrouted ? "Yes" : "No"%>
                </td>
            </tr>
            <%
                } %>
        </tbody>
    </table>
</asp:Content>
