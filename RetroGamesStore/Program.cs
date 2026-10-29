using System.Globalization;
using System.Text;
using RetroGamesStore.Models;
using RetroGamesStore.Services;

namespace RetroGamesStore;

internal static class Program
{
    private static readonly CultureInfo CulturaBR =
        CultureInfo.GetCultureInfo("pt-BR");

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Retro Games Store";

        StoreService loja = new StoreService();

        ExibirSplash();
        MenuPrincipal(loja);

        Console.Clear();
        Console.WriteLine("Obrigado por visitar a Retro Games Store!");
        Console.WriteLine("Até a próxima!");
    }

    // TELA 1: SPLASH
    private static void ExibirSplash()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("""
        ======================================================
          ____  _____ _____ ____   ___
         |  _ \| ____|_   _|  _ \ / _ \
         | |_) |  _|   | | | |_) | | | |
         |  _ <| |___  | | |  _ <| |_| |
         |_| \_\_____| |_| |_| \_\\___/

                   RETRO GAMES STORE
        ======================================================
        """);

        Console.ResetColor();
        Console.WriteLine("\n       Sua loja de games clássicos!");
        Console.WriteLine("\nCarregando...");

        Thread.Sleep(1200);
    }

    // TELA 2: MENU PRINCIPAL
    private static void MenuPrincipal(StoreService loja)
    {
        bool executando = true;

        while (executando)
        {
            Console.Clear();
            Cabecalho("MENU PRINCIPAL");

            Console.WriteLine("  [1] Catálogo de jogos");
            Console.WriteLine("  [2] Relatório de vendas");
            Console.WriteLine("  [3] Resumo do estoque");
            Console.WriteLine("  [0] Sair");

            Console.Write("\nEscolha uma opção: ");
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ExibirCatalogo(loja);
                    break;

                case "2":
                    ExibirRelatorioVendas(loja);
                    break;

                case "3":
                    ExibirResumoEstoque(loja);
                    break;

                case "0":
                    executando = false;
                    break;

                default:
                    Mensagem("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    // TELA 3: CATÁLOGO / CARDS DE JOGOS
    private static void ExibirCatalogo(StoreService loja)
    {
        bool voltar = false;

        while (!voltar)
        {
            Console.Clear();
            Cabecalho("CATÁLOGO DE JOGOS");

            Console.WriteLine(
                $"{"ID",-5} {"JOGO",-25} {"PLATAFORMA",-20} " +
                $"{"PREÇO",12} {"ESTOQUE",10}");

            Console.WriteLine(new string('-', 76));

            foreach (Game jogo in loja.Games)
            {
                Console.WriteLine(
                    $"{jogo.Id,-5} " +
                    $"{Limitar(jogo.Name, 25),-25} " +
                    $"{Limitar(jogo.Platform, 20),-20} " +
                    $"{jogo.Price,12:C} " +
                    $"{jogo.Stock,10}");
            }

            Console.WriteLine("\n[1] Comprar / registrar pedido");
            Console.WriteLine("[2] Cadastrar novo jogo");
            Console.WriteLine("[0] Voltar ao menu");

            Console.Write("\nEscolha uma opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RegistrarPedido(loja);
                    break;

                case "2":
                    CadastrarJogo(loja);
                    break;

                case "0":
                    voltar = true;
                    break;

                default:
                    Mensagem("Opção inválida.");
                    break;
            }
        }
    }

    // REGISTRO DE PEDIDOS
    private static void RegistrarPedido(StoreService loja)
    {
        Console.Clear();
        Cabecalho("REGISTRAR PEDIDO");

        Console.Write("Digite o ID do jogo: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Mensagem("Digite um ID numérico válido.");
            return;
        }

        Game? jogo = loja.FindGame(id);

        if (jogo == null)
        {
            Mensagem("Jogo não encontrado.");
            return;
        }

        Console.WriteLine($"\nJogo: {jogo.Name}");
        Console.WriteLine($"Plataforma: {jogo.Platform}");
        Console.WriteLine($"Preço unitário: {jogo.Price:C}");
        Console.WriteLine($"Estoque disponível: {jogo.Stock}");

        Console.Write("\nNome do cliente: ");
        string cliente = Console.ReadLine()?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(cliente))
        {
            cliente = "Cliente não identificado";
        }

        Console.Write("Quantidade desejada: ");

        if (!int.TryParse(Console.ReadLine(), out int quantidade))
        {
            Mensagem("Digite uma quantidade numérica válida.");
            return;
        }

        bool sucesso = loja.RegisterSale(
            id,
            quantidade,
            cliente,
            out string mensagem);

        if (sucesso)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + mensagem);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + mensagem);
        }

        Console.ResetColor();
        Pausar();
    }

    // CADASTRO DE NOVOS JOGOS
    private static void CadastrarJogo(StoreService loja)
    {
        Console.Clear();
        Cabecalho("CADASTRAR NOVO JOGO");

        Console.Write("Nome do jogo: ");
        string nome = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Plataforma: ");
        string plataforma = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Preço (ex.: 79,90): ");
        string textoPreco = Console.ReadLine() ?? "";

        if (!decimal.TryParse(
            textoPreco,
            NumberStyles.Number,
            CulturaBR,
            out decimal preco))
        {
            Mensagem("Preço inválido. Exemplo: 79,90.");
            return;
        }

        Console.Write("Quantidade em estoque: ");

        if (!int.TryParse(Console.ReadLine(), out int estoque))
        {
            Mensagem("Digite uma quantidade inteira válida.");
            return;
        }

        bool sucesso = loja.AddGame(
            nome,
            plataforma,
            preco,
            estoque,
            out string mensagem);

        Console.WriteLine("\n" + mensagem);
        Pausar();
    }

    // TELA 4: RELATÓRIO DE VENDAS
    private static void ExibirRelatorioVendas(StoreService loja)
    {
        Console.Clear();
        Cabecalho("RELATÓRIO DE VENDAS");

        if (loja.Sales.Count == 0)
        {
            Console.WriteLine("Nenhum pedido foi registrado ainda.");
        }
        else
        {
            Console.WriteLine(
                $"{"ID",-5} {"DATA",-18} {"CLIENTE",-20} " +
                $"{"JOGO",-22} {"QTD.",5} {"TOTAL",12}");

            Console.WriteLine(new string('-', 88));

            foreach (Sale venda in loja.Sales.OrderBy(v => v.Id))
            {
                Console.WriteLine(
                    $"{venda.Id,-5} " +
                    $"{venda.Date.ToString("dd/MM/yyyy HH:mm"),-18} " +
                    $"{Limitar(venda.Customer, 20),-20} " +
                    $"{Limitar(venda.GameName, 22),-22} " +
                    $"{venda.Quantity,5} " +
                    $"{venda.Total,12:C}");
            }
        }

        decimal faturamento = loja.Sales.Sum(v => v.Total);
        int unidadesVendidas = loja.Sales.Sum(v => v.Quantity);

        Console.WriteLine("\n========== RESUMO DAS VENDAS ==========");
        Console.WriteLine($"Pedidos registrados: {loja.Sales.Count}");
        Console.WriteLine($"Unidades vendidas:   {unidadesVendidas}");
        Console.WriteLine($"Faturamento total:    {faturamento:C}");

        Pausar();
    }

    // RESUMO DO ESTOQUE
    private static void ExibirResumoEstoque(StoreService loja)
    {
        Console.Clear();
        Cabecalho("RESUMO DO ESTOQUE");

        Console.WriteLine($"Jogos cadastrados: {loja.Games.Count}");
        Console.WriteLine(
            $"Unidades disponíveis: {loja.Games.Sum(j => j.Stock)}");

        Console.WriteLine(
            $"Jogos sem estoque: {loja.Games.Count(j => j.Stock == 0)}");

        Console.WriteLine("\nJogos com estoque baixo (3 unidades ou menos):");

        List<Game> estoqueBaixo = loja.Games
            .Where(j => j.Stock <= 3)
            .ToList();

        if (estoqueBaixo.Count == 0)
        {
            Console.WriteLine("Nenhum jogo com estoque baixo.");
        }
        else
        {
            foreach (Game jogo in estoqueBaixo)
            {
                Console.WriteLine($"- {jogo.Name}: {jogo.Stock} unidade(s)");
            }
        }

        Pausar();
    }

    // FORMATAÇÃO DO CABEÇALHO
    private static void Cabecalho(string titulo)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.WriteLine("==========================================================");
        Console.WriteLine("                  RETRO GAMES STORE");
        Console.WriteLine($"                  {titulo}");
        Console.WriteLine("==========================================================");

        Console.ResetColor();
        Console.WriteLine();
    }

    // MENSAGENS E NAVEGAÇÃO
    private static void Mensagem(string texto)
    {
        Console.WriteLine("\n" + texto);
        Pausar();
    }

    private static void Pausar()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    private static string Limitar(string texto, int tamanho)
    {
        if (texto.Length <= tamanho)
        {
            return texto;
        }

        return texto[..(tamanho - 3)] + "...";
    }
}