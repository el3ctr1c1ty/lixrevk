using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using Lirxe.Model;

namespace Lirxe
{
    public partial class Runner
    {
        private object[] GetArgsPayload(Payload payload, MethodInfo method, bool throwOn = false)
                {
                    var p = new List<object>();
                    foreach (var param in method.GetParameters())
                    {
                        if (payload.Store.ContainsKey(param.Name))
                            p.Add(payload.Store[param.Name]);
                        else
                        {
                            if (param.HasDefaultValue) p.Add(param.HasDefaultValue);
                            else if (throwOn) throw new ArgumentException($"{param.Name} does not stored in payload and doesn't have default value");
                            else p.Add(null);
                        }
                    }
        
                    return p.ToArray();
                }
        private object[] GetArgsCommand(Match cmdMatch, MethodInfo method, bool throwOn = false)
                {
                    var p = new List<object>();
                    foreach (var param in method.GetParameters())
                    {
                        if (cmdMatch.Groups.ContainsKey(param.Name))
                            p.Add(StringToType(cmdMatch.Groups[param.Name].Value, param.ParameterType));
                        else
                        {
                            if (param.HasDefaultValue) p.Add(param.DefaultValue);
                            else if (throwOn) throw new ArgumentException($"{param.Name} does not stored in payload and doesn't have default value");
                            else p.Add(null);
                        }
                    }

                    return p.ToArray();
                }

        private static bool ArgsValid(object[] args) => args.ToList().Distinct(null).Any();

     public object StringToType(string val, Type type)
     {
         if (type == typeof(int)) return int.Parse(val);
         if (type == typeof(long)) return long.Parse(val);
         if (type == typeof(float)) return float.Parse(val);
         if (type == typeof(byte)) return byte.Parse(val);
         if (type == typeof(bool))
             return val.ToString() != "-" && val.ToString() != "no" && val.ToString() != "0" &&
                    val.ToString() != "нет" && val.ToString() != "false";
         if (type == typeof(string)) return val.ToString();

         return null;
     }
    }
}