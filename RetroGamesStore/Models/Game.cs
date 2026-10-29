namespace RetroGamesStore.Models;

public class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Platform { get; set; } = "";

    public decimal Price { get; set; }

    public int Stock { get; set; }
}