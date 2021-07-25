using System.Reflection;
using System.Threading.Tasks;
using Lirxe;
using VkNet;
using VkNet.Model;

namespace Sample
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var bot = new
            {
                AccessToken = "ACCESS_TOKEN",
                PublicId = (ulong)1
            };
            
            
            var vk = new VkApi();
            vk.Authorize(new ApiAuthParams(){AccessToken = bot.AccessToken});
            var source = new LongpoolSource(vk, bot.PublicId);
            var run = new Runner(new[] {Assembly.GetExecutingAssembly()});
            source.Request += ctx => run.Run(ctx);
            await source.RunAsync();
        }
    }
}