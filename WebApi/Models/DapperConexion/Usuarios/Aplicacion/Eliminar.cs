using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion
{
    public class eliminarUsuario
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                var resultados = await _usuarioRepositorio.Elimina(request.Id);
                if (resultados > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el usuario");
            }
        }
    }
}
