using System;
using System.Reflection;

namespace Zork.Scripts
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; }

        public IEnumerable<string> Verbs { get; }

        public CommandAttribute(string commandName, string verb) :
            this(commandName, new string[] { verb })
        {
        }

        public CommandAttribute(string commandName, string[] verbs)
        {
            CommandName = commandName;
            Verbs = verbs;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandClassAttribute : Attribute
    {
        //Type[] types = Assembly.GetExecutingAssembly().GetTypes();

    }
}
