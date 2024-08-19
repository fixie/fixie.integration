using System.Text.Json;
using System.Text.Json.Serialization;
using Fixie;
using Fixie.Reports;
using static System.Text.Json.JsonSerializer;

namespace CustomConvention.Tests;

public class JsonReport :
    IHandler<ExecutionStarted>,
    IHandler<TestSkipped>,
    IHandler<TestPassed>,
    IHandler<TestFailed>,
    IHandler<ExecutionCompleted>
{
    readonly TestEnvironment environment;
    readonly JsonSerializerOptions options;
    readonly string path;

    FileStream? stream;
    StreamWriter? writer;
    bool first;

    public JsonReport(TestEnvironment environment)
    {
        this.environment = environment;

        options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new ExceptionConverter());

        path = Path.Combine(environment.RootPath, "TestResults.json");
    }

    public async Task Handle(ExecutionStarted message)
    {
        stream = new FileStream(path, FileMode.Create);
        writer = new StreamWriter(stream);
        first = true;
        
        await writer.WriteLineAsync("[");
    }

    public async Task Handle(TestSkipped message) => await Append(message);

    public async Task Handle(TestPassed message) => await Append(message);

    public async Task Handle(TestFailed message) => await Append(message);

    public Task Handle(ExecutionCompleted message)
    {
        writer?.WriteLine();
        writer?.WriteLine("]");

        writer?.Dispose();
        stream?.Dispose();

        environment.Console.WriteLine($"Report: {path}");
        
        return Task.CompletedTask;
    }

    async Task Append<TMessage>(TMessage message)
    {
        if (writer != null)
        {
            if (first)
                first = false;
            else
                await writer.WriteLineAsync(",");

            await writer.WriteAsync(Serialize(message, options));
        }
    }

    class ExceptionConverter : JsonConverter<Exception>
    {
        public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Message", value.Message);
            writer.WriteString("Type", value.GetType().FullName);
            writer.WriteString("StackTrace", value.StackTraceSummary());
            writer.WriteEndObject();
        }
    }
}