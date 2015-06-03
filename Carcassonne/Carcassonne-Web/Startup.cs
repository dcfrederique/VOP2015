using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carcassonne_Web.Startup))]
namespace Carcassonne_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            var serializer = JsonSerializer.Create(serializerSettings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer); 

            app.MapSignalR();
        }
    }
}
