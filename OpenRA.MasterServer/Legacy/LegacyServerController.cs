//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Sockets;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using OpenRA.MiniYamlParser;

//namespace OpenRA.MasterServer.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class LegacyServerController : ControllerBase
//    {
//        private readonly MasterServerContext _context;
//        private readonly ValidationService _validationService;


//        //private static List<ServerInputModel> servers = new List<ServerInputModel>();

//        private long STALE_GAME_TIMEOUT = 300;

//        public LegacyServerController(MasterServerContext context, ValidationService validationService)
//        {
//            _context = context;
//            _validationService = validationService;
//        }

//        [HttpGet]
//        public async Task<ActionResult<Response>> Get()
//        {
//            var ts = DateTimeOffset.Now.ToUnixTimeSeconds() - STALE_GAME_TIMEOUT;
//            var servers = await _context.Servers.Where(c => c.TimeStamp > ts).ToListAsync();

//            return Ok(servers);
//        }

//        [HttpPost]
//        public async Task<ActionResult<Response>> Post()
//        {
//            try
//            {

//                using var stream = new StreamReader(HttpContext.Request.Body);

//                var body = await stream.ReadToEndAsync();


//                var yaml = MiniYamlLoader.FromString(body);

//                var root = yaml.First();

//                var ee = root.Value.ToDictionary();
//                var remoteAddress = HttpContext.Connection.RemoteIpAddress;

//                var clients = ee["Clients"].Nodes.Select(GetClient).ToList();

//                var server = new Server
//                {
//                    ProtocolVersion = int.Parse(ee["Protocol"].Value),
//                    Name = ee["Name"].Value,
//                    Address = remoteAddress.ToString(),
//                    Port = int.Parse(ee["Address"].Value.Split(":").Last()),
//                    Map = ee["Map"].Value,

//                    Mod = ee["Mod"].Value,
//                    ModTitle = ee["ModTitle"].Value,
//                    ModWebsite = ee["ModWebsite"].Value,
//                    ModIcon32 = ee["ModIcon32"].Value,
//                    MaxPlayers = int.Parse(ee["MaxPlayers"].Value),
//                    Protected = bool.Parse(ee["Protected"].Value),
//                    Authentication = bool.Parse(ee["Authentication"].Value),
//                    DisabledSpawnPoints = ee["DisabledSpawnPoints"].Value.Split(",").Select(p => int.Parse(p)).ToArray(),
//                    Clients = clients

//                };

//                //var open = PingHost(remoteAddress, server.Port);
//                //if (!open)
//                //    return new Response { Error = Error.NotResponding };

//                if (!string.IsNullOrWhiteSpace(server.ModIcon32))
//                {
//                    var result = await _validationService.CheckModIcon(server.ModIcon32, 32);
//                }


//                //var id = $"{remoteAddress}:{server.Port}";

//                //var ser = await _context.Servers.FindAsync(id);

//                //ser ??= new Server();
//                //ser.Id = id;
//                //ser.Address = remoteAddress.ToString();
//                //ser.Port = server.Port;
//                //ser.Name = server.Name;
//                //ser.Authentication = server.Authentication;
//                //ser.Location = server.Location;
//                //ser.Map = server.Map;

//                //ser.ModIcon32 = server.ModIcon32;
//                //ser.MaxPlayers = server.MaxPlayers;
//                //ser.Mod = server.Mod;
//                //ser.ModTitle = server.ModTitle;
//                //ser.ModWebsite = server.ModWebsite;
//                //ser.Protected = server.Protected;
//                //ser.PlayTime = server.PlayTime;
//                //ser.ProtocolVersion = server.ProtocolVersion;
//                //ser.Version = server.Version;

//                //ser.TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

//                //foreach (var client in server.Clients)
//                //{
//                //    ser.Clients.Add(new GameClient1
//                //    {
//                //        Name = client.Name,
//                //        Faction = client.Faction,
//                //        Color = client.Color,
//                //        Fingerprint = client.Fingerprint,
//                //        IsAdmin = client.IsAdmin,
//                //        IsBot = client.IsBot,
//                //        IsSpectator = client.IsSpectator,
//                //        SpawnPoint = client.SpawnPoint,
//                //        Team = client.Team
//                //    });
//                //}

//                ////var ser = new Server
//                ////{
//                ////    Address = address,
//                ////    Name = server.Name,
//                ////    TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds()
//                ////};

//                //if (server.State == 2)
//                //{
//                //    ser.State = ServerState.GameStarted;
//                //}

//                //_context.AddOrUpdate(ser);
//                //await _context.SaveChangesAsync();
//            }
//            catch (SocketException e)
//            {
//                return NotFound();
//            }


//            return Ok();
//        }

//        private GameClient1 GetClient(MiniYamlNode clientNode)
//        {

//            var nodes = clientNode.Value.ToDictionary();

//            return new GameClient1
//            {
//                Name = nodes["Name"].Value,
//                Fingerprint = nodes["Fingerprint"].Value,
//                Color = nodes["Color"].Value,
//                Team =  int.Parse(nodes["Team"].Value),
//                Faction = nodes["Faction"].Value,
//                SpawnPoint = int.Parse(nodes["SpawnPoint"].Value),
//                IsAdmin = bool.Parse(nodes["IsAdmin"].Value),
//                IsSpectator = bool.Parse(nodes["IsSpectator"].Value),
//                IsBot = bool.Parse(nodes["IsBot"].Value),

//            };

//        }

//        static bool PingHost(IPAddress address, int port)
//        {
//            try
//            {
//                // var hosts = Dns.GetHostAddresses(host[0]);
//                //var endpoint = new IPEndPoint(host., int.Parse(host[1]));
//                //var endpoint = new DnsEndPoint(host[0], Int32.Parse(host[1]));

//                //var client = new TcpClient(endpoint.AddressFamily) { NoDelay = true };
//                using var client = new TcpClient();
//                client.Connect(address, port);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }


//    }
//}
