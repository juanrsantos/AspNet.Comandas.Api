using Comandas.Data.Repositories.Interfaces;
using Comandas.Domain;
using Comandas.Services;
using Comandas.Services.Interfaces;
using Comandas.Shared.Dtos;
using Comandas.Shared.Exceptions;
using Moq;
using System.Runtime.CompilerServices;

namespace Comandas.Tests
{
    public class MesaServicesTest
    {
        private readonly Mock<IMesaRepository> _mockRepository;
        private readonly MesaServices _mesaServices;
        private readonly Mock<IRedisRepository> _redisRepository;

        public MesaServicesTest()
        {
            _mockRepository = new Mock<IMesaRepository>();
            _redisRepository = new Mock<IRedisRepository>();
            _mesaServices = new MesaServices(_mockRepository.Object, _redisRepository.Object);
        }

        [Fact]
        public async Task AddAsyncServicesTest_success()
        {
            // [Arrange]
            Mesa mesa = new Mesa();
            // [Act]
            await  _mesaServices.AddAsync(mesa);
            // [Asserts]
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Mesa>()), Times.Once);

        }

        [Fact]
        public async Task GetMesaAsyncServicesTest_success_object_valid()
        {
            // [Arrange]
            int mesaId = 2;
            var mesaDTO = new MesaDTO { Id = mesaId, NumeroMesa = 123, SituacaoMesa = 0 };

           _mockRepository.Setup(x => x.GetMesa(It.IsAny<int>())).ReturnsAsync(mesaDTO);

  
            // [Act]
            var result = await _mesaServices.GetMesaAsync(mesaId);

            // [Assert]
            Assert.NotNull(result);
            Assert.Equal(mesaId, result.Id);
            Assert.Equal(mesaDTO.NumeroMesa, result.NumeroMesa);
            Assert.Equal(mesaDTO.SituacaoMesa, result.SituacaoMesa);
            _mockRepository.Verify(repo => repo.GetMesa(mesaId), Times.Once);
        }

        [Fact]
        public async Task RemoveMesaAsyncServicesTest_success()
        {
            // Arrange
            int mesaId = 1;
            var mesa = new Mesa { Id = mesaId, NumeroMesa = 123, SituacaoMesa = 0 };
            _mockRepository.Setup(repo => repo.GetMesaById(mesaId)).ReturnsAsync(mesa);

            // Act
            await _mesaServices.RemoveMesaAsync(mesaId);

            // Assert
            _mockRepository.Verify(repo => repo.GetMesaById(mesaId), Times.Once);
            _mockRepository.Verify(repo => repo.RemoveMesaAsync(mesa), Times.Once);

        }


        [Fact]
        public async Task RemoveMesaAsyncServicesTest_failed()
        {
            // [ARRANGE]
            int mesaId = 1;
            _mockRepository.Setup(repo => repo.GetMesaById(mesaId)).ReturnsAsync((Mesa) null);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _mesaServices.RemoveMesaAsync(mesaId));
        }

        [Fact]
        public async Task GetMesasAsyncServiceTests_success()
        {
            //[ARRANGE]
            int mesaId = 1;
            CancellationToken cancellationToken = new CancellationToken();
            var objetoEsperado = new PagedResponseDto<MesaDTO>(new List<MesaDTO>()
               {
                    new MesaDTO { Id = mesaId, NumeroMesa = 123, SituacaoMesa = 0  },
                    new MesaDTO { Id = mesaId, NumeroMesa = 123, SituacaoMesa = 0  }
               }, 2, 1, 10);

            _mockRepository.Setup(x => x.GetMesasAsync(It.IsAny<CancellationToken>(), It.IsAny<int>(), It.IsAny<int>()))
               .ReturnsAsync(objetoEsperado);


            //[ACT]
            var result = await _mesaServices.GetMesasAsync(cancellationToken, 1, 10);


            //[ASSERT]
            Assert.Equal(objetoEsperado.TotalPaginas, result.TotalPaginas); 
            Assert.Equal(objetoEsperado.PaginaAtual, result.PaginaAtual); 
            Assert.Equal(objetoEsperado.TamanhoPagina, result.TamanhoPagina);
            Assert.Equal(objetoEsperado.Dados.ToList().Count, result.Dados.ToList().Count); 

        }




    }
}