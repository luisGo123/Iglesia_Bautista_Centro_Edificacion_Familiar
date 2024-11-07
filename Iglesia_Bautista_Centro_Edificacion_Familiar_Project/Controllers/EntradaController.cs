using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO;
using Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("PermitirTodo")]
    [ApiController]
    public class EntradaController : ControllerBase
    {

        private readonly dbIglesia_Bautista_Centro_FamiliarContext _dbcontext;

        public EntradaController(dbIglesia_Bautista_Centro_FamiliarContext context)
        {
            _dbcontext = context;
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> CrearEntrada([FromBody] Entrada nuevaEntrada)
        {
            if (nuevaEntrada == null || nuevaEntrada.Cantidad == null || nuevaEntrada.IdProducto == null)
            {
                return BadRequest("Datos incompletos.");
            }

            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                // Agregar la nueva entrada al contexto
                _dbcontext.Entrada.Add(nuevaEntrada);
                await _dbcontext.SaveChangesAsync();

                // Buscar el producto correspondiente por IdProducto
                var producto = await _dbcontext.Productos.FindAsync(nuevaEntrada.IdProducto);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado.");
                }

                // Actualizar la existencia del producto sumando la cantidad de la entrada
                producto.Existencia = (producto.Existencia ?? 0) + nuevaEntrada.Cantidad;

                // Guardar los cambios en el producto
                _dbcontext.Productos.Update(producto);
                await _dbcontext.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                return Ok(nuevaEntrada);
            }
            catch
            {
                // Si ocurre un error, revierte la transacción
                await transaction.RollbackAsync();
                return StatusCode(500, "Ocurrió un error al crear la entrada.");
            }
        }


        [HttpGet("Listar")]
        public async Task<IActionResult> ListarEntradas()
        {
            try
            {
                var entradas = await _dbcontext.Entrada
                    .Include(e => e.IdProductoNavigation) // Asegúrate de que la relación esté configurada
                        .ThenInclude(p => p.IdCategoriaNavigation) // Relación con Categoría
                    .Include(e => e.IdProductoNavigation)
                        .ThenInclude(p => p.IdTipoProductoNavigation) // Relación con Tipo de Producto
                    .Include(e => e.IdProductoNavigation)
                        .ThenInclude(p => p.IdMarcaNavigation) // Relación con Marca
                    .Select(e => new EntradaDTO
                    {
                        IdEntrada = e.IdEntrada, // Asegúrate de que el nombre de la propiedad es correcto
                        IdProducto = e.IdProducto,
                        Cantidad = e.Cantidad,
                        NombreProducto = e.IdProductoNavigation.NombreProducto, // Propiedad de Producto
                        Color = e.IdProductoNavigation.Color, // Propiedad de Producto
                        Existencia = e.IdProductoNavigation.Existencia,
                        Categoria = e.IdProductoNavigation.IdCategoriaNavigation.Nombre, // Asumiendo que 'Nombre' es el campo en Categoria
                        TipoProducto = e.IdProductoNavigation.IdTipoProductoNavigation.Nombre, // Asumiendo que 'Nombre' es el campo en TipoProducto
                        Marca = e.IdProductoNavigation.IdMarcaNavigation.Nombre // Asumiendo que 'Nombre' es el campo en Marca
                    })
                    .ToListAsync();

                return Ok(entradas);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error al obtener la lista de entradas.");
            }
        }

        [HttpDelete("Eliminar/{idEntrada}")]
        public async Task<IActionResult> EliminarEntrada(int idEntrada)
        {
            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                // Buscar la entrada por id
                var entrada = await _dbcontext.Entrada
                    .Include(e => e.IdProductoNavigation)
                    .FirstOrDefaultAsync(e => e.IdEntrada == idEntrada);

                if (entrada == null)
                {
                    return NotFound("La entrada no fue encontrada.");
                }

                // Obtener el producto y restablecer la cantidad eliminada
                var producto = entrada.IdProductoNavigation;
                if (producto != null)
                {
                    producto.Existencia -= entrada.Cantidad ?? 0; // Restar la cantidad de la existencia
                    _dbcontext.Productos.Update(producto);
                }

                // Eliminar la entrada
                _dbcontext.Entrada.Remove(entrada);
                await _dbcontext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok($"Entrada con ID {idEntrada} eliminada correctamente y la existencia del producto ha sido actualizada.");
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Ocurrió un error al eliminar la entrada.");
            }
        }


    }
}
