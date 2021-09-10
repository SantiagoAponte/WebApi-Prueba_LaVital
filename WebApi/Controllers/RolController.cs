using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DapperConexion.Usuarios;
using WebApi.Models.DapperConexion.Usuarios.Roles;

namespace WebApi.Controllers
{
    public class RolController : MyControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UsuarioModel> _userManager;
        public RolController(UserManager<UsuarioModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // [Authorize]
        [HttpPost("crearRol")]
        public async Task<ActionResult<Unit>> CrearRol(nuevoRol.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpPost("asignarRol")]
        public async Task<ActionResult<Unit>> asignarUserRol(asignarRol.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

    }
}
