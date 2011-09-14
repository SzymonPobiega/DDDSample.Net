using System.Web.Mvc;
using System.Web.Routing;

namespace DDDSample.UI.BookingAndTracking
{
    public class RouteRegistrar
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.MapRoute(
               "Details",
               "Booking/CargoDetails/{trackingId}",
               new { controller = "Booking", action = "CargoDetails" }
               );

            routes.MapRoute(
               "TrackingDetails",
               "Tracking/Track/{trackingId}",
               new { controller = "Tracking", action = "Track" }
               );

            routes.MapRoute(
               "Change destination",
               "Booking/ChangeDestination/{trackingId}",
               new { controller = "Booking", action = "ChangeDestination" }
               );

            routes.MapRoute(
               "Assign to route",
               "Booking/AssignToRoute/{trackingId}",
               new { controller = "Booking", action = "AssignToRoute" }
               );

            routes.MapRoute(
               "Default",
               "{controller}/{action}/{id}",
               new { controller = "Home", action = "Index", id = "" }
               );
        }
    }
}