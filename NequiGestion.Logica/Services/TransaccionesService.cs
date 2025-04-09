using NequiGestion.Logica.Dtos;
using NequiGestion.Persistencia;
//using NequiGestion.Entidades2._0; // ✅ Asegura importar la entidad
using System;
using System.Threading.Tasks;
using NecliGestion.Entidades2._0;
using NecliGestion.Persistencia;

namespace NequiGestion.Logica.Services
{
    public class TransaccionesService
    {
        private readonly TransaccionesRepository _transaccionesRepo;
        private readonly CuentasRepository _cuentasRepo; // Agregar esta línea
        public TransaccionesService(TransaccionesRepository transaccionesRepo, CuentasRepository cuentasRepo)
        {
            _transaccionesRepo = transaccionesRepo;
            _cuentasRepo = cuentasRepo; // Agregar esta línea

        }

        public async Task<RegistrarTransaccionDto> RegistrarTransaccion(RegistrarTransaccionDto transaccionDto)
        {
            if (transaccionDto.Monto < 1000 || transaccionDto.Monto > 5000000)
            {
                throw new Exception("El monto debe estar entre $1000 COP y $5.000.000");
            }
            var cuentaOrigen = await _cuentasRepo.ObtenerCuentaPorId(transaccionDto.NumeroCuentaOrigen);
            var CuentaDestino = await _cuentasRepo.ObtenerCuentaPorId(transaccionDto.NumeroCuentaDestino);


            if (cuentaOrigen == null)
            {
                throw new Exception("La cuenta de origen no existe");
            }

            if (CuentaDestino == null)
            {
                throw new Exception("La cuenta de destino no existe");

            }
            if (cuentaOrigen.Saldo < transaccionDto.Monto)
            {
                throw new Exception("Saldo insuficiente en la cuenta de origen");
            }

            var transaccion = new Transacciones
            {
                NumeroTransaccion = Guid.NewGuid(), // ✅ Genera un número aleatorio
                NumeroCuentaOrigen = transaccionDto.NumeroCuentaOrigen,
                NumeroCuentaDestino = transaccionDto.NumeroCuentaDestino,
                Monto = transaccionDto.Monto,
                Fecha = DateTime.Now,

                Tipo = "Salida"
            };

            await _transaccionesRepo.RegistrarTransaccion(transaccion);
            return transaccionDto;
        }
        public async Task<ConsultarTransaccionDto> ConsultarTransaccion(Guid numeroTransaccion, ConsultarTransaccionDto transaccion)
        {
            var transaccionDto = await _transaccionesRepo.ObtenerTransaccionPorId(numeroTransaccion);
            if (transaccionDto == null)
            {
                throw new Exception("No se encontró la transacción");
            }

            return transaccion;
        }

        public async Task<ConsultarTransaccionDto> ConsultarTransaccion(Guid numeroTransaccion)
        {
            var transaccion = await _transaccionesRepo.ObtenerTransaccionPorId(numeroTransaccion);
            if (transaccion == null)
            {
                throw new Exception("No se encontró la transacción");
            }

            var consultarTransaccionDto = new ConsultarTransaccionDto
            {
                NumeroTransaccion = transaccion.NumeroTransaccion.ToString(),
                Desde = transaccion.Fecha, // Asumiendo que 'Desde' es la fecha de la transacción
                Hasta = transaccion.Fecha,  // Asumiendo que 'Hasta' es la fecha de la transacción
                NumeroCuentaOrigen = transaccion.NumeroCuentaOrigen.ToString(),
                NumeroCuentaDestino = transaccion.NumeroCuentaDestino.ToString()
            };

            return consultarTransaccionDto;
        }
        public async Task<List<ConsultarTransaccionDto>> ObtenerTodaslasTransacciones()
        {
            var transacciones = await _transaccionesRepo.ObtenerTodaslasTransacciones();

            var transaccionesDto = transacciones.Select(t => new ConsultarTransaccionDto
            {
                NumeroTransaccion = t.NumeroTransaccion.ToString(),
                Desde = t.Fecha,
                Hasta = t.Fecha,
                NumeroCuentaOrigen = t.NumeroCuentaOrigen.ToString(),
                NumeroCuentaDestino = t.NumeroCuentaDestino.ToString()
            }).ToList();

            return transaccionesDto ?? new List<ConsultarTransaccionDto>();
        }

    }

}
