using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion
{
    public class consultarPorId
    {
        public class Ejecuta : IRequest<UsuarioModel>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioModel>
        {
            private readonly IUsuario _usuarioRepository;
            public Manejador(IUsuario usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }

            public async Task<UsuarioModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _usuarioRepository.ObtenerPorId(request.Id);
                if (usuario == null)
                {
                    throw new Exception("No se encontro el usuario");
                }

                return usuario;
            }
        }
    }
}
