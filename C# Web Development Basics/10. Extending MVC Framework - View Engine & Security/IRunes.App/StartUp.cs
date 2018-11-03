using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.Framework.Api;
using SIS.Framework.Services.Contracts;

namespace IRunes.App
{
    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<IAlbumService, AlbumService>();
            dependencyContainer.RegisterDependency<ITrackService, TrackService>();
        }
    }
}
