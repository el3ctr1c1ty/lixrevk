using System;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.Keyboard;

namespace Lirxe
{
    public class PromptKeyboardButton:MessageKeyboardButton
    {
        public PromptKeyboardButton() : base()
        {
        }

        public PromptKeyboardButton(PromptStore s, long owner, Action<Message, PromptProvider> onClick) : base()
        {
            this.Action = new MessageKeyboardButtonAction();
            Action.Payload = s.ButtonPrompt(onClick, owner);
            Action.Type = KeyboardButtonActionType.Text;
        }
        public PromptKeyboardButton(string label, PromptStore s, long owner, Action<Message, PromptProvider> onClick) : base()
        {
            this.Action = new MessageKeyboardButtonAction();
            Action.Label = label;
            Action.Payload = s.ButtonPrompt(onClick, owner);
            Action.Type = KeyboardButtonActionType.Text;
        }
    }
}