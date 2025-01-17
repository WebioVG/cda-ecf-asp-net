namespace cda_ecf_asp_net.Models;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string StatsCollectionName { get; set; } = string.Empty;
}
