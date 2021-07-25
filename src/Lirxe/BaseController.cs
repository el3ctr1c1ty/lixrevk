using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lirxe.Model;
using VkNet;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace Lirxe
{
    public abstract class BaseController
    {
        public ActionContext Context;

        public BaseController(ActionContext ctx)
        {
            Context = ctx;
        }

        public VkApi Vk => Context.Vk;
        public Message Message => Context.Message;
        public Payload Payload => Context.Payload;
        public User Sender => Context.Sender;

        public async Task<Message> Prompt(string cancelKey = null, System.Action onCancel = null) =>
            await Context.Prompts.PromptAsync(Context.SenderId, cancelKey, onCancel);

        public void Prompt(Action<Message, PromptProvider> handler, string cancelKey = null, System.Action onCancel = null)
            => Context.Prompts.Prompt(handler, Context.SenderId, cancelKey, onCancel);
        
        public void Send(string msg) => Context.SendMessage(msg);
        public void SendSticker(int id) => Context.SendSticker(id);

        public void SendMessage(string msg, MessageKeyboard keyboard) => Context.SendMessage(msg, keyboard);
        public void SendMessage(string msg, MessageKeyboard keyboard, IEnumerable<MediaAttachment> attachment)
            => Context.SendMessage(msg, keyboard, attachment);

        public void SendMessage(string msg, long peerId, MessageKeyboard? keyboard,
            IEnumerable<MediaAttachment>? attachment)
            => Context.SendMessage(msg, peerId, keyboard, attachment);
    }
}