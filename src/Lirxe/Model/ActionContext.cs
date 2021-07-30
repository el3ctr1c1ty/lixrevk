using System;
using System.Collections.Generic;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
// ReSharper disable PossibleInvalidOperationException

namespace Lirxe.Model
{

    public static class MessageExtension
    {
        public static bool IsPrivate(this Message msg) => msg.OwnerId == msg.PeerId;
        public static bool IsGroup(this Message msg) => !IsPrivate(msg);
    }
    public class ActionContext
    {
        public User Sender { get; set; }
        public VkNet.VkApi Vk { get; set; }
        public Payload Payload { get; set; }
        public Message Message { get; set; }
        public long SenderId => Message.FromId ?? (long)Message.OwnerId;
        public PromptStore Prompts { get; set; }
        public long PeerId => (long) Message.PeerId;
        
        
        public long SendMessage(string msg) =>
            SendMessage(msg, PeerId, null, null);
        public long SendSticker(int id) =>
            Vk.Messages.Send(new MessagesSendParams()
            {
                PeerId = PeerId, RandomId = DateTime.Now.Millisecond, StickerId = (uint?)id
            });

        
        public void EditMessage(long msgId, string msg) =>
            EditMessage(msgId, msg, null);
        public void EditMessage(long msgId, string msg, MessageKeyboard? keyboard)
            => EditMessage(msgId, msg, keyboard, null);
        public void EditMessage(long msgId, string msg, MessageKeyboard? keyboard,
            IEnumerable<MediaAttachment>? attachment) => Vk.Messages.Edit(new MessageEditParams()
        {
            Message = msg, PeerId = PeerId, Keyboard = keyboard, Attachments = attachment, MessageId = msgId
        });
        

        public long SendMessage(string msg, MessageKeyboard keyboard)
            => SendMessage(msg, PeerId, keyboard, null);
        public long SendMessage(string msg, MessageKeyboard keyboard, IEnumerable<MediaAttachment> attachment)
            => SendMessage(msg, PeerId, keyboard, attachment);
        public long SendMessage(string msg, long peerId, MessageKeyboard? keyboard, IEnumerable<MediaAttachment>? attachment)
            =>
                Vk.Messages.Send(new MessagesSendParams()
                {
                    Message = msg, PeerId = peerId, RandomId = DateTime.Now.Millisecond, Keyboard = keyboard, Attachments = attachment
                });
    }
}