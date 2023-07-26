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

//namespace OpenRA.MasterServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ServerController : ControllerBase
//    {
//        private readonly MasterServerContext _context;
//        private readonly IHttpClientFactory _clientFactory;

//        //private static List<ServerInputModel> servers = new List<ServerInputModel>();

//        private long STALE_GAME_TIMEOUT = 300;

//        public ServerController(MasterServerContext context, IHttpClientFactory clientFactory)
//        {
//            _context = context;
//            _clientFactory = clientFactory;
//        }

//        [HttpGet]
//        public async Task<ActionResult<Response>> Get()
//        {
//            var ts = DateTimeOffset.Now.ToUnixTimeSeconds() - STALE_GAME_TIMEOUT;
//            var servers = await _context.Servers.Where(c => c.TimeStamp > ts).ToListAsync();

//            return Ok(servers);
//        }

//        [HttpPost]
//        public async Task<ActionResult<Response>> Post([FromBody] ServerInputModel server)
//        {
//            try
//            {
//                var remoteAddress = HttpContext.Connection.RemoteIpAddress;

//                var open = PingHost(remoteAddress, server.Port);
//                if (!open)
//                    return new Response { Error = Error.NotResponding };

//                if (!string.IsNullOrWhiteSpace(server.ModIcon32))
//                {
//                    var result = await CheckModIcon(server.ModIcon32, 32);
//                }


//                var id = $"{remoteAddress}:{server.Port}";

//                var ser = await _context.Servers.FindAsync(id);

//                ser ??= new Server();
//                ser.Id = id;
//                ser.Address = remoteAddress.ToString();
//                ser.Port = server.Port;
//                ser.Name = server.Name;
//                ser.Authentication = server.Authentication;
//                ser.Location = server.Location;
//                ser.Map = server.Map;

//                ser.ModIcon32 = server.ModIcon32;
//                ser.MaxPlayers = server.MaxPlayers;
//                ser.Mod = server.Mod;
//                ser.ModTitle = server.ModTitle;
//                ser.ModWebsite = server.ModWebsite;
//                ser.Protected = server.Protected;
//                ser.PlayTime = server.PlayTime;
//                ser.ProtocolVersion = server.ProtocolVersion;
//                ser.Version = server.Version;

//                ser.TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();

//                foreach (var client in server.Clients)
//                {
//                    ser.Clients.Add(new GameClient1
//                    {
//                        Name = client.Name,
//                        Faction = client.Faction,
//                        Color = client.Color,
//                        Fingerprint = client.Fingerprint,
//                        IsAdmin = client.IsAdmin,
//                        IsBot = client.IsBot,
//                        IsSpectator = client.IsSpectator,
//                        SpawnPoint = client.SpawnPoint,
//                        Team = client.Team
//                    });
//                }

//                //var ser = new Server
//                //{
//                //    Address = address,
//                //    Name = server.Name,
//                //    TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds()
//                //};

//                if (server.State == 2)
//                {
//                    ser.State = ServerState.GameStarted;
//                }

//                _context.AddOrUpdate(ser);
//                await _context.SaveChangesAsync();
//            }
//            catch (SocketException e)
//            {
//                return NotFound();
//            }


//            return Ok();
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

//        async Task<bool> CheckModIcon(string url, int size)
//        {
//            try
//            {
//                var client = _clientFactory.CreateClient();
//                var stream = await client.GetStreamAsync(url);

//                var memoryStream = new MemoryStream();
//                await stream.CopyToAsync(memoryStream);

//                memoryStream.Seek(0, SeekOrigin.Begin);
//                var png = new Png(memoryStream);

//                return png.Height == size && png.Width == size;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                throw;
//            }

//        }
//    }

//    public class Response
//    {
//        public Error Error { get; set; }
//    }

//    public enum Error
//    {
//        NotResponding,
//        WrongFormat
//    }

//    public enum ServerState
//    {
//        WaitingPlayers = 1,
//        GameStarted = 2,
//        ShuttingDown = 3
//    }

//    public static class ContextExtensions
//    {
//        public static void AddOrUpdate(this DbContext ctx, object entity)
//        {
//            var entry = ctx.Entry(entity);
//            switch (entry.State)
//            {
//                case EntityState.Detached:
//                    ctx.Add(entity);
//                    break;
//                case EntityState.Modified:
//                    ctx.Update(entity);
//                    break;
//                case EntityState.Added:
//                    ctx.Add(entity);
//                    break;
//                case EntityState.Unchanged:
//                    //item already in db no need to do anything  
//                    break;

//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//    }
//}
