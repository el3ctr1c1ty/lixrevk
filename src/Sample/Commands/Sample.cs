using Lirxe;
using Lirxe.Model;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Keyboard;
using Action = Lirxe.Action;

namespace Sample.Commands
{
    public class Controller:BaseController
    {

        public Controller(ActionContext ctx) : base(ctx) { }
        [DefaultAction]
        public void Default() => Send("This is default action");

        [Action("hello")]
        public async void Hello()
        {
            Send("How you doing?");
            Send($"You doing {(await Prompt("no", ()=>{Send("sad"); Default();})).Text}");
        }
        
        [Action("hyd")]
        public async void HelloBtn()
        {
            var kbd = new MessageKeyboard
            {
                Buttons = new[]
                {
                    new[]
                    {
                        new PromptKeyboardButton("Good", Context.Prompts, Context.SenderId,
                            (_, _) => Send($"You doing Good :)")) {Color = KeyboardButtonColor.Positive},
                        new PromptKeyboardButton("Bad", Context.Prompts, Context.SenderId,
                            (_, _) => Send($"You doing Bad :(")) {Color = KeyboardButtonColor.Negative},
                    }
                }
            };
            SendMessage("How you doing?", kbd);
        }
    }
}












