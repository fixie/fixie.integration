using System;
using System.Runtime.CompilerServices;

namespace Fixie.Integration;

public static class Log
{
    public static void WhereAmI([CallerMemberName] string method = null)
    {
        Console.WriteLine(method);

        if (method == "Dispose")
            Console.WriteLine();
    }

    public static void WhereAmI(object[] args, [CallerMemberName] string method = null)
        => Console.WriteLine($"{method}({String.Join(", ", args)})");

    public static void WhereAmI<T>(object[] args, [CallerMemberName] string method = null)
        => Console.WriteLine($"{method}<{typeof(T)}>({String.Join(", ", args)})");

    public static void WriteLine(string line)
        => Console.WriteLine(line);
}