using System.Net.Http.Headers;
using Newtonsoft.Json;
using WhichRobotApi.Models;

namespace WhichRobotApi.Services;

public class RobotsService: IRobotsService
{
    public async Task<TransportRobotResponse> GetTransportRobot(TransportRobotRequest request)
    {
        // asuming robots collection is not empty
        // otherwise the following will throw an exception
        var robots = await GetAvailableRobots();

        var robotsWithDistance = robots
            .Select( robot => new {
                Robot = robot,
                Distance = CalcDistance(request.X, request.Y, robot.X, robot.Y)
            })
            .OrderBy(x => x.Distance);

        var closestRobotWithDistance = robotsWithDistance.First();

        // getting robots within 10 distance units of the closest one
        var distanceThreshold = closestRobotWithDistance.Distance + 10;
        var multipleRobotsWithDistance = robotsWithDistance.TakeWhile(x => x.Distance <= distanceThreshold);
        
        // choosing robot
        var transportRobotWithDistance = multipleRobotsWithDistance.Count() > 1
            ? multipleRobotsWithDistance.OrderByDescending(x => x.Robot.BatteryLevel).First()
            : closestRobotWithDistance;

        // TODO: refactor using a factory pattern
        return new TransportRobotResponse
        {
            RobotId = transportRobotWithDistance.Robot.RobotId,
            BatteryLevel = transportRobotWithDistance.Robot.BatteryLevel,
            DistanceToGoal = transportRobotWithDistance.Distance
        };
    }
    private async Task<List<Robot>> GetAvailableRobots()
    {
        // TODO: refactor using DI 
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
 
        var httpResponse = await httpClient.GetAsync("https://60c8ed887dafc90017ffbd56.mockapi.io/robots");
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Cannot retrieve Robots");
        }

        var content = await httpResponse.Content.ReadAsStringAsync();
        var robots = JsonConvert.DeserializeObject<List<Robot>>(content);
        if(robots == null || !robots.Any())
        {
            throw new Exception("No Robots received");
            
        }
        return robots;
    }

    public double CalcDistance(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y2;

        return Math.Sqrt((dx*dx) + (dy*dy));
    }
}