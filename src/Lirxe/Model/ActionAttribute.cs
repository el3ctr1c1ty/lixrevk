using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lirxe
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=true)]
    public class Action:Attribute
    {
        public string TextCommand { get;}
        public Regex TextPattern
        {
            get
            {
                if (string.IsNullOrEmpty(TextCommand)) return new Regex(Guid.NewGuid().ToString("N"));
                var pattern = TextCommand.Replace("\\", @"\\").Replace("/", @"\/")
                    .Replace("*", @"\*").Replace("+", @"\+");
                    
                var argsMatches = new Regex(@"\[(?<name>.*?)\]").Matches(pattern);
                if (argsMatches.Any())
                {
                    var name = pattern[argsMatches.First().Index..];

                    pattern = pattern.Replace(pattern.Substring(argsMatches.First().Index), name);
                    
                    foreach (Match argMatch in argsMatches)
                        pattern = pattern.Replace(argMatch.Value, $"(?<{argMatch.Groups["name"].Value}>.*)");
                }

                return new Regex(pattern);
            }
        }

        public string PayloadCommand { get; }

        public Action(string textCommand = null, string payloadCommand = null)
        {
            TextCommand = textCommand;
            PayloadCommand = payloadCommand;
        }
    }
}