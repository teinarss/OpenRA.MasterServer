//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using OpenRA.MasterServer.Controllers;

//namespace OpenRA.MasterServer
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllers();
//            services.AddHttpClient();

//            services.AddTransient<ValidationService>();

//            services.AddDbContext<MasterServerContext>(options =>
//                options.UseInMemoryDatabase(Configuration.GetConnectionString("DefaultConnection")));

//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseHttpsRedirection();

//            app.UseRouting();

//            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
//        }
//    }

//    public class MasterServerContext : DbContext
//    {
//        public MasterServerContext(DbContextOptions<MasterServerContext> options) : base(options)
//        {
//        }

//        public DbSet<Server> Servers { get; set; }


//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Server>(p =>
//            {
//                p.ToTable("Servers");
//                p.HasKey(d => d.Id);

//                p.OwnsMany<GameClient1>(l => l.Clients, a => { a.WithOwner().HasForeignKey(g => g.Address); });
//            });
//        }
//    }

//    public class Server
//    {
//        public int ProtocolVersion { get; set; }

//        public string Name { get; set; }

//        public int Port { get; set; }

//        public ServerState State { get; set; }

//        public int MaxPlayers { get; set; }

//        public string Map { get; set; }

//        public string Mod { get; set; }

//        public string Version { get; set; }

//        public string ModTitle { get; set; }

//        public string ModWebsite { get; set; }

//        public string ModIcon32 { get; set; }

//        public string Location { get; set; }

//        public bool Protected { get; set; }

//        public bool Authentication { get; set; }

//        public string Started { get; set; }

//        public int PlayTime { get; set; }

//        public ICollection<GameClient1> Clients { get; set; }

//        //public int[] DisabledSpawnPoints { get; set; }
//        public string Address { get; set; }
//        public long TimeStamp { get; set; }
//        public string Id { get; set; }
//        public int[] DisabledSpawnPoints { get; set; }
//    }

//    public class GameClient1
//    {
//        public string Name { get; set; }
//        public string Fingerprint { get; set; }
//        public string Color { get; set; }
//        public string Faction { get; set; }
//        public int Team { get; set; }
//        public int SpawnPoint { get; set; }
//        public bool IsAdmin { get; set; }
//        public bool IsSpectator { get; set; }
//        public bool IsBot { get; set; }

//        public string Address { get; set; }
//    }

//    public class ValidationService
//    {
//        private readonly IHttpClientFactory _clientFactory;

//        public ValidationService(IHttpClientFactory clientFactory)
//        {
//            _clientFactory = clientFactory;
//        }

//        public async Task<bool> CheckModIcon(string url, int size)
//        {
//            try
//            {
//                var client = _clientFactory.CreateClient();

//                var response = await client.GetAsync(url);

//                if (!response.IsSuccessStatusCode)
//                {

//                }



//                var stream = await response.Content.ReadAsStreamAsync();

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
//}