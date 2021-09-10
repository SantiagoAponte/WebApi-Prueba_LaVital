using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Email { get; set; }
            public string UserName { get; set; }
            public string PasswordHash { get; set; }
            public  bool EmailConfirmed { get; set; } = true;
            public  bool PhoneNumberConfirmed { get; set; } = true;
            public  bool TwoFactorEnabled { get; set; } = true;
            public  bool LockoutEnabled { get; set; } = true;
            public int AccessFailedCount { get; set; } = 9999;

        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IUsuario _usuarioRepository;
            public Manejador(IUsuario usuarioRepository)
            {
                _usuarioRepository = usuarioRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var resultado = await _usuarioRepository.Nuevo(request.Email, request.UserName, request.PasswordHash, request.EmailConfirmed, request.TwoFactorEnabled, request.PhoneNumberConfirmed, request.LockoutEnabled, request.AccessFailedCount);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el nuevo usuario");
            }
        }
    }
}
