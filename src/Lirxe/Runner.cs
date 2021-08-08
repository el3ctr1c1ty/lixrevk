using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Lirxe.Logging;
using Lirxe.Model;

namespace Lirxe
{
    public class RunnableAction
    {
        public Type Controller { get; set; }
        public MethodInfo Method { get; set; }
        public IEnumerable<Action> Signatures { get; set; }
    }
    
    public partial class Runner
    {
        private List<RunnableAction> _actions = new();
        private RunnableAction _defaultAction;
        private Logger l;
        private PromptStore _promptStore;
        private string[] _mentionStrings;
        
        
        public Runner(IEnumerable<Assembly> assemblies, Logger logger = null, string[] mention = null)
        {
            _mentionStrings = mention ?? new string[] { };
            _promptStore = new PromptStore(this);
            l = logger ?? new ConsoleLogger();
            foreach (var assembly in assemblies)
            {
                var controllers = assembly.GetTypes();
                foreach (var controller in controllers)
                {
                    var getMethods = controller.GetMethods();
                    var methods = getMethods.Where(x =>
                        x.GetCustomAttributes(false).OfType<Action>().Any());
                    var defaultMethods = getMethods.Where(x =>
                        x.GetCustomAttributes(false).OfType<DefaultAction>().Any()).ToList();
                    if (defaultMethods.Any())
                        _defaultAction = new RunnableAction()
                            {Controller = controller, Method = defaultMethods.First(), Signatures = null};
                    foreach (var method in methods)
                        _actions.Add(new RunnableAction()
                        {
                            Controller = controller, Method = method,
                            Signatures = method.GetCustomAttributes(false).OfType<Action>()
                        });
                }
                
            }

            if (_defaultAction == null)
            {
                l.w("No default action set");
            }
        }


        public void Run(ActionContext ctx)
        {
            CheckMention(ctx);
            
            var prompts = _promptStore.Prompts.Where(p => p.Owner == ctx.SenderId && p.Valid(ctx.Message.Payload)).ToArray();
            if (prompts.Any()) RunToPrompt(ctx, prompts.First());
            
            else RunToAction(ctx);
        }
        private void RunToPrompt(ActionContext ctx, PromptInfo prompt)
        {
            
            if (prompt.CancelKey != null && ctx.Message.Text == prompt.CancelKey)
            {
                prompt.Provider.Cancel();
                l.i($"Canceled prompt [{prompt.Id:N}]\n");
                if (prompt.OnCancel != null)
                {
                    l.sj($"Executing cancel handler of prompt [{prompt.Id:N}]");
                    Task.Run(()=>prompt.OnCancel.Invoke());
                    l.fj($"Executed cancel handler of prompt [{prompt.Id:N}]     \n");
                }
            }
            else
            {
                l.sj($"Executing prompt [{prompt.Id:N}]");
                Task.Run(()=>prompt.Handler.Invoke(ctx.Message, prompt.Provider));
                if (!prompt.IsButton) prompt.Provider.Cancel();
                l.fj($"Executed prompt [{prompt.Id:N}]     \n");
            }
        }

        private void CheckMention(ActionContext ctx)
        {
            var regx = new Regex($@"\[(club|public){ctx.GroupId}\|.*\]");
            foreach (Match m in regx.Matches(ctx.Message.Text))
            {
                ctx.Mentioned = true;
                ctx.Message.Text = ctx.Message.Text.Replace(m.Value+", ", "");
                ctx.Message.Text = ctx.Message.Text.Replace(m.Value+",", "");
                ctx.Message.Text = ctx.Message.Text.Replace(m.Value+" ", "");
                ctx.Message.Text = ctx.Message.Text.Replace(m.Value, "");
            } ctx.Message.Text = HttpUtility.HtmlDecode(ctx.Message.Text);
            foreach (var str in _mentionStrings)
            {
                if (!ctx.Message.Text.Contains(str)) continue;
                ctx.Mentioned = true;
                ctx.Message.Text = ctx.Message.Text.Replace(str, "");
            }
        }
        private void RunToAction(ActionContext ctx)
        {
            ctx.Prompts = _promptStore;
            
            
            var matching = _actions.Where(ra => ra.Signatures.Any(s =>
                (s.TextPattern.Match(ctx.Message.Text).Success)
                && (s.TextPattern.Match(ctx.Message.Text).Index==0)
                && (s.TextPattern.Match(ctx.Message.Text).Length==ctx.Message.Text.Length)||
                (ctx.Payload != null && s.PayloadCommand != null && s.PayloadCommand == ctx.Payload.Action)));
            if (matching.Any())
            {
                if (matching.Count()>1) l.Warn($"Multiple action matches. Running first of them.\nActions: " +
                                               $"{string.Join(", ",matching.Select(m=>m.Controller.FullName+"."+m.Method.Name))}\n");
                var action = matching.First();
                object[] args;
                if (action.Signatures.Any(r=>r.TextPattern.IsMatch(ctx.Message.Text)))
                    args =
                        GetArgsCommand(action.Signatures.First(r => r.TextPattern.IsMatch(ctx.Message.Text)).TextPattern.Match(ctx.Message.Text),
                            action.Method);
                else if (ctx.Payload != null)
                    args = GetArgsPayload(ctx.Payload, action.Method, true);
                else throw new ArgumentException("No ways to execute this.");
                
                l.sj($"Executing {action.Controller.FullName}.{action.Method.Name}");
                action.Method.Invoke(Activator.CreateInstance(action.Controller, ctx), args);
                l.fj($"Executed {action.Controller.FullName}.{action.Method.Name}\n");
            }
            else
            {
                if (_defaultAction != null)
                {
                    l.sj($"Executing {_defaultAction.Controller.FullName}.{_defaultAction.Method.Name}");
                    _defaultAction.Method.Invoke(Activator.CreateInstance(_defaultAction.Controller, ctx), new object?[]{});
                    l.fj($"Executed {_defaultAction.Controller.FullName}.{_defaultAction.Method.Name}\n");
                }
                
            }
            
        }

        
    }
}