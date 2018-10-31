using System;

public class RobotState
{
    public RobotState()
    {
        runstat = 1;
        alarm = 0;
        alarminfo = "";
    }
    public int deviceid { get; set; }

    public int runstat { get; set; }

    public int alarm { get; set; }

    public string alarminfo { get; set; }
}
