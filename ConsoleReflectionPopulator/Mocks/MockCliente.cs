namespace ConsoleReflectionPopulator.Mocks
{
    public static class Mock
    {
        public static object GetCliente() => new
        {
            Id = 1,
            Nome = "Cliente 1",
            Endereco = GetEndereco(),
            Pedidos = GetPedidos()
        };

        public static object GetEndereco() =>
            new
            {
                Rua = "Rua A",
                Numero = "123",
                Cidade = "Cidade B"
            };

        public static List<object> GetPedidos() =>
            new List<object>
            {
                new { PedidoId = 1, Produto = "Produto 1", Valor = 100 },
                new { PedidoId = 2, Produto = "Produto 2", Valor = 200 }
            };
    }
}