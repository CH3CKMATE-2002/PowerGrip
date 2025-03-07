namespace Andreas.PowerGrip.Server.Services.Interfaces;

public interface ISysLogService
{
    string[] GetLogFilenames();

    string[] ReadLog(string filename);
}