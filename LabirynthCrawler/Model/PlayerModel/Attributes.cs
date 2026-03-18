using System.Numerics;

namespace LabirynthCrawler.Model.PlayerModel;

public class Attributes(int power = 0, int agility = 0, int health = 0, int luck = 0, int aggression = 0, int wisdom = 0)
{
    public int Power = power;
    public int Agility = agility;
    public int Health = health;
    public int Luck = luck;
    public int Aggression = aggression;
    public int Wisdom = wisdom;
}