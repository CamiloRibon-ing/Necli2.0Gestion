using Microsoft.AspNetCore.Mvc;
using NecliGestion.Entidades2._0;

using System.Threading.Tasks;



namespace Necli2._0.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly CuentasRepository _cuentasRepo;

        public CuentaController(CuentasRepository cuentasRepo)
        {
            _cuentasRepo = cuentasRepo;
        }

        [HttpPost("crear")]
        public IActionResult CrearCuenta([FromBody] Cuentas cuenta)
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

        [HttpGet("{idCuenta}")]
        public async Task<IActionResult> ObtenerCuentaPorId(int idCuenta)
        {
            var cuenta = await _cuentasRepo.ObtenerCuentaPorId(idCuenta);
            if (cuenta != null)
            {
                return Ok(cuenta);
            }
            else
            {
                return NotFound("No se encontró la cuenta");
            }
        }

        [HttpGet("todas")]
        public async Task<IActionResult> ObtenerTodasLasCuentas()
        {
            var cuentas = await _cuentasRepo.ObtenerTodasLasCuentas();
            return Ok(cuentas);
        }

        [HttpPut("actualizar/{idCuenta}")]
        public async Task<IActionResult> ActualizarCuenta(int idCuenta, [FromBody] decimal nuevoSaldo)
        {
            bool resultado = await _cuentasRepo.ActualizarCuenta(idCuenta, nuevoSaldo);
            if (resultado)
            {
                return Ok("Cuenta actualizada correctamente");
            }
            else
            {
                return BadRequest("No se pudo actualizar la cuenta");
            }
        }
    }
}
