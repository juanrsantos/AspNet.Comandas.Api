using Comandas.Api.Controllers;
using Comandas.Api.Data;
using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Comandas.Api.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Threading;

namespace Comandas.Api.Repositories
{
    public class MesaRepository : IMesaRepository
    {
        private readonly ComandaDbContext _context;
        private readonly ILogger<UsuariosController> _logger;

        public MesaRepository(ComandaDbContext context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Mesa?> GetMesaPorNumeroMesa(int numeroMesa)
        {
            try
            {
                _logger.LogInformation($"[{nameof(GetMesaPorNumeroMesa)}] Iniciando consulta da mesa por Numero Mesa");
                // Verifica se o contexto ainda está aberto
                if (_context.Database.CanConnect())
                {
                    var mesa = await _context.Mesas.TagWith("GetMesaPorNumero").FirstOrDefaultAsync(x => x.NumeroMesa == numeroMesa);
                    return mesa;
                }
                else
                {
                    throw new InvalidOperationException("Não é possível conectar ao banco de dados.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar mesa", ex);
            }
        }

        public async Task<PagedResponseDto<MesaDTO>> GetMesasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            var query = _context.Mesas.AsQueryable();
            var count = await query.CountAsync();

            var mesas = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .TagWith("GetMesas").AsNoTracking().Select(x => new MesaDTO
                {
                    Id = x.Id,
                    NumeroMesa = x.NumeroMesa,
                    SituacaoMesa = x.SituacaoMesa
                }).ToListAsync(cancellationToken);

            var res = new PagedResponseDto<MesaDTO>(mesas, count, page, pageSize);
            return res;
        }

    }
}
