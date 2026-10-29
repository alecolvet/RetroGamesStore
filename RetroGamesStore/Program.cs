using System;
using System.Text;

namespace RetroGamesStore;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Retro Games Store";

        ExibirSplash();
        MenuPrincipal();
    }

    static void ExibirSplash()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("""
        ======================================
             ____  _____ _____ ____   ___
            |  _ \| ____|_   _|  _ \ / _ \
            | |_) |  _|   | | | |_) | | | |
            |  _ <| |___  | | |  _ <| |_| |
            |_| \_\_____| |_| |_| \_\\___/

                 R E T R O   G A M E S
        ======================================
        """);

        Console.ResetColor();
        Console.WriteLine("\nCarregando loja...");
        System.Threading.Thread.Sleep(1500);
    }

    static void MenuPrincipal()
    {
        bool executando = true;

        while (executando)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("========== RETRO GAMES ==========");
            Console.ResetColor();

            Console.WriteLine("1 - Catálogo de jogos");
            Console.WriteLine("2 - Relatório de vendas");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha uma opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\nCatálogo de jogos");
                    Pausar();
                    break;

                case "2":
                    Console.WriteLine("\nRelatório de vendas");
                    Pausar();
                    break;

                case "0":
                    executando = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    Pausar();
                    break;
            }
        }
    }

    static void Pausar()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}