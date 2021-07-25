using System;
using Lirxe.Logging;
using Lirxe.Model;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Lirxe
{
    public class LongpoolSource:Source
    {
        private VkApi _client;
        private ulong _id;
        private Logger l;
        public LongpoolSource(VkApi client, ulong pubId, Logger log = null) : base()
        {
            l = log ?? new ConsoleLogger(); _client = client; _id = pubId; }

        public override event RequestEvent Request;
        public override event EventHandler OnRun;

        protected override void Run(){
            
                l.i($"Longpool source running, public id - {_id}\n");
                var s = _client.Groups.GetLongPollServer(_id);
                OnRun?.Invoke(this, EventArgs.Empty);
                while (true)
                {
                    try
                    {
                        var poll = _client.Groups.GetBotsLongPollHistory(
                            new BotsLongPollHistoryParams()
                                {Server = s?.Server, Ts = s?.Ts, Key = s?.Key, Wait = 25});

                        s.Ts = poll.Ts;
                        if (poll?.Updates == null) continue;
                        foreach (var a in poll.Updates)
                        {
                            if (a.Type != GroupUpdateType.MessageNew) continue;
                            var ac = new ActionContext
                            {
                                Message = a.MessageNew.Message, Sender = new User {Id = (long) a.MessageNew.Message.PeerId},
                                Vk = _client
                            };
                            if (!string.IsNullOrEmpty(a.MessageNew.Message.Payload))
                                ac.Payload = Payload.Deserialize(a.MessageNew.Message.Payload);
                            Request?.Invoke(ac);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        l.e(e.ToString());
                    }
                }
            

        }
    }
}