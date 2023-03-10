namespace Lururen.Testing;

public class CommandQueueTests
{
    private class TestCommand : IRunnableCommand
    {
        public bool Executed = false;

        public void Run(Guid client, Application app)
        {
            Executed = true;
        }
    }

    private class TestApp : Application
    {
        public TestApp() : base(new SocketServerMessageBridge())
        {
        }

        public override void Init()
        {
        }

        public override void Dispose()
        {
        }

        public override ResourceInfo GetResourceInfo()
        {
            throw new NotImplementedException();
        }

        public override Stream GetResource(string resourceName)
        {
            throw new NotImplementedException();
        }
    }

    [Fact]
    public void ProcessCommandTest()
    {
        Application app = new TestApp();
        CommandQueue queue = new(app);
        TestCommand testCmd = new();
        queue.Push(Guid.Empty, testCmd);
        Assert.False(testCmd.Executed);
        queue.ProcessCommands();
        Assert.True(testCmd.Executed);
    }
}