namespace LabirynthCrawler.Model.MapGeneration;

public interface IMapBuilder
{
    IMapBuilder BuildEmpty();
    IMapBuilder FillWithWalls();
    IMapBuilder AddCorridors();
    IMapBuilder AddRooms(int roomCount, int roomSize);
    IMapBuilder AddMainRoom(int roomSizeX, int roomSizeY);
    IMapBuilder AddItems(int count);
    IMapBuilder AddWeapons(int count);
}