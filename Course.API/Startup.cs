using CourseAPI.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CourseAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Disable recursive cycle error
            services.AddControllers().AddJsonOptions(o => 
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            // Add XML format
            services.AddMvc().AddXmlSerializerFormatters();

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // ajout de l'authentification
            .AddJwtBearer(options =>
            {
                /* options.SaveToken = true;
                si SaveToken est activé, ASP.NET Core stocke automatiquement le jeton d’accès et d’actualisation résultant dans la session d’authentification ainsi vous pouvez récupérer des données via 
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                 ‎*/


                // modèle https requis
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters() //Contient un ensemble de paramètres utilisés par un SecurityTokenHandler lors de la validation d’un SecurityToken.
                {
                    ValidateIssuer = true, //permet de connaitre l’auteur du jeton en lui affectant une Url, un nom de serveur, d’application ou tout autres informations permettant d’en connaitre l’origine.
                    ValidateAudience = true, /*permet de connaitre le type d’audience ciblée par exemple le type de client comme par exemple mobile ou web ou la nature de l’application qui peut être en développement ou en production. L’interprétation de l’audience est propre à l’application.*/
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    ValidateLifetime = true, //valider les valeurs d’expiration
                    ClockSkew = TimeSpan.Zero //pas de tolérance pour la date d’expiration
                };
            });
            services.AddControllers();
            services.AddDbContext<CourseDbContext>(op => op.UseSqlServer(Configuration.GetConnectionString("CourseAPIDbContext")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Add static files (image)
                app.UseStaticFiles();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
