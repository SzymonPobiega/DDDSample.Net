<%@ Page Title="Book new cargo" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<DDDSample.Commands.BookNewCargoCommand>" %>

<%@ Import Namespace="DDDSample.UI.BookingAndTracking.Models" %>
<asp:Content ID="bookNewCargoTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Book new cargo
</asp:Content>
<asp:Content ID="bookNewCargoContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
        $("#arrivalDeadlineDatepicker").datepicker({ altField: '#ArrivalDeadline' });
        });
    </script>
    
    <h2>
        Book new cargo</h2>
    <p>
        Use the form below to book a new cargo.
    </p>
    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm())
       { %>
    <div>
        <fieldset>
            <legend>Cargo information</legend>
            <p>
                <label for="Origin">
                    Origin:</label>
                <%= Html.DropDownList("Origin")%>
            </p>
            <p>
                <label for="Destination">
                    Destination:</label>
                <%= Html.DropDownList("Destination")%>
            </p>
            <p>
                <label for="ArrivalDeadline">
                    Arrival deadline:</label>
                <%= Html.TextBox("ArrivalDeadline") %>
                <%= Html.ValidationMessage("ArrivalDeadline", "*")%>
                <div type="text" id="arrivalDeadlineDatepicker">
                </div>
            </p>
            <p>
                <input type="submit" value="Book" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
