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
            .AddItems(5)
            .AddWeapons(7)
            .AddEnemies(5)
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

    public void BuildOverworldMap(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddMainRoom(14, 10) 
            .AddRooms(4, 6)
            .AddRooms(3, 4)
            .AddCorridors()
            .AddItems(8)
            .AddWeapons(4)
            .AddEnemies(6)
            .AddArtifact();
    }

    public void BuildNetherMap(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddCorridors()
            .AddCorridors()
            .AddRooms(6, 3)
            .AddItems(5)
            .AddWeapons(4)
            .AddEnemies(9)
            .AddArtifact();
    }

    public void BuildEndMap(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddMainRoom(20, 10)
            .AddRooms(12, 2)
            .AddItems(4)
            .AddWeapons(3)
            .AddEnemies(8)
            .AddArtifact();
    }
}