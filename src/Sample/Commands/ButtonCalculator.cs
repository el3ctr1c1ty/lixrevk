using System.Threading.Tasks;
using Lirxe;
using Lirxe.Model;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;

namespace Sample.Commands
{
    public class ButtonCalculator:BaseController
    {
        public ButtonCalculator(ActionContext ctx) : base(ctx) { }

        [Action(payloadCommand: "Add")]
        public async Task Add()
        {
            Send($"Enter first number");
            var a = float.Parse((await Prompt()).Text);
            Send($"Enter second number");
            var b = float.Parse((await Prompt()).Text);

            SendMessage($"Result is {a+b}", new MessageKeyboard{Buttons = new[]{new[]
            {
                new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction{Payload = new Payload("Calc"), Label = $"Count something more", Type = KeyboardButtonActionType.Text}, Color = KeyboardButtonColor.Default
                }
            }}});
        }
        
        [Action(payloadCommand: "Sub")]
        public async Task Sub()
        {
            Send($"Enter first number");
            var a = float.Parse((await Prompt()).Text);
            Send($"Enter second number");
            var b = float.Parse((await Prompt()).Text);

            SendMessage($"Result is {a-b}", new MessageKeyboard{Buttons = new[]{new[]
            {
                new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction{Payload = new Payload("Calc"), Label = $"Count something more", Type = KeyboardButtonActionType.Text}, Color = KeyboardButtonColor.Default
                }
            }}});
        }
        
        [Action(payloadCommand: "Multi")]
        public async Task Multi()
        {
            Send($"Enter first number");
            var a = float.Parse((await Prompt()).Text);
            Send($"Enter second number");
            var b = float.Parse((await Prompt()).Text);

            SendMessage($"Result is {a*b}", new MessageKeyboard{Buttons = new[]{new[]
            {
                new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction{Payload = new Payload("Calc"), Label = $"Count something more", Type = KeyboardButtonActionType.Text}, Color = KeyboardButtonColor.Default
                }
            }}});
        }
        
        [Action(payloadCommand: "Div")]
        public async Task Div()
        {
            Send($"Enter first number");
            var a = float.Parse((await Prompt()).Text);
            Send($"Enter second number");
            var b = float.Parse((await Prompt()).Text);

            SendMessage($"Result is {a/b}", new MessageKeyboard{Buttons = new[]{new[]
            {
                new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction{Payload = new Payload("Calc"), Label = $"Count something more", Type = KeyboardButtonActionType.Text}, Color = KeyboardButtonColor.Default
                }
            }}});
        }
        
        [Action(payloadCommand: "Mod")]
        public async Task Mod()
        {
            Send($"Enter first number");
            var a = float.Parse((await Prompt()).Text);
            Send($"Enter second number");
            var b = float.Parse((await Prompt()).Text);

            SendMessage($"Result is {a%b}", new MessageKeyboard{Buttons = new[]{new[]
            {
                new MessageKeyboardButton()
                {
                    Action = new MessageKeyboardButtonAction{Payload = new Payload("Calc"), Label = $"Count something more", Type = KeyboardButtonActionType.Text}, Color = KeyboardButtonColor.Default
                }
            }}});
        }

        [Action("calculate")]
        [Action("calc")]
        [Action(payloadCommand:"Calc")]
        public void Calculate() =>
            SendMessage("Use the keyboard.", new MessageKeyboard()
            {
                Buttons = new[]
                {
                    new[]
                    {
                        new MessageKeyboardButton
                        {
                            Action = new MessageKeyboardButtonAction {Payload = new Payload("Add"), Label = "(+) Add", Type = KeyboardButtonActionType.Text},
                            Color = KeyboardButtonColor.Primary
                        },
                        new MessageKeyboardButton
                        {
                            Action = new MessageKeyboardButtonAction
                                {Payload = new Payload("Sub"), Label = "(-) Subtract", Type = KeyboardButtonActionType.Text},
                            Color = KeyboardButtonColor.Default
                        }
                    },
                    new[]
                    {
                        new MessageKeyboardButton
                        {
                            Action = new MessageKeyboardButtonAction
                                {Payload = new Payload("Multi"), Label = "(*) Multiply", Type = KeyboardButtonActionType.Text},
                            Color = KeyboardButtonColor.Default
                        },
                        new MessageKeyboardButton
                        {
                            Action = new MessageKeyboardButtonAction
                                {Payload = new Payload("Div"), Label = "(/) Divide", Type = KeyboardButtonActionType.Text},
                            Color = KeyboardButtonColor.Default
                        },
                        new MessageKeyboardButton
                        {
                            Action = new MessageKeyboardButtonAction
                                {Payload = new Payload("Div"), Label = "(%) Modulo", Type = KeyboardButtonActionType.Text},
                            Color = KeyboardButtonColor.Default
                        }
                    }
                }
            });
    }
}