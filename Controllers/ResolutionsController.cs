using Microsoft.AspNetCore.Mvc;
using ELECTIVE_H1_BSIT_32E3_RedDanielRafael.Models;

namespace ELECTIVE_H1_BSIT_32E3_RedDanielRafael.Controllers
{
    [ApiController]
    [Route("api/resolutions")]
    public class ResolutionsController : ControllerBase
    {
        private static List<Resolution> _resolutions = new List<Resolution>
        {
            new Resolution
            {
                Id = 1,
                Title = "Walk 20 minutes daily",
                IsDone = false,
                CreatedAt = DateTime.Parse("2025-12-15T12:00:00Z").ToUniversalTime()
            },
            new Resolution
            {
                Id = 2,
                Title = "Save 10% of income",
                IsDone = false,
                CreatedAt = DateTime.Parse("2025-12-15T12:00:00Z").ToUniversalTime()
            },
            new Resolution
            {
                Id = 3,
                Title = "Read 12 books",
                IsDone = true,
                CreatedAt = DateTime.Parse("2025-12-15T12:00:00Z").ToUniversalTime()
            }
        };
        private static int _nextId = 4;

        [HttpGet]
        public IActionResult GetAll([FromQuery] bool? isDone, [FromQuery] string? title)
        {
            if (Request.Query.ContainsKey("isDone"))
            {
                var isDoneValue = Request.Query["isDone"].ToString();
                if (isDoneValue.ToLower() != "true" && isDoneValue.ToLower() != "false")
                {
                    return BadRequest(new ErrorResponse
                    {
                        Error = "BadRequest",
                        Message = "Invalid isDone parameter.",
                        Details = new List<string> { "isDone must be true or false" }
                    });
                }
            }

            var filtered = _resolutions.AsEnumerable();

            if (isDone.HasValue)
            {
                filtered = filtered.Where(r => r.IsDone == isDone.Value);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                filtered = filtered.Where(r => r.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            var response = new ResolutionListResponse
            {
                Items = filtered.Select(r => new ResolutionDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    IsDone = r.IsDone
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Invalid id parameter.",
                    Details = new List<string> { "id must be greater than 0" }
                });
            }

            var resolution = _resolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "NotFound",
                    Message = "Resolution not found.",
                    Details = new List<string> { $"No resolution found with id: {id}" }
                });
            }

            return Ok(resolution);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateResolutionRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Request body is required.",
                    Details = new List<string> { "Request body is missing" }
                });
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Validation failed.",
                    Details = new List<string> { "title is required" }
                });
            }

            var newResolution = new Resolution
            {
                Id = _nextId++,
                Title = request.Title,
                IsDone = false,
                CreatedAt = DateTime.UtcNow
            };

            _resolutions.Add(newResolution);

            return StatusCode(201, newResolution);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateResolutionRequest? request)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Invalid id parameter.",
                    Details = new List<string> { "id must be greater than 0" }
                });
            }

            if (request == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Request body is required.",
                    Details = new List<string> { "Request body is missing" }
                });
            }

            if (request.Id == 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Body id is required.",
                    Details = new List<string> { "body id is missing" }
                });
            }

            if (id != request.Id)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Route id does not match body id.",
                    Details = new List<string>
                    {
                        $"route id: {id}",
                        $"body id: {request.Id}"
                    }
                });
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Validation failed.",
                    Details = new List<string> { "title is required" }
                });
            }

            var resolution = _resolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "NotFound",
                    Message = "Resolution not found.",
                    Details = new List<string> { $"No resolution found with id: {id}" }
                });
            }

            resolution.Title = request.Title;
            resolution.IsDone = request.IsDone;
            resolution.UpdatedAt = DateTime.UtcNow;

            return Ok(resolution);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "BadRequest",
                    Message = "Invalid id parameter.",
                    Details = new List<string> { "id must be greater than 0" }
                });
            }

            var resolution = _resolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "NotFound",
                    Message = "Resolution not found.",
                    Details = new List<string> { $"No resolution found with id: {id}" }
                });
            }

            _resolutions.Remove(resolution);

            return NoContent();
        }
    }
}
