using NequiGestion.Logica.Dtos;
using NequiGestion.Persistencia;
//using NequiGestion.Entidades2._0; // ? Asegura importar la entidad
using System;
using System.Threading.Tasks;
using NecliGestion.Entidades2._0;
using System.Collections.Generic;

namespace NequiGestion.Logica.Services
{
    public class TransaccionesService
    {
        private readonly TransaccionesRepository _transaccionesRepo;

        public TransaccionesService(TransaccionesRepository transaccionesRepo)
        {
            _transaccionesRepo = transaccionesRepo;
        }

        public async Task<RegistrarTransaccionDto> RegistrarTransaccion(RegistrarTransaccionDto transaccionDto)
        {
            if (transaccionDto.Monto < 1000 || transaccionDto.Monto > 5000000)
            {
                throw new Exception("El monto debe estar entre $1000 COP y $5.000.000");
            }

            var transaccion = new Transacciones
            {
                NumeroTransaccion = new Random().Next(1000, 9999), // ? Genera un número aleatorio
                CuentaOrigen = transaccionDto.CuentaOrigen,
                CuentaDestino = transaccionDto.CuentaDestino,
                Monto = transaccionDto.Monto,
                Fecha = DateTime.Now,
                Tipo = "Salida"
            };

            await _transaccionesRepo.RegistrarTransaccion(transaccion);
            return transaccionDto;
        }

        public async Task<ConsultarTransaccionDto> ConsultarTransaccion(int numeroTransaccion)
        {
            var transaccion = await _transaccionesRepo.ObtenerTransaccionPorId(numeroTransaccion);
            if (transaccion == null)
            {
                throw new Exception("No se encontró la transacción");
            }

            return new ConsultarTransaccionDto
            {
                NumeroTransaccion = transaccion.NumeroTransaccion.ToString(),
                Desde = transaccion.Fecha,
                Hasta = transaccion.Fecha
            };
        }

        public async Task<List<Transacciones>> ConsultarTodasLasTransacciones()
        {
            return await _transaccionesRepo.ObtenerTodaslasTransacciones();
        }

        public async Task<bool> EliminarTransaccion(EliminarTransaccionDto eliminarTransaccionDto)
        {
            if (!int.TryParse(eliminarTransaccionDto.NumeroTransaccion, out int numeroTransaccion))
            {
                throw new Exception($"El número de transacción {eliminarTransaccionDto.NumeroTransaccion} no es válido");
            }

            var transaccion = await _transaccionesRepo.ObtenerTransaccionPorId(numeroTransaccion);
            if (transaccion == null)
            {
                throw new Exception($"No se puede eliminar la transacción con ID {numeroTransaccion} porque no existe");
            }
            return await _transaccionesRepo.EliminarTransaccion(numeroTransaccion);
        }
    }
}
