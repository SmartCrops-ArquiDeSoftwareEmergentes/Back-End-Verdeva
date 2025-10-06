using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using _1_API.Response;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutriControl.Domain.IAM.Models.Comands;
using NutriControl.Domain.IAM.Queries;
using NutriControl.Domain.IAM.Services;
using NutriControl.Presentation.Filters;

namespace NutriControl.Presentation.IAM.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserCommandService _userCommandService;
        private readonly IUserQueryService _userQueryService;

        public UserController(IUserCommandService userCommandService, IUserQueryService userQueryService, IMapper mapper)
        {
            _userCommandService = userCommandService;
            _userQueryService = userQueryService;
            _mapper = mapper;
        }

        // GET: api/v1/User/getall
        /// <summary>Obtiene todos los usuarios activos.</summary>
        /// <remarks>
        /// GET /api/v1/User/getall
        /// </remarks>
        /// <response code="200">Devuelve todos los usuarios.</response>
        /// <response code="404">Si no hay usuarios.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [CustomAuthorize("Farmer")]
        [Route("getall")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _userQueryService.Handle(new GetUserAllQuery());

            if (result.Count == 0) return NotFound();

            return Ok(result);
        }

        // POST: api/v1/User/register
        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/v1/User/register
        ///     {
        ///         "username": "Jorge Ruiz",
        ///         "dni": "28089824",
        ///         "ruc": "35355800618",
        ///         "emailAddress": "user@example.com",
        ///         "phone": "809324921",
        ///         "role": "Farmer - Admin",
        ///         "password": "Securepassword123!",
        ///         "confirmPassword": "Securepassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="command">Datos del usuario a crear.</param>
        /// <returns>ID del usuario recién creado.</returns>
        /// <response code="201">Devuelve el ID del usuario creado.</response>
        /// <response code="400">Si el usuario tiene propiedades inválidas.</response>
        /// <response code="409">Error validando los datos.</response>
        /// <response code="500">Error inesperado.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] SingupCommand command)
        {
            var retun = await _userCommandService.Handle(command);
            return CreatedAtAction("register", new { id = retun });
        }

        // POST: api/v1/User/login
        /// <summary>
        /// Inicia sesión de usuario.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/v1/User/login
        ///     {
        ///         "username": "Jorge Ruiz",
        ///         "password": "Securepassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="command">Credenciales del usuario.</param>
        /// <returns>Token JWT generado.</returns>
        /// <response code="201">Devuelve el token JWT.</response>
        /// <response code="400">Si las credenciales son inválidas.</response>
        /// <response code="409">Error validando los datos.</response>
        /// <response code="500">Error inesperado.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] SigninCommand command)
        {
            var retun = await _userCommandService.Handle(command);
            return CreatedAtAction("login", new { jwt = retun });
        }

        // PUT: api/v1/User/{id}
        /// <summary>
        /// Actualiza un usuario existente por su ID.
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     PUT /api/v1/User/5
        ///     {
        ///         "username": "Usuario Actualizado",
        ///         "DniOrRuc": "12345678",
        ///         "emailAddress": "nuevoemail@example.com",
        ///         "phone": "809123456"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID del usuario a actualizar.</param>
        /// <param name="command">Datos actualizados del usuario.</param>
        /// <response code="200">Usuario actualizado correctamente.</response>
        /// <response code="400">Si el usuario tiene propiedades inválidas.</response>
        /// <response code="404">Si el usuario no se encuentra.</response>
        /// <response code="500">Error inesperado.</response>
        [HttpPut("{id}")]
        [CustomAuthorize("Farmer")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateUserCommand command)
        {
            command.Id = id;
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);

            var result = await _userCommandService.Handle(command);

            if (!result) return NotFound();

            return Ok();
        }

        // DELETE: api/v1/User/{id}
        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <remarks>
        /// DELETE /api/v1/User/{id}
        /// </remarks>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <response code="200">Usuario eliminado correctamente.</response>
        /// <response code="404">Si el usuario no se encuentra.</response>
        /// <response code="500">Error inesperado.</response>
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        [HttpDelete("{id}")]
        [CustomAuthorize("Farmer")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            DeleteUserCommand command = new DeleteUserCommand { Id = id };
            var result = await _userCommandService.Handle(command);
            return Ok();
        }
        
        
        
        
    }
}