// See https://aka.ms/new-console-template for more information
using ConsoleReflectionPopulator.Acao;
using ConsoleReflectionPopulator.Entidades;
using ConsoleReflectionPopulator.Mocks;

Console.WriteLine("Hello, World!");

var cliente = ReflectionPopulator.CreateObject<Cliente>(Mock.GetCliente());

Console.WriteLine($"Id: {cliente.Id}");
Console.WriteLine($"Nome: {cliente.Nome}");

Console.WriteLine("\nEndereco:");
Console.WriteLine($"Rua: {cliente.Endereco.Rua}");
Console.WriteLine($"Numero: {cliente.Endereco.Numero}");
Console.WriteLine($"Cidade: {cliente.Endereco.Cidade}");

Console.WriteLine("\nPedidos:");
foreach (var pedido in cliente.Pedidos)
{
    Console.WriteLine($"PedidoId: {pedido.PedidoId}");
    Console.WriteLine($"Produto: {pedido.Produto}");
    Console.WriteLine($"Valor: {pedido.Valor}\n");
}
Console.ReadLine();