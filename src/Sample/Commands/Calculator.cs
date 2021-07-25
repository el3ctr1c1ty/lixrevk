using Lirxe;
using Lirxe.Model;

namespace Sample.Commands
{
    public class Calculator:BaseController
    {
        public Calculator(ActionContext ctx) : base(ctx) { }
        
        [Action("[a] + [b]")]
        [Action("/add [a] [b]")]
        public void Add(float a, float b) => Send($"Result is {a + b}");
        
        [Action("[a] - [b]")]
        [Action("/subtract [a] [b]")]
        [Action("/sub [a] [b]")]
        public void Sub(float a, float b) => Send($"Result is {a - b}");
        
        [Action("[a] * [b]")]
        [Action("/multiply [a] [b]")]
        [Action("/multi [a] [b]")]
        public void Multi(float a, float b)=> Send($"Result is {a * b}");
        
        [Action("[a] / [b]")]
        [Action("/divide [a] [b]")]
        [Action("/div [a] [b]")]
        public void Div(float a, float b)=> Send($"Result is {a / b}");
        
        [Action("[a] % [b]")]
        [Action("/modulo [a] [b]")]
        [Action("/mod [a] [b]")]
        public void Mod(float a, float b)=> Send($"Result is {a % b}");
    }
}