using WhichRobotApi.Models;

namespace WhichRobotApi.Services;

public interface IRobotsService
{
    Task<TransportRobotResponse> GetTransportRobot(TransportRobotRequest request);
}