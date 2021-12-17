namespace WhichRobotApi.Models;

public class TransportRobotResponse
{
    public int RobotId { get; set; }

    public double DistanceToGoal { get; set; }

    public int BatteryLevel { get; set; }
}