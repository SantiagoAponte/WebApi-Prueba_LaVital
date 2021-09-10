using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.DapperConexion;
using WebApi.Models.DapperConexion.Usuarios;
using WebApi.Models.DapperConexion.Usuarios.Aplicacion;
using WebApi.Seguridad.TokenSeguridad;
using WebApi.Seguridad.TokenSeguridad.Seguridad;
using WebApi.Models.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models.DapperConexion.Usuarios.Login;

namespace WebApi
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
        //Despliegue de Cors para ser consumida por FrontEnd
            services.AddCors(o => o.AddPolicy("corsApp", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

        //Conexion a base de datos mediante Entity framework y Dapper.
            services.AddControllers();
            services.AddDbContext<PruebaLaVitalContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));
       
            //Constructor de la entidad Core de microsoft, añadiendo al usuario creado con un rol asignado y un claim.
            var builder = services.AddIdentityCore<UsuarioModel>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<UsuarioModel,IdentityRole>>();

                //Constructor de sesión de base de datos, ingreso al sistema y hora reciente de ingreso.
             identityBuilder.AddEntityFrameworkStores<PruebaLaVitalContext>();
            identityBuilder.AddSignInManager<SignInManager<UsuarioModel>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Creackón de Token para acceso al aplicativo (Guardara el Rol en el Json encriptado)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
            //Gestor de la comunicación en el middleware.
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            services.AddTransient<IFactoryConnection, FactoryConnection>();
            services.AddScoped<IUsuario, usuarioRepositorio>();

            //Servicios para mantener arriba cada vez que se requieran
            //services.AddScoped<IUsuarioSesion, UsuarioSesion>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
