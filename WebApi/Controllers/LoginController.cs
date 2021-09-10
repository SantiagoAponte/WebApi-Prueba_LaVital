using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DapperConexion.Usuarios;
using WebApi.Models.DapperConexion.Usuarios.Login;

namespace WebApi.Controllers
{
    public class LoginController : MyControllerBase
    {
        // http://localhost:5000/api/Login
        [HttpPost]
        public async Task<ActionResult<usuarioDTO>> Login(Login.Ejecuta data)
        {
            return await Mediator.Send(data);
        }
    }
}
