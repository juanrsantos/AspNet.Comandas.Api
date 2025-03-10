using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Data.Repositories.Implementation
{
    public class MesaRepository : IMesaRepository
    {
        private readonly ComandaDbContext _context;

        public MesaRepository(ComandaDbContext context)
        {
            _context = context;
   
        }

        public async Task<MesaDTO> GetMesa(int id)
        {
            var mesa = await _context.Mesas.Where(x => x.Id == id).Select(x => new MesaDTO
            {
                Id = x.Id,
                NumeroMesa = x.NumeroMesa,
                SituacaoMesa = x.SituacaoMesa
            }).AsNoTracking().FirstOrDefaultAsync();


            return mesa;
        }

        public async Task<Mesa> GetMesaById(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            return mesa;
        }

        public async Task<Mesa?> GetMesaPorNumeroMesa(int numeroMesa)
        {
            try
            {
              
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



        public async Task SaveChangesAsync(CancellationToken? cancellationToken)
        {
            await _context.SaveChangesAsync();
    
        }

        public async Task  UpdateMesaAsync(Mesa mesa)
        {
            var existe = await _context.Mesas.AnyAsync(m => m.Id == mesa.Id);
            if (!existe) throw new Exception("Mesa não encontrada");

            _context.Entry(mesa).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                existe = await _context.Mesas.AnyAsync(m => m.Id == mesa.Id);
                if (!existe)
                {
                    throw new  BadRequestException("Mesa não encontrada");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Erro ao acessar ao banco");
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveMesaAsync(Mesa mesa)
        {
            _context.Mesas.Remove(mesa);
           await  _context.SaveChangesAsync();
        }

        public async Task AddAsync(Mesa mesa)
        {
           await  _context.Mesas.AddAsync(mesa);
        }
    }
}
