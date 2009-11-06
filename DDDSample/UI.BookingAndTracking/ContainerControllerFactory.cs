using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;

namespace DDDSample.UI.BookingAndTracking
{
   /// <summary>
   /// A controller factory based on abient DI container.
   /// </summary>
   public class ContainerControllerFactory : DefaultControllerFactory
   {
      protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
      {
         return (IController)ServiceLocator.Current.GetInstance(controllerType);
      }
   }
}
