using Comandas.Api.Enums;
using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Comandas.Services
{
    public class ComandasServices : IComandasServices
    {
        private readonly IComandasRepository _comandasRepository;
        private readonly IMesaRepository _mesaRepository;
        private readonly IComandaItemsRepository _comandaItemsRepository;
        private readonly ICardapioItemRepository _cardapioItemRepository;
        private readonly IPedidoCozinhaItemsRepository _pedidoCozinhaItemRepository;
        private readonly IPedidoCozinhaRepository _pedidoCozinhaRepository;
        private readonly IMesaServices _mesaServices;


        public ComandasServices(IComandasRepository comandasRepository, IMesaRepository mesaRepository, IComandaItemsRepository comandaItemsRepository, ICardapioItemRepository cardapioItemRepository, IPedidoCozinhaItemsRepository pedidoCozinhaItemRepository, IPedidoCozinhaRepository pedidoCozinhaRepository, IMesaServices mesaServices)
        {
            _comandasRepository = comandasRepository;
            _mesaRepository = mesaRepository;
            _comandaItemsRepository = comandaItemsRepository;
            _cardapioItemRepository = cardapioItemRepository;
            _pedidoCozinhaItemRepository = pedidoCozinhaItemRepository;
            _pedidoCozinhaRepository = pedidoCozinhaRepository;
            _mesaServices = mesaServices;
        }

        public async Task<ComandaGetDTO> Get(int id)
        {
            try
            {
                var comanda = await _comandasRepository.Get(id);

                if (comanda is null)
                {
                    throw new BadRequestException($"Comanda {id} não encontrada ");
                }

                var comandaDTO = new ComandaGetDTO
                {
                    Id = comanda.Id,
                    NroMesa = comanda.NroMesa,
                    NomeCliente = comanda.NomeCliente,
                };


                var comandaItensDto = await _comandaItemsRepository.GetItensdaComanda(id);
                comandaDTO.comandaItems = comandaItensDto.ToList();
                return comandaDTO;
            }
            catch (BadRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter comanda", ex);
            }
        }

        public async Task<PagedResponseDto<ComandaGetDTO>> GetComandasAsync(CancellationToken cancellationToken, int page, int pageSize)
        {
            return await _comandasRepository.GetComandasAsync(cancellationToken, page, pageSize);
        }

        public async Task<Comanda> Post(ComandaDTO comanda)
        {
            try
            {
                var mesa = await _mesaRepository.GetMesaPorNumeroMesa(comanda.NumeroMesa) ?? throw new BadRequestException("Mesa não encontrada");
                mesa.AlterarSituacao((int)SituacaoMesaEnum.Ocupado);
                // mesa.SituacaoMesa = 1;
                var novaComanda = new Comanda
                {
                    NumeroMesa = comanda.NumeroMesa,
                    NomeCliente = comanda.NomeCliente
                };

                await _comandasRepository.AddAsync(novaComanda);

                foreach (var item in comanda.CardapioItems)
                {
                    var novaComandaItem = new ComandaItem
                    {
                        Comanda = novaComanda,
                        CardapioItemId = item
                    };

                    await _comandaItemsRepository.AddAsync(novaComandaItem);

                    // Consultar se o cardapio possui preparo
                    var cardapioItem = await _cardapioItemRepository.GetCardapioItemPorId(item);

                    if (cardapioItem is null)
                    {
                        throw new BadRequestException($"Cardapio com código {item} não encontrado");
                    }

                    if (cardapioItem.PossuiPreparo)
                    {
                        // Se possui preparo criar Pedido de cozinha(Comanda) e Pedido de cozinha item(ComandaItem)
                        var pedidoCozinha = new PedidoCozinha
                        {
                            Comanda = novaComanda,
                            SituacaoId = 1
                        };
                        await _pedidoCozinhaRepository.AddAsync(pedidoCozinha);

                        var pedidoCozinhaItem = new PedidoCozinhaItem
                        {
                            PedidoCozinha = pedidoCozinha,
                            ComandaItem = novaComandaItem,
                        };

                        await _pedidoCozinhaItemRepository.AddAsync(pedidoCozinhaItem);
                    }
                }
                await _pedidoCozinhaItemRepository.SaveChangesAsync(default!);
      
                return novaComanda;
            }
            catch(BadRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex) 
            {
                throw new Exception("Erro ao criar comanda", ex);
            }
            // Devolvendo no cabeçalho da resposta a url de consulta do novo objeto gerado.
            // CreatedAtAction(nameof(Get), new { id = novaComanda.Id }, comanda);
        }

        public async Task UpdateComandaAsync(ComandaUpdateDTO comanda)
        {

            var comandaUpdate = await _comandasRepository.GetById(comanda.Id);

            if (comandaUpdate is null)
            {
                throw new NotFoundException("Não encontrado");
            }

            if (!string.IsNullOrEmpty(comanda.NomeCliente))
            {
                comandaUpdate.NomeCliente = comanda.NomeCliente;
            }
            if (comanda.NumeroMesa > 0)
            {
                // Verificar disponibilidade da mesa
                var mesa = await _mesaRepository.GetMesaPorNumeroMesa(comanda.NumeroMesa);

                if (mesa is null)
                {
                    throw new BadRequestException("Mesa invalida");
                }
                if (mesa.SituacaoMesa != (int)SituacaoMesaEnum.Disponivel)
                {
                    throw new BadRequestException("Mesa ocupada");
                }
                // Mudar status da mesa para ocupado
                mesa.SituacaoMesa = (int)SituacaoMesaEnum.Ocupado;
                // Mudar o status da mesa antiga para disponivel
                var mesantiga = await  _mesaRepository.GetMesaPorNumeroMesa(comandaUpdate.NumeroMesa);
                mesantiga.SituacaoMesa = (int)SituacaoMesaEnum.Disponivel;
                // Atualizar o numero da mesa comanda
                comandaUpdate.NumeroMesa = comanda.NumeroMesa;

            }

            // percorrer os itens da comanda
            foreach (var item in comanda.ComandaItems)
            {
                // verificar se esta incluindo itens 
                if (item.Incluir)
                {
                    var novoComandaItem = new ComandaItem
                    {
                        Comanda = comandaUpdate,
                        CardapioItemId = item.CardapioItemId,
                    };
                    await _comandaItemsRepository.AddAsync(novoComandaItem);
                   // await _context.ComandaItems.AddAsync(novoComandaItem);

                    // Verificar se o cardapio possui preparo
                    //var cardapioItem = await _context.CardapioItems.FirstOrDefaultAsync(x => x.Id == item.CardapioItemId);
                    var cardapioItem = await _cardapioItemRepository.GetCardapioItemPorId(novoComandaItem.CardapioItemId);
                    if (cardapioItem is null)
                    {
                        throw new BadRequestException("Cardapio invalido");
                    }
                    // criar o pedido de cozinha
                    if (cardapioItem.PossuiPreparo)
                    {
                        var novoPedidoCozinha = new PedidoCozinha
                        {
                            Comanda = comandaUpdate,
                            SituacaoId = 1
                        };

                        //await _context.PedidoCozinhas.AddAsync(novoPedidoCozinha);
                        await _pedidoCozinhaRepository.AddAsync(novoPedidoCozinha);
                        // criar o item do pedido de cozinha

                        var novoPedidoCozinhaItem = new PedidoCozinhaItem
                        {
                            PedidoCozinha = novoPedidoCozinha,
                            ComandaItem = novoComandaItem,
                        };
                        await _pedidoCozinhaItemRepository.AddAsync(novoPedidoCozinhaItem);
                    }
                }

                // verificar se esta excluindo itens 
                if (item.Excluir)
                {
                    //var comandaItemExcluir = await _context.ComandaItems.AsNoTracking().FirstAsync(x => x.Id == item.Id);
                    var comandaItenExcluir = await _comandaItemsRepository.GetComandaItemById(item.CardapioItemId);
                    _comandaItemsRepository.RemoverComandaItemAsync(comandaItenExcluir);
                }
            }

            try
            {
                await _comandasRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var existeComanda = await _comandasRepository.ComandaExiste(comanda.Id);
                if (!existeComanda)
                {
                    throw new NotFoundException("Comanda não encontrada");
                }

                throw;
            }
        }
    }
}
