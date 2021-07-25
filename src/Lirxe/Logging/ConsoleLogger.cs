using System;
using System.Text.RegularExpressions;

namespace Lirxe.Logging
{
    public class ConsoleLogger:Logger
    {
        private string Time => s(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"));
        private DateTime _job;
        
        // ReSharper disable InconsistentNaming
        private static string s(string s) => s.Replace("|", "$i").Replace(":", "$t");

        private static string un(string s) => s.Replace("$i", "|").Replace("$t", ":");
        // ReSharper restore InconsistentNaming
        public override void Log(string l) => WriteColor($"{Time} {l}");

        public override void Error(string e) => WriteColor($"|w,r:{Time}| {e}");

        public override void Warn(string w)=> WriteColor($"|w,y:{Time}| {w}");

        public override void Info(string i)=> WriteColor($"|b,w:{Time}| {i}");

        public override void Debug(string d)=> WriteColor($"|dgr:{Time} {s(d)}|");
        
        public override void StartJob(string j)
        {
            _job = DateTime.Now;
            WriteColor($"|w,dgr:{Time}| {s(j)}");
        }

        public override void FinishJob(string j)
        {
            WriteColor($"\r|b,w:{Time}||b,dgr: {Math.Round((DateTime.Now-_job).TotalMilliseconds,0)} ms.| {s(j)}");
        }

        public static void WriteColor(string input)
        {
            if (input.Contains("\r")) input = "\r" + input.Replace("\r", "");
            var s = Regex.Split(input, @"(\|.*?\|)");
            foreach (var i in s)
            {
                if (i.Contains("|"))
                {
                    var t = i.Replace("|", "");
                    var p = t.Split(':');
                    var c = p[0].Split(',');
                    if (c.Length > 1) Console.BackgroundColor = ConsoleColors.ToConsoleColor(c[1]);
                    if (c.Length > 0) Console.ForegroundColor = ConsoleColors.ToConsoleColor(c[0]);

                    Console.Write(un(p[1]));
                }
                else
                {
                    Console.ResetColor();
                    Console.Write(un(i));
                }
            }
        }
    }
}