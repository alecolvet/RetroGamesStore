using RetroGamesStore.Models;

namespace RetroGamesStore.Services;

public class StoreService
{
    private int nextGameId = 5;
    private int nextSaleId = 1;

    public List<Game> Games { get; } = new()
    {
        new Game
        {
            Id = 1,
            Name = "Super Mario World",
            Platform = "Super Nintendo",
            Price = 99.90m,
            Stock = 10
        },

        new Game
        {
            Id = 2,
            Name = "Sonic the Hedgehog",
            Platform = "Mega Drive",
            Price = 79.90m,
            Stock = 8
        },

        new Game
        {
            Id = 3,
            Name = "Pac-Man",
            Platform = "Arcade",
            Price = 49.90m,
            Stock = 15
        },

        new Game
        {
            Id = 4,
            Name = "Street Fighter II",
            Platform = "Super Nintendo",
            Price = 89.90m,
            Stock = 5
        }
    };

    public List<Sale> Sales { get; } = new();

    public Game? FindGame(int id)
    {
        return Games.FirstOrDefault(game => game.Id == id);
    }

    public bool RegisterSale(
        int gameId,
        int quantity,
        string customer,
        out string message)
    {
        Game? game = FindGame(gameId);

        if (game == null)
        {
            message = "Jogo não encontrado.";
            return false;
        }

        if (quantity <= 0)
        {
            message = "A quantidade deve ser maior que zero.";
            return false;
        }

        if (quantity > game.Stock)
        {
            message = $"Estoque insuficiente. Disponível: {game.Stock}.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(customer))
        {
            customer = "Cliente não identificado";
        }

        game.Stock -= quantity;

        Sale sale = new Sale
        {
            Id = nextSaleId++,
            Customer = customer,
            GameId = game.Id,
            GameName = game.Name,
            Quantity = quantity,
            UnitPrice = game.Price,
            Total = game.Price * quantity,
            Date = DateTime.Now
        };

        Sales.Add(sale);

        message =
            $"Pedido #{sale.Id} registrado com sucesso!\n" +
            $"Jogo: {game.Name}\n" +
            $"Quantidade: {quantity}\n" +
            $"Total: {sale.Total:C}\n" +
            $"Estoque restante: {game.Stock}";

        return true;
    }

    public bool AddGame(
        string name,
        string platform,
        decimal price,
        int stock,
        out string message)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            message = "O nome do jogo é obrigatório.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(platform))
        {
            message = "A plataforma é obrigatória.";
            return false;
        }

        if (price < 0)
        {
            message = "O preço não pode ser negativo.";
            return false;
        }

        if (stock < 0)
        {
            message = "O estoque não pode ser negativo.";
            return false;
        }

        Game game = new Game
        {
            Id = nextGameId++,
            Name = name,
            Platform = platform,
            Price = price,
            Stock = stock
        };

        Games.Add(game);

        message = $"Jogo '{name}' cadastrado com sucesso!";

        return true;
    }
}