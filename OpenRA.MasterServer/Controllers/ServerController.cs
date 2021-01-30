using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OpenRA.MasterServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly MasterServerContext _context;

        private static List<Server> servers = new List<Server>();

        public ServerController(MasterServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Response>> Get()
        {
            //var servers = await _context.Servers.ToListAsync();

            return Ok(servers);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Post([FromBody]Server server)
        {
            try
            {
                var remoteAddress = HttpContext.Connection.RemoteIpAddress;
                var address = server.Address.Split(':');
                //var ou = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Tcp);
                var open = PingHost(remoteAddress, Int32.Parse(address[1]));
                if (!open)
                    return NotFound();

                servers.Add(server);

                var ser = new Server();

                //await _context.Servers.AddAsync(ser);
                //await _context.SaveChangesAsync();
            }
            catch (SocketException e)
            {
                return NotFound();
            }


            return Ok();
        }

        public static bool PingHost(IPAddress address,  int port)
        {
            

            try
            {
                // var hosts = Dns.GetHostAddresses(host[0]);
                //var endpoint = new IPEndPoint(host., int.Parse(host[1]));
                //var endpoint = new DnsEndPoint(host[0], Int32.Parse(host[1]));

                //var client = new TcpClient(endpoint.AddressFamily) { NoDelay = true };
                var client = new TcpClient();
                client.Connect(address, port);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class Response
    {

    }
}
