﻿using System.Xml.Linq;
using Fixie.Reports;

namespace Fixie.Integration.Reports;

public class XUnitV2XmlReport :
    IHandler<TestSkipped>,
    IHandler<TestPassed>,
    IHandler<TestFailed>,
    IHandler<ExecutionCompleted>
{
    readonly SortedDictionary<string, ClassResult> report = [];
    readonly string path;
    readonly TestEnvironment environment;

    public XUnitV2XmlReport(TestEnvironment environment)
    {
        this.environment = environment;

        path = Path.Combine(environment.RootPath, "TestResults.xml");
    }

    public Task Handle(TestSkipped message)
    {
        ForClass(message).Add(message);
        return Task.CompletedTask;
    }

    public Task Handle(TestPassed message)
    {
        ForClass(message).Add(message);
        return Task.CompletedTask;
    }

    public Task Handle(TestFailed message)
    {
        ForClass(message).Add(message);
        return Task.CompletedTask;
    }

    ClassResult ForClass(TestCompleted message)
    {
        Parse(message.Test, out var type, out _);

        if (!report.ContainsKey(type))
            report.Add(type, new ClassResult(type));

        return report[type];
    }

    public Task Handle(ExecutionCompleted message)
    {
        var now = DateTime.UtcNow;

        Save(new XDocument(
            new XElement("assemblies",
                new XElement("assembly",
                    new XAttribute("name", environment.Assembly.Location),
                    new XAttribute("run-date", now.ToString("yyyy-MM-dd")),
                    new XAttribute("run-time", now.ToString("HH:mm:ss")),
                    new XAttribute("time", Seconds(message.Duration)),
                    new XAttribute("total", message.Total),
                    new XAttribute("passed", message.Passed),
                    new XAttribute("failed", message.Failed),
                    new XAttribute("skipped", message.Skipped),
                    new XAttribute("environment", $"{IntPtr.Size * 8}-bit {environment.TargetFramework}"),
                    new XAttribute("test-framework", environment.TestFramework),
                    report.Values.Select(x => x.ToElement())))));

        report.Clear();

        environment.Console.WriteLine($"Report: {path}");

        return Task.CompletedTask;
    }

    static string Seconds(TimeSpan duration)
    {
        //The XML Schema spec requires decimal values to use a culture-ignorant format.
        return FormattableString.Invariant($"{duration.TotalSeconds:0.000}");
    }

    void Save(XDocument document)
    {
        using var stream = new FileStream(path, FileMode.Create);
        using var writer = new StreamWriter(stream);
        document.Save(writer, SaveOptions.None);
    }

    class ClassResult(string name)
    {
        TimeSpan duration = TimeSpan.Zero;
        readonly List<XElement> results = [];
        int passed;
        int failed;
        int skipped;

        public void Add(TestSkipped message)
        {
            duration += message.Duration;
            skipped += 1;

            Parse(message.Test, out var type, out var method);

            var test = new XElement("test",
                new XAttribute("name", message.TestCase),
                new XAttribute("type", type),
                new XAttribute("method", method),
                new XAttribute("result", "Skip"),
                new XAttribute("time", Seconds(message.Duration)));

            test.Add(new XElement("reason", new XCData(message.Reason)));

            results.Add(test);
        }

        public void Add(TestPassed message)
        {
            duration += message.Duration;
            passed += 1;

            Parse(message.Test, out var type, out var method);

            results.Add(
                new XElement("test",
                    new XAttribute("name", message.TestCase),
                    new XAttribute("type", type),
                    new XAttribute("method", method),
                    new XAttribute("result", "Pass"),
                    new XAttribute("time", Seconds(message.Duration))));
        }

        public void Add(TestFailed message)
        {
            duration += message.Duration;
            failed += 1;

            Parse(message.Test, out var type, out var method);

            results.Add(
                new XElement("test",
                    new XAttribute("name", message.TestCase),
                    new XAttribute("type", type),
                    new XAttribute("method", method),
                    new XAttribute("result", "Fail"),
                    new XAttribute("time", Seconds(message.Duration)),
                    new XElement("failure",
                        new XAttribute("exception-type", message.Reason.GetType().FullName!),
                        new XElement("message", new XCData(message.Reason.Message)),
                        new XElement("stack-trace", new XCData(message.Reason.StackTraceSummary())))));
        }

        public XElement ToElement()
        {
            return new XElement("collection",
                new XAttribute("time", Seconds(duration)),
                new XAttribute("name", name),
                new XAttribute("total", passed + failed + skipped),
                new XAttribute("passed", passed),
                new XAttribute("failed", failed),
                new XAttribute("skipped", skipped),
                results);
        }
    }

    static void Parse(string fullName, out string className, out string methodName)
    {
        var indexOfMemberSeparator = fullName.LastIndexOf(".");
        className = fullName.Substring(0, indexOfMemberSeparator);
        methodName = fullName.Substring(indexOfMemberSeparator + 1);
    }
}