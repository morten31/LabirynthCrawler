using LabirynthCrawler.Model.MapGeneration;

namespace LabirynthCrawler.Model.Themes;

public interface IMapGenerationStrategy
{
    void Generate(IMapBuilder builder);
}