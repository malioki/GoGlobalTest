using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebAppGoGlobal.Data;
using WebAppGoGlobal.Models;

namespace WebAppGoGlobal.Controllers
{
    [Authorize]
    /*[ApiController]
    [Route(template: "Bookmark")]*/
    public class BookmarkController : Controller
    {
        private readonly ILogger<BookmarkController> _logger;

        private BookmarkContext _context;
        public BookmarkController(BookmarkContext context, ILogger<BookmarkController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet(template: "Index")]
        public IActionResult Index()
        {
            List<Repository> repositories = new List<Repository>();
            repositories = _context.Repositories.ToList();
            return View(repositories);
        }

        [HttpGet(template: "Search")]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetBookmark([FromBody]Repository repository)
        {
            try
            {
                var user = _context.Users.Select(s => s).Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                _context.Repositories.Add(new Repository() { Title = repository.Title, Description = repository.Description, Icon = repository.Icon, User = user });
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.InnerException);
            }
            return StatusCode(200, true);
        }

        [HttpGet]
        public async Task<IActionResult> Unbookmark(int id)
        {
            try
            {
                Repository repository = new Repository() { Id = Convert.ToInt32(id) };
                _context.Repositories.Attach(repository);
                _context.Repositories.Remove(repository);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.InnerException);
            }
            return StatusCode(200, true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}