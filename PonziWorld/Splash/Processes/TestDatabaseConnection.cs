using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Splash.Processes;

internal class TestDatabaseConnection
    : SagaProcess<
        DatabaseConnectionTestedEvent,
        DatabaseConnectionTestedEventPayload,
        TestDatabaseConnectionCommand,
        TestDatabaseConnectionCommandPayload>
{
    public static TestDatabaseConnection Process => new();
    private TestDatabaseConnection() { }
}

internal class DatabaseConnectionTestedEvent
    : PubSubEvent<DatabaseConnectionTestedEventPayload>
{ }

internal record DatabaseConnectionTestedEventPayload(
    bool Success);

internal class TestDatabaseConnectionCommand
    : PubSubEvent<TestDatabaseConnectionCommandPayload>
{ }

internal record TestDatabaseConnectionCommandPayload;
