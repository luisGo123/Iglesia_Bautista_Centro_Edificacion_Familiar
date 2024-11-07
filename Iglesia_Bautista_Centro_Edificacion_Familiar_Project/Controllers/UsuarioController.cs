using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")] 
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public UsuarioController(dbIglesia_Bautista_Centro_FamiliarContext _context)
        {
            _dbcontext = _context;

        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var Usuarios = _dbcontext.Usuarios.Select(r => new

                {
                    r.IdUsuario,
                    r.NombreCompleto,
                    r.ApellidoCompleto,
                    r.Correo,
                    r.Contrasena,
                    r.Cedula,
                    r.Telefono,
                    r.IdPerfil,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = " La Petición realizada  fue exitosamente", response = Usuarios });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpGet]
        [Route("Buscar")]
        public IActionResult Buscar(string? cedula, string? correo, string? nombreCompleto)
        {
            try
            {
                var query = _dbcontext.Usuarios.AsQueryable();

                if (!string.IsNullOrEmpty(cedula))
                {
                    query = query.Where(u => u.Cedula.Contains(cedula));
                }

                if (!string.IsNullOrEmpty(correo))
                {
                    query = query.Where(u => u.Correo.Contains(correo));
                }

                if (!string.IsNullOrEmpty(nombreCompleto))
                {
                    query = query.Where(u => u.NombreCompleto.Contains(nombreCompleto));
                }

                var usuarios = query.Select(r => new
                {
                    r.IdUsuario,
                    r.NombreCompleto,
                    r.ApellidoCompleto,
                    r.Correo,
                    r.Contrasena,
                    r.Cedula,
                    r.Telefono,
                    r.IdPerfil
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Búsqueda realizada con éxito", response = usuarios });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdUsuario:int}")]
        public IActionResult Obtener(int IdUsuario)
        {
            Usuario usuarios = _dbcontext.Usuarios.Find(IdUsuario);

            if (usuarios == null)
            {
                return BadRequest(" lo siento El usuario  no existe");

            }

            try
            {

                var usuariosCreacion = _dbcontext.Usuarios.Select(r => new
                {
                    r.IdUsuario,
                    r.NombreCompleto,
                    r.ApellidoCompleto,
                    r.Correo,
                    r.Contrasena,
                    r.Cedula,
                    r.Telefono,
                    r.IdPerfil,

                }).ToList();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Petición realizada exitosamente", response = usuarios });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Usuario objeto)
        {
            try
            {
                // Verificar si ya existe un usuario con la misma cédula
                var existeUsuario = _dbcontext.Usuarios
                    .Any(u => u.Cedula == objeto.Cedula);

                if (existeUsuario)
                {
                    return StatusCode(409, new { mensaje = "Ya existe un usuario con esta cédula." }); // 409 Conflict
                }

                var creacionUsuario = new Usuario
                {
                    NombreCompleto = objeto.NombreCompleto,
                    ApellidoCompleto = objeto.ApellidoCompleto,
                    Correo = objeto.Correo,
                    Contrasena = objeto.Contrasena,
                    Cedula = objeto.Cedula,
                    Telefono = objeto.Telefono,
                    IdPerfil = objeto.IdPerfil
                };

                _dbcontext.Usuarios.Add(creacionUsuario);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { savedRole = creacionUsuario, mensaje = "Su usuario se ha creado y guardado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Usuario usuario)
        {
            try
            {
                // Verificar si el usuario existe con el correo y la contraseña
                var usuarioEncontrado = _dbcontext.Usuarios
                    .FirstOrDefault(u => u.Correo == usuario.Correo && u.Contrasena == usuario.Contrasena);

                if (usuarioEncontrado == null)
                {
                    return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." }); // 401 Unauthorized
                }

                // Devolver toda la información del usuario si coincide
                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso",
                    usuario = new
                    {
                        usuarioEncontrado.IdUsuario,
                        usuarioEncontrado.NombreCompleto,
                        usuarioEncontrado.ApellidoCompleto,
                        usuarioEncontrado.Correo,
                        usuarioEncontrado.Contrasena, // Incluir la contraseña
                        usuarioEncontrado.Cedula,
                        usuarioEncontrado.Telefono,
                        usuarioEncontrado.IdPerfil
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }






        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Usuario objeto)
        {
            Usuario usuario = _dbcontext.Usuarios.Find(objeto.IdUsuario);

            if (usuario == null)
            {
                return BadRequest("Lo siento, su usuario no ha sido encontrado.");
            }


            var existeUsuario = _dbcontext.Usuarios
                .Any(u => u.Cedula == objeto.Cedula && u.IdUsuario != objeto.IdUsuario);

            if (existeUsuario)
            {
                return StatusCode(409, new { mensaje = "Ya existe otro usuario con esta cédula." }); // 409 Conflict
            }

            try
            {
                usuario.NombreCompleto = objeto.NombreCompleto;
                usuario.ApellidoCompleto = objeto.ApellidoCompleto;
                usuario.Correo = objeto.Correo;
                usuario.Contrasena = objeto.Contrasena;
                usuario.Cedula = objeto.Cedula;
                usuario.Telefono = objeto.Telefono;
                usuario.IdPerfil = objeto.IdPerfil;

                _dbcontext.Usuarios.Update(usuario);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su usuario se actualizó correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{idUsuario:int}")]
        public IActionResult Eliminar(int idUsuario)
        {
            Usuario usuario = _dbcontext.Usuarios.Find(idUsuario);

            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            // Verificar si el usuario está asociado con algún almacén
            var usuarioEnAlmacen = _dbcontext.Almacenes.Any(a => a.IdUsuario == idUsuario);

            if (usuarioEnAlmacen)
            {
                return StatusCode(409, new { mensaje = "No se puede eliminar el usuario porque está asociado a un almacén." }); // 409 Conflict
            }

            try
            {
                _dbcontext.Usuarios.Remove(usuario);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Su usuario se eliminó exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}