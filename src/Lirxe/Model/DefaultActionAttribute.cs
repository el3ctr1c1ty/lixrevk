using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lirxe.Model
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DefaultAction : Attribute
    {
    }
}