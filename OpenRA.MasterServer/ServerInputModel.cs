using System.Drawing;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using OpenRA.MasterServer.Controllers;

namespace OpenRA.MasterServer
{
    public class ServerInputModel
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

        public int Players { get; set; }

        public int Bots { get; set; }

        public int Spectators { get; set; }

        public int PlayTime { get; set; }

        public GameClient[] Clients { get; set; }

        public int[] DisabledSpawnPoints { get; set; }
    }

    public class GameClient
    {
        public string Name { get; set; }
        public string Fingerprint { get; set; }
        public Color Color { get; set; }
        public string Faction { get; set; }
        public int Team { get; set; }
        public int SpawnPoint { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpectator { get; set; }
        public bool IsBot { get; set; }
    }
}