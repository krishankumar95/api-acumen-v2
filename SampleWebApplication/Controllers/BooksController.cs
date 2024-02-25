using Microsoft.AspNetCore.Mvc;

namespace SampleWebApplication;

[Route("/api/books")]
[ApiController]
public class BooksController:ControllerBase
{
    private ILogger _logger; 
    public BooksController(ILogger<BooksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        _logger.LogInformation("Recieved a request to retreive all books");
       return Ok("All books will be returned");
    }

    [HttpGet("{id}")]
    public ActionResult Get(string id)
    {
        _logger.LogInformation($"Recieved a request to retreive book with id:{id}");
        return Ok ($"Book Info About {id}");
    }

    [HttpPost]
    public ActionResult Post([FromBody] Book request)
    {
        _logger.LogInformation($"Recieved a request to create a new book");
        return Created($"/api/books/{request.Id}",request);
    }
}
