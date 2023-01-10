namespace Lururen.Testing;

public class CommandQueueTests
{
    public class TestCommand : ICommand
    {
        public bool Executed = false;
        public void Run()
        {
            Executed = true;
        }
    }

    [Fact]
    public void ProcessCommandTest()
    {
        CommandQueue queue = new CommandQueue();
        TestCommand testCmd = new TestCommand();
        queue.Push(testCmd);
        queue.ProcessCommands();
        Assert.True(testCmd.Executed);
    }
}