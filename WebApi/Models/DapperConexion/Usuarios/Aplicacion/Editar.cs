using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion
{
    public class editarUsuario
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IUsuario _usuarioRepositorio;

            public Manejador(IUsuario usuarioRepositorio)
            {
                _usuarioRepositorio = usuarioRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await _usuarioRepositorio.Actualiza(request.Id, request.Email, request.Username, request.Password);
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar la data del instructor");
            }
        }
    }
}
