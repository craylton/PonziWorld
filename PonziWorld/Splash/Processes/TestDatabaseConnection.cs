using PonziWorld.Events;
using Prism.Events;

namespace PonziWorld.Splash.Processes;

internal class TestDatabaseConnection
    : SagaProcess<
        TestDatabaseConnectionCommand,
        TestDatabaseConnectionCommandPayload,
        DatabaseConnectionTestedEvent,
        DatabaseConnectionTestedEventPayload>
{
    public static TestDatabaseConnection Process => new();
    private TestDatabaseConnection() { }
}

internal class TestDatabaseConnectionCommand
    : PubSubEvent<TestDatabaseConnectionCommandPayload>
{ }

internal record TestDatabaseConnectionCommandPayload;

internal class DatabaseConnectionTestedEvent
    : PubSubEvent<DatabaseConnectionTestedEventPayload>
{ }

internal record DatabaseConnectionTestedEventPayload(
    bool Success);
