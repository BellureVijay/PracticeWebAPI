using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PracticeProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadPoolController : ControllerBase
    {
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            ThreadPool.GetAvailableThreads(out int worker, out int io);
            ThreadPool.GetMaxThreads(out int maxWorker,out int maxIo);
            return Ok(new
            {
                AvailableWorker=worker,
                AvailableIo=io,
                MaxWorker=maxWorker,
                MaxIo=maxIo,
                CurrentThreadId=Thread.CurrentThread.ManagedThreadId
            });
        }

        [HttpGet("block")]
        public IActionResult BlockThread()
        {
            Thread.Sleep(5000);
            return Ok("Thread blocked for 5 seconds");
        }

        [HttpGet("Async")]
        public async Task<IActionResult> AsyncAwait()
        {
            await Task.Delay(5000);
            return Ok("Async Delay completed");
        }
    }
}
