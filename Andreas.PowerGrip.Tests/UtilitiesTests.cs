namespace Andreas.PowerGrip.Tests;

[TestFixture]
public class UtilitiesTests
{
    private ILogger<UtilitiesTests> _logger = null!;

    [SetUp]
    public void Setup()
    {
        _logger = LoggerCreator.Create<UtilitiesTests>();
    }

    [Test]
    public void UnixNodeStatus_Accurate()
    {
        using var _ = _logger.BeginScope("Stat Syscall Accuracy");
        const string FilePath = "test.txt";
        File.Delete(FilePath);

        using var file = File.Create(FilePath, 256, FileOptions.DeleteOnClose);
        using var writer = new StreamWriter(file) { AutoFlush = true };
        
        _logger.LogDebug("Writing to file: {file}", FilePath);
        writer.WriteLine("Hello?");

        var stat = UnixINodeStatus.Get(FilePath);
        var date1 = stat.LastModificationTime;

        Thread.Sleep(TimeSpan.FromSeconds(1)); // Wait a teeny-tiny noticable bit.

        _logger.LogDebug("Writing to file again: {file}", FilePath);
        writer.WriteLine("Is anyone there?!");

        stat = UnixINodeStatus.Get(FilePath);
        var date2 = stat.LastModificationTime;

        _logger.LogDebug("First write: {first}", date1);
        _logger.LogDebug("Last write: {last}", date2);

        Assert.That(date1, Is.LessThan(date2), "Expected first write to be before the last write");
        File.Delete(FilePath);

        Assert.Pass();
    }
}
