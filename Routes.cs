using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Devq.Conversations
{
    public class Routes : IRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes() {

            return new[] {
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "Conversations",
                        new RouteValueDictionary {
                            {"area", "Devq.Conversations"},
                            {"controller", "Conversation"},
                            {"action", "Index"},
                            {"id", UrlParameter.Optional}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Devq.Conversations"}
                        },
                        new MvcRouteHandler()
                    )
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var route in GetRoutes())
                routes.Add(route);
        }
    }
}