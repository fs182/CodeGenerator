using CodeGenerator.API.Models;
using CodeGenerator.Application.Handlers.Table;
using CodeGenerator.Application.Interfaces.General;
using CodeGenerator.Infrastructure.Context;
using CodeGenerator.Infrastructure.Repositories.General;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace CodeGenerator.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy(name: "HeritageTreesCors", builder =>
                {
                    builder.AllowAnyOrigin();
                })
            );
            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("HeritageTreesDB");
            //IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false).Build();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<GeneralContext>(f => f.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Audience"],
                    ValidIssuer = Configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context => {

                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        var result = JsonConvert.SerializeObject(new
                        {
                            status = "un-authorized",
                            message = "un-authorized"
                        });

                        return context.Response.WriteAsync(result);
                    },

                    OnForbidden = context => {

                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        var result = JsonConvert.SerializeObject(new
                        {
                            status = "un-authorized",
                            message = "un-authorized"
                        });

                        return context.Response.WriteAsync(result);
                    }
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HeritageTrees.API", Version = "2.0" });
                options.CustomSchemaIds(c => c.FullName);
                options.EnableAnnotations();

            });
            services.AddMediatR( typeof(PopulateHandler).GetTypeInfo().Assembly);
            services.AddTransient<IGeneralRepository, GeneralRepository>();
            
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("./swagger/v1/swagger.json", "HeritageTrees.API");
                options.InjectStylesheet("./swagger-ui/custom.css");
                options.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
