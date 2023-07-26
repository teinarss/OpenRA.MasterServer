using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using OpenRA.MiniYamlParser;

namespace OpenRA.MasterServer.Legacy;

public class LegacyUpdateServerEndpoint : EndpointWithoutRequest
{
    private readonly MasterServerContext _context;

    public LegacyUpdateServerEndpoint(MasterServerContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Post("/legacy");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        using var stream = new StreamReader(HttpContext.Request.Body);

        var body = await stream.ReadToEndAsync();

        var yaml = MiniYamlLoader.FromString(body);

        var root = yaml.First();

        var miniYamlValues = root.Value.ToDictionary();
        var remoteAddress = HttpContext.Connection.RemoteIpAddress;

        var clients = miniYamlValues["Clients"].Nodes.Select(GetClient).ToList();

        var server = new Server
        {
            ProtocolVersion = int.Parse(miniYamlValues["Protocol"].Value),
            Name = miniYamlValues["Name"].Value,
            Address = remoteAddress.ToString(),
            Port = int.Parse(miniYamlValues["Address"].Value.Split(":").Last()),
            Map = miniYamlValues["Map"].Value,
            Mod = miniYamlValues["Mod"].Value,
            ModTitle = miniYamlValues["ModTitle"].Value,
            ModWebsite = miniYamlValues["ModWebsite"].Value,
            ModIcon32 = miniYamlValues["ModIcon32"].Value,
            MaxPlayers = int.Parse(miniYamlValues["MaxPlayers"].Value),
            Protected = bool.Parse(miniYamlValues["Protected"].Value),
            Authentication = bool.Parse(miniYamlValues["Authentication"].Value),
            DisabledSpawnPoints = miniYamlValues["DisabledSpawnPoints"].Value.Split(",").Select(p => int.Parse(p)).ToArray(),
            Clients = clients
        };

        await _context.Servers.AddAsync(server);
        await _context.SaveChangesAsync();


        await SendStringAsync("[200] an error message");
    }

    private GameClient GetClient(MiniYamlNode clientNode)
    {

        var nodes = clientNode.Value.ToDictionary();

        return new GameClient
        {
            Name = nodes["Name"].Value,
            Fingerprint = nodes["Fingerprint"].Value,
            Color = nodes["Color"].Value,
            Team = int.Parse(nodes["Team"].Value),
            Faction = nodes["Faction"].Value,
            SpawnPoint = int.Parse(nodes["SpawnPoint"].Value),
            IsAdmin = bool.Parse(nodes["IsAdmin"].Value),
            IsSpectator = bool.Parse(nodes["IsSpectator"].Value),
            IsBot = bool.Parse(nodes["IsBot"].Value),

        };

    }
}

public class Server
{
    public int ProtocolVersion { get; set; }

    public string Name { get; set; }

    public int Port { get; set; }

    public ServerState State { get; set; }

    public int MaxPlayers { get; set; }

    public string Map { get; set; }

    public string Mod { get; set; }

    public string Version { get; set; }

    public string ModTitle { get; set; }

    public string ModWebsite { get; set; }

    public string ModIcon32 { get; set; }

    public string Location { get; set; }

    public bool Protected { get; set; }

    public bool Authentication { get; set; }

    public string Started { get; set; }

    public int PlayTime { get; set; }

    public ICollection<GameClient> Clients { get; set; }

    //public int[] DisabledSpawnPoints { get; set; }
    public string Address { get; set; }
    public long TimeStamp { get; set; }
    public int Id { get; set; }
    public int[] DisabledSpawnPoints { get; set; }
}

public class GameClient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Fingerprint { get; set; }
    public string Color { get; set; }
    public string Faction { get; set; }
    public int Team { get; set; }
    public int SpawnPoint { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSpectator { get; set; }
    public bool IsBot { get; set; }

    public string Address { get; set; }
}

public enum ServerState
{
    WaitingPlayers = 1,
    GameStarted = 2,
    ShuttingDown = 3
}