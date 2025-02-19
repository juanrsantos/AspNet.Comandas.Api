using Comandas.Api.Dtos;
using Comandas.Api.Models;
using Comandas.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using System.Threading;

namespace Comandas.Api.Services.Implementation
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
                var mesa = await _mesaRepository.GetMesaPorNumeroMesa(comanda.NumeroMesa);


                if (mesa is null)
                {
                    throw new BadRequestException("Mesa não encontrada");
                }

                if (mesa.SituacaoMesa != 0)
                {
                    throw new BadRequestException("Mesa ocupada");
                }

                mesa.SituacaoMesa = 1;
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


    }
}
