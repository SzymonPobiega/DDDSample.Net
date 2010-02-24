<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<AssignToRouteModel>" %>
<%@ Import Namespace="DDDSample.Messages"%>
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
                    <%=Html.Hidden("Legs[" + legIndex + "].LoadLocation", leg.LoadLocation)%>
                    <%=Html.Hidden("Legs[" + legIndex + "].LoadDate", leg.LoadDate)%>
                    <%=Html.Hidden("Legs[" + legIndex + "].UnloadLocation", leg.UnloadLocation)%>
                    <%=Html.Hidden("Legs[" + legIndex + "].UnloadDate", leg.UnloadDate)%>                    
                    <%
                       legIndex++; %>
                <tr>
                    <td>
                        XXX
                    </td>
                    <td>
                        <%=leg.LoadLocation%>
                    </td>
                    <td>
                        <%=leg.LoadDate%>
                    </td>
                    <td>
                        <%=leg.UnloadLocation%>
                    </td>
                    <td>
                        <%=leg.UnloadDate%>
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
