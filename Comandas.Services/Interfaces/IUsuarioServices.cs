using Comandas.Shared.Dtos;

namespace Comandas.Services.Interfaces
{
    public interface IUsuarioServices
    {
        Task<PagedResponseDto<UsuarioDTO>> GetUsuariosAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<UsuarioResponse> Login(UsuarioRequest usuarioRequest, CancellationToken cancellationToken);
    }
}
