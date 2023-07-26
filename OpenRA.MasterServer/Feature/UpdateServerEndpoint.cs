using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using OpenRA.MasterServer.Services;

namespace OpenRA.MasterServer.Feature;

public class UpdateServerEndpoint : Endpoint<UpdateServerRequest>
{
    private readonly ValidationService _validationService;

    public UpdateServerEndpoint(ValidationService validationService)
    {
        _validationService = validationService;
    }

    public override void Configure()
    {
        Post("/server");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateServerRequest req, CancellationToken ct)
    {


        var remoteAddress = HttpContext.Connection.RemoteIpAddress;

        var open = _validationService.PingHost(remoteAddress, req.Port);
        if (!open)
        {
            await SendAsync(new Response { Error = Error.NotResponding });

        }


        if (!string.IsNullOrWhiteSpace(req.ModIcon32))
        {
            var result = await _validationService.CheckModIcon(req.ModIcon32, 32);
        }




        await SendAsync(new Response { Error = Error.NotResponding });
    }
}
public class Response
{
    public Error Error { get; set; }
}

public enum Error
{
    NotResponding,
    WrongFormat
}
public class UpdateServerRequest
{
    public int ProtocolVersion { get; set; }

    public string Name { get; set; }

    public int Port { get; set; }

    public int State { get; set; }

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

    public GameClient[] Clients { get; set; }

    public int[] DisabledSpawnPoints { get; set; }
}

public class GameClient
{
    public string Name { get; set; }
    public string Fingerprint { get; set; }
    public string Color { get; set; }
    public string Faction { get; set; }
    public int Team { get; set; }
    public int SpawnPoint { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSpectator { get; set; }
    public bool IsBot { get; set; }
}