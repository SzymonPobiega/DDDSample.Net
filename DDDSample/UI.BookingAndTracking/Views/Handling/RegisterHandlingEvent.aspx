<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Models" %>
<asp:Content ID="registerHandlingEventTitle" ContentPlaceHolderID="TitleContent"
    runat="server">
    Book new cargo
</asp:Content>
<asp:Content ID="registerHandlingEventContent" ContentPlaceHolderID="MainContent"
    runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
        $("#completionTimeDatepicker").datepicker({ altField: '#completionTime' });
        });
    </script>

    <h2>
        Register new handling event</h2>
    <p>
        Use the form below to register a new cargo handling event.
    </p>
    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm())
       { %>
    <div>
        <fieldset>
            <legend>Event information</legend>
            <p>
                <label for="trackingId">
                    Cargo tracking id:</label>
                <%= Html.TextBox("trackingId") %>
                <%= Html.ValidationMessage("trackingId", "*")%>
            </p>
            <p>
                <label for="completionTime">
                    Completion time:</label>
                <%= Html.TextBox("completionTime")%>
                <%= Html.ValidationMessage("completionTime", "*")%>
                <div type="text" id="completionTimeDatepicker">
                </div>
            </p>
            <p>
                <label for="destination">
                    Location:</label>
                <%= Html.DropDownList("location")%>
            </p>
            <p>
                <label for="type">
                    Event type:</label>
                <%= Html.DropDownList("type")%>
            </p>
            <p>
                <input type="submit" value="Register" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
