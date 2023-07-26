using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OpenRA.MasterServer.Services;

public class ValidationService
{
    private readonly IHttpClientFactory _clientFactory;

    public ValidationService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<bool> CheckModIcon(string url, int size)
    {
        try
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {

            }



            var stream = await response.Content.ReadAsStreamAsync();

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var png = new Png(memoryStream);

            return png.Height == size && png.Width == size;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public bool PingHost(IPAddress address, int port)
    {
        try
        {
            // var hosts = Dns.GetHostAddresses(host[0]);
            //var endpoint = new IPEndPoint(host., int.Parse(host[1]));
            //var endpoint = new DnsEndPoint(host[0], Int32.Parse(host[1]));

            //var client = new TcpClient(endpoint.AddressFamily) { NoDelay = true };
            using var client = new TcpClient();
            client.Connect(address, port);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}