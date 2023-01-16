using Lururen.Networking.SimpleSocketBus;

namespace Lururen.Testing;

public class CommandQueueTests
{
    private class TestCommand : ICommand
    {
        public bool Executed = false;
        public void Run(Guid client, Application app)
        {
            Executed = true;
        }
    }

    private class TestApp : Application
    {
        public override void Init()
        {
            this.DataBus = new SocketDataBus();
        }

        public override void Dispose()
        {
        }
    }

    [Fact]
    public void ProcessCommandTest()
    {
        Application app = new TestApp();
        CommandQueue queue = new CommandQueue(app);
        TestCommand testCmd = new TestCommand();
        queue.Push(Guid.Empty, testCmd);
        Assert.False(testCmd.Executed);
        queue.ProcessCommands();
        Assert.True(testCmd.Executed);
    }
}