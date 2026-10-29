namespace RetroGamesStore.Models;

public class Sale
{
    public int Id { get; set; }

    public string Customer { get; set; } = "";

    public int GameId { get; set; }

    public string GameName { get; set; } = "";

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }

    public DateTime Date { get; set; }
}