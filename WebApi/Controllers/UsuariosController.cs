using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.DapperConexion.Usuarios;
using WebApi.Models.DapperConexion.Usuarios.Aplicacion;
using WebApi.Models.DapperConexion.Usuarios.Login;
using static WebApi.Controllers.MyControllerBase;

namespace WebApi.Controllers
{
    [ApiController]
    public class UsuariosController : MyControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator)
        {
            _mediator = mediator;
        }
        //public IActionResult Index()
        //{
        //  return View();
        //}

        //    [Authorize(Roles = "Admin")]
        
        
       [HttpGet]
       public async Task<ActionResult<List<UsuarioModel>>> GetResultAsync(){
           return await _mediator.Send(new Consulta.Lista());
       }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data) {
            return await _mediator.Send(data);
        }

        [HttpPost("registrar")] [AllowAnonymous]
        public async Task<ActionResult<usuarioDTO>> Registrar(Registrar.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, editarUsuario.Ejecuta data){
            data.Id = id;
            return await _mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id){
            return await _mediator.Send(new eliminarUsuario.Ejecuta{Id = id});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> ObtenerPorId(Guid id){
            return await _mediator.Send(new consultarPorId.Ejecuta{Id = id});

     }
    }
}
