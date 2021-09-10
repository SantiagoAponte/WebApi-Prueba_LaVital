using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion
{
    public class Consulta
    {
        public class Lista : IRequest<List<UsuarioModel>> { }

        public class Manejador : IRequestHandler<Lista, List<UsuarioModel>>
        {
            private readonly IUsuario _usuarioRepository;
            public Manejador(IUsuario usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }
            public async Task<List<UsuarioModel>> Handle(Lista request, CancellationToken cancellationToken)
            {
                var resultado = await _usuarioRepository.ObtenerLista();
                return resultado.ToList();

            }
        }
    }
}
