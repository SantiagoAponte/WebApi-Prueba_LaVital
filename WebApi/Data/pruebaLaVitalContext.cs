using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DapperConexion.Usuarios;

namespace WebApi.Data
{
    public class PruebaLaVitalContext : IdentityDbContext<UsuarioModel>
    {
        public PruebaLaVitalContext(DbContextOptions options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
        }
         //Set<UserModel> UserModel {get;set;}
    }
}
