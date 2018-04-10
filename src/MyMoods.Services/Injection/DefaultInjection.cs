using Hangfire;
using Hangfire.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyMoods.Shared.Contracts;
using System.Linq;

namespace MyMoods.Services.Injection
{
    public static class DefaultInjection
    {
        public static void Inject(IServiceCollection services, IConfigurationRoot config)
        {
            var appStorage = new
            {
                Connection = config.GetConnectionString("DefaultConnection"),
                DatabaseName = config.GetConnectionString("DefaultConnection").Split('/').Last()
            };

            services.AddScoped(x => Shared.Mongo.Database.Get(appStorage.Connection));
            services.AddScoped<IStorage, Shared.Mongo.Storage>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<IFormsService, FormsService>();
            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IMoodsService, MoodsService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<ITagsService, TagsService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddSingleton(config);

            var hangfireStorage = new
            {
                Connection = config.GetConnectionString("HangfireConnection"),
                DatabaseName = config.GetConnectionString("HangfireConnection").Split('/').Last()
            };

            services.AddHangfire(x => x.UseMongoStorage(
                hangfireStorage.Connection,
                hangfireStorage.DatabaseName,
                new MongoStorageOptions()
                {
                    MigrationOptions = new MongoMigrationOptions()
                    {
                        Strategy = MongoMigrationStrategy.Migrate
                    }
                })
            );
        }
    }
}
