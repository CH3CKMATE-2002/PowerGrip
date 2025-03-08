namespace Andreas.PowerGrip.Systemd.Controllers;

[ApiController, Route("[controller]")]
public class SystemController: ControllerBase
{
    [HttpGet, Route("apt-update")]
    public async Task<ActionResult<ServiceResponse<ProcessData>>> AptUpdate()
    {
        var psi = new ProcessStartInfo
        {
            FileName = "apt",
            Arguments = "update",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var stopwatch = Stopwatch.StartNew();
        using var process = Process.Start(psi);

        if (process is null)
        {
            stopwatch.Stop();
            return BadRequest(new ServiceResponse<ProcessData>
            {
                Success = false,
                Title = "Failure at Launching Process",
                Data = ProcessData.FailedToLaunch(),
                Errors =
                [
                    ServiceError.InternalServerError("The 'apt' process could not be launched")
                ]
            });
        }

        await process.WaitForExitAsync();
        stopwatch.Stop();

        return new ServiceResponse<ProcessData>
        {
            Success = true,
            Title = "Process Completed",
            Data = new ProcessData
            {
                Tag = "apt update",
                LaunchSuccess = true,
                ExitCode = process.ExitCode,
                RunningTime = stopwatch.Elapsed,
                StdError = await process.StandardError.ReadToEndAsync(),
                StdOutput = await process.StandardOutput.ReadToEndAsync(),
            },
        };
    }
}