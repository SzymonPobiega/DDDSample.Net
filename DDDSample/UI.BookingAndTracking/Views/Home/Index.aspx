<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Reflection"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    DDDSample.NET 0.1
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome to DDDSample.NET sample application.</h2>
    <p>
        This is a sample application for demonstrating Domain Driven Design concepts.<br/>
        It is based upon <a href="http://www.amazon.com/Domain-Driven-Design-Tackling-Complexity-Software/dp/0321125215/ref=sr_1_1?ie=UTF8&s=books&qid=1238687848&sr=8-1">Eric Evans' book</a> and <a href="http://dddsample.sourceforge.net/">Java DDDSample application</a>.        
    </p>
    <p>
        Currently, the only available functionality is cargo booking. Click the 'Booking' button in top left corner to try.
    </p>
</asp:Content>
