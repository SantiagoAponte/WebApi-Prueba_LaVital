using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Roles
{
    public class asignarRol
    {
        public class Ejecuta : IRequest
        {
            public string Email { get; set; }
            public string RolName { get; set; }
        }
        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Email).NotEmpty().WithMessage("El campo no debe estar vacio");
                RuleFor(x => x.RolName).NotEmpty().WithMessage("El campo no debe estar vacio");
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<UsuarioModel> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<UsuarioModel> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var role = await _roleManager.FindByNameAsync(request.RolName);
                if (role == null)
                {
                    throw new Exception("El rol no existe");
                }
                var userIden = await _userManager.FindByEmailAsync(request.Email);
                if (userIden == null)
                {
                    throw new Exception("El usuario no existe");
                }
                var result = await _userManager.AddToRoleAsync(userIden, request.RolName);
                if (result.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo agregar el Rol al usuario");
            }
        }
    }
}