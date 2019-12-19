using System.Threading.Tasks;
using Bot;
using Bot.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Tests
{
    public class ApiTests
    {
        [Test]
        public async Task Get_works()
        {
            var subject = new BotController(new NullLogger<BotController>());
            var result = await subject.Get();
            Assert.That(result, Is.TypeOf<Response>());
        }
    }
}