using NecliGestion.Entidades2._0;

using NecliGestion.Persistencia;
using NequiGestion.Logica.Dtos;
using System;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services
{
    public class CuentaService
    {
        private readonly CuentasRepository _cuentaRepo;

        public CuentaService(CuentasRepository cuentaRepo)
        {
            _cuentaRepo = cuentaRepo;
        }

        public async Task<RegistroCuentaDto> CrearCuenta(RegistroCuentaDto nuevaCuentaDto)
        {
            var nuevaCuenta = new Cuentas
            {
                NumeroCuenta = new Random().Next(1000, 9999), // Genera un ID aleatorio
                IdUsuario = nuevaCuentaDto.IdUsuario,
                Saldo = nuevaCuentaDto.Saldo,
                FechaCreacion = DateTime.Now
            };

            bool resultado = await Task.Run(() => _cuentaRepo.CrearCuenta(nuevaCuenta));

            if (!resultado)
            {
                throw new Exception("Error al crear la cuenta en la base de datos.");
            }

            return new RegistroCuentaDto
            {
                NumeroCuenta = nuevaCuenta.NumeroCuenta,
                IdUsuario = nuevaCuenta.IdUsuario,
                Saldo = nuevaCuenta.Saldo
            };
        }

        public async Task<List<ConsultarCuentaDto>> ObtenerTodasLasCuentas()
        {
            var cuentas = await _cuentaRepo.ObtenerTodasLasCuentas();

            return cuentas.Select(cuenta => new ConsultarCuentaDto
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo,
                FechaCreacion = cuenta.FechaCreacion ?? DateTime.MinValue
            }).ToList();
        }

        public async Task<ConsultarCuentaDto> ObtenerCuentaPorId(int numeroCuenta)
        {
            var cuenta = await _cuentaRepo.ObtenerCuentaPorId(numeroCuenta);

            if (cuenta != null)
            {
                return new ConsultarCuentaDto
                {
                    NumeroCuenta = cuenta.NumeroCuenta,
                    Saldo = cuenta.Saldo,
                    FechaCreacion = cuenta.FechaCreacion ?? DateTime.MinValue
                };
            }

            return null;
        }

        public async Task<bool> EliminarCuenta(EliminarCuentaDto eliminarCuentaDto)
        {
            if (!int.TryParse(eliminarCuentaDto.NumeroCuenta, out int numeroCuenta))
            {
                throw new Exception("Número de cuenta inválido");
            }

            var cuenta = await _cuentaRepo.ObtenerCuentaPorId(numeroCuenta);

            if (cuenta == null)
            {
                throw new Exception("Cuenta no Encontrada");
            }
            if (cuenta.Saldo > 50000)
            {
                throw new Exception("No se puede eliminar la cuenta porque tiene saldo pendiente de $50.000");
            }
            return await _cuentaRepo.EliminarCuenta(numeroCuenta);
        }

    }
}