using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades2._0;
using NecliGestion.Logica.Services;
using NecliGestion.Persistencia;
using NequiGestion.Logica.Dtos;

namespace Necli3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasUserController : ControllerBase
    {
        private readonly CuentasRepository _cuentasRepo;
        private readonly CuentaService _cuentaService;

        public CuentasUserController(CuentasRepository cuentasRepo, CuentaService cuentaService)
        {
            _cuentasRepo = cuentasRepo;
            _cuentaService = cuentaService;
        }

        [HttpPost("crear")]
        public IActionResult CrearCuenta([FromBody] Cuentas cuenta)
        {
            try
            {
                bool resultado = _cuentasRepo.CrearCuenta(cuenta);
                if (resultado)
                {
                    return Ok("Cuenta creada correctamente");
                }
                else
                {
                    return BadRequest("No se pudo crear la cuenta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la cuenta: {ex.Message}");
            }
        }

        [HttpGet("{NumeroCuenta}")]
        public async Task<IActionResult> ObtenerCuentaPorId(int NumeroCuenta)
        {
            var cuenta = await _cuentasRepo.ObtenerCuentaPorId(NumeroCuenta);
            if (cuenta != null)
            {
                return Ok(cuenta);
            }
            else
            {
                return NotFound("No se encontró la cuenta");
            }
        }

        [HttpGet("ListarTodasCuentas")]
        public async Task<IActionResult> ObtenerTodasLasCuentas()
        {
            var cuentas = await _cuentasRepo.ObtenerTodasLasCuentas();
            return Ok(cuentas);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarCuenta([FromBody] EliminarCuentaDto eliminarCuentaDto)
        {
            try
            {
                bool resultado = await _cuentaService.EliminarCuenta(eliminarCuentaDto);

                if (!resultado)
                {
                    return NotFound(new { mensaje = "Cuenta no encontrada." });
                }

                return Ok(new { mensaje = "Cuenta eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
