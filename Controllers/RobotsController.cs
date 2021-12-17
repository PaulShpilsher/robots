using Microsoft.AspNetCore.Mvc;
using WhichRobotApi.Models;
using WhichRobotApi.Services;

namespace WhichRobotApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RobotsController : ControllerBase
{
    private readonly ILogger<RobotsController> _logger;
    private readonly IRobotsService _robotsService;

    public RobotsController(ILogger<RobotsController> logger, IRobotsService robotsService)
    {
        _logger = logger;
        _robotsService = robotsService;
    }

    [HttpPost]
    public async Task<TransportRobotResponse> GetTransportRobot(TransportRobotRequest request)
    {
        return await _robotsService.GetTransportRobot(request);
    }
}
