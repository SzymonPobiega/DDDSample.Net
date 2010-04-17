<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CargoTrackingViewAdapter>" %>
<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Models"%>

<asp:Content ID="trackCargoTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Track your cargo
</asp:Content>
<asp:Content ID="trackCargoContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm())
       { %>
    <div id="TrackingId">
        <span>
            <label for="trackingId">
                Enter your tracking id:</label>
        </span><span>
            <%= Html.TextBox("trackingId") %>
            <%= Html.ValidationMessage("trackingId", "*")%>
        </span><span>        
            <input type="submit" value="Track!" />
        </span>
    </div>
    <% } %>
    <% if (Model != null) { %>
    <div id="TrackingDetails">
    <h2><%=Model.StatusText %></h2>
    <p>Estimated time of arrival in <%=Model.Destination %>: <%=Model.Eta %></p>
    <p><%=Model.NextExpectedActivity %></p>
        
      <h3>Handling History</h3>
        <ul style="list-style-type: none;">
            <% foreach (HandlingEventViewAdapter @event in Model.HandlingEvents)
               { %>
            <li>
                <p>
                <% if (@event.IsExpected)
                   {%>
                   <img style="vertical-align: top;" src="../../Content/images/tick.png" alt=""/>
                <%}
                   else
                   {%>
                   <img style="vertical-align: top;" src="../../Content/images/error.png" alt=""/>
                <%} %>                
                &nbsp;<%=@event.Description%></p>
            </li>            
            <% } %>
        </ul>
        <p><%=Html.ActionLink("Register new handling event", "RegisterHandlingEvent", "Handling", new { trackingId = Model.TrackingId}, null)%></p>         
    </div>
    <% } %>
</asp:Content>
