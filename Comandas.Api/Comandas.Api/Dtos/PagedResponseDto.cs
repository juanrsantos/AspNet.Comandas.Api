namespace Comandas.Api.Dtos
{
    public class PagedResponseDto<T>
    {
       
            public int PaginaAtual { get; set; } // CurrentPage
            public int TotalPaginas { get; set; } // TotalPages
            public int TamanhoPagina { get; set; } // PageSize
            public int TotalRegistros { get; set; } // TotalCount
            public IEnumerable<T> Dados { get; set; } = new List<T>(); // Data

            public PagedResponseDto(IEnumerable<T> dados, int totalRegistros, int paginaAtual, int tamanhoPagina)
            {
                Dados = dados;
                TotalRegistros = totalRegistros;
                TamanhoPagina = tamanhoPagina;
                PaginaAtual = paginaAtual;
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanhoPagina);
            }
        
    }
}
