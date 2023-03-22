namespace ConsoleReflectionPopulator.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
        public IList<Pedido> Pedidos { get; set; }
    }
}