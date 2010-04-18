<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<AssignToRouteModel>" %>
<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Facade" %>
<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Models" %>
<asp:Content ID="changeCargoDestinationTitle" ContentPlaceHolderID="TitleContent"
    runat="server">
    Change destination of cargo
</asp:Content>
<asp:Content ID="changeCargoDestinationContent" ContentPlaceHolderID="MainContent"
    runat="server">
    <h2>
        Select route for cargo <%= Model.Cargo.TrackingId %></h2>
    <p>
        Cargo is going from
        <%= Model.Cargo.Origin %>
        to
        <%= Model.Cargo.Destination %>.
    </p>
    <div>
        <%            
            int i = 1;
            foreach (RouteCandidateDTO routeCandidate in Model.RouteCandidates)
            {
                using (Html.BeginForm())
                {%>
        <table>
            <caption>
                Route candidate
                <%=i%></caption>
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
                   int legIndex = 0;
                    foreach (LegDTO leg in routeCandidate.Legs)
                    {%>
                    <%=Html.Hidden("Legs["+legIndex+"].VoyageNumber", leg.VoyageNumber) %>
                    <%=Html.Hidden("Legs["+legIndex+"].VoyageId", leg.VoyageId) %>
                    <%=Html.Hidden("Legs["+legIndex+"].From", leg.From) %>
                    <%=Html.Hidden("Legs["+legIndex+"].LoadTime", leg.LoadTime) %>
                    <%=Html.Hidden("Legs["+legIndex+"].To", leg.To) %>
                    <%=Html.Hidden("Legs["+legIndex+"].UnloadTime", leg.UnloadTime) %>                    
                    <%
                       legIndex++; %>
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
        <input type="submit" value="Assign cargo to this route" />
        <%
            }
                    i++;
                }
        %>
    </div>
</asp:Content>
