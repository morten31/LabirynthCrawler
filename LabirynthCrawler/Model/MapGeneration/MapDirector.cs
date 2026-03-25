using System.Net.NetworkInformation;

namespace LabirynthCrawler.Model.MapGeneration;

public class MapDirector
{
    public void BuildStandardMap(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddMainRoom(10, 5)
            .AddCorridors()
            .AddRooms(1, 4)
            .AddRooms(6, 3)
            .AddItems(7)
            .AddWeapons(4)
            ;
    }
    public void BuildLabirynth(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddCorridors()
            .AddRooms(7, 3)
            .AddItems(7)
            .AddWeapons(4)
            ;
    }

    public void BuildWithoutItems(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddCorridors()
            .AddMainRoom(10,5)
            .AddRooms(5, 3)
            ;
    }
}