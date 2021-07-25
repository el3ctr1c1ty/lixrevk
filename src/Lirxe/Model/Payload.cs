using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lirxe.Model
{
    public class Payload
    {
        [JsonProperty("_")]
        public string Action { get; set; }
        [JsonProperty(".")]
        public IDictionary<string,object> Store { get; set; }

        public Payload(string act)
        {
            Action = act;
        }

        
        public Payload()
        {
        }

        public Payload(string act, IDictionary<string, object> di)
        {
            Action = act;
            Store = di;
        }
        public static Payload Deserialize(string content)
        {
            if (content.Contains("{") || content.Contains("[")) return JsonConvert.DeserializeObject<Payload>(content);
            //else return new Payload(content);
            else throw new ArgumentException("VK does not support non-json payload", nameof(content));
        }

        public string Serialize() => JsonConvert.SerializeObject(this);
        public override string ToString() => Serialize();

        public static implicit operator string(Payload p) => p.Serialize();
       // public static explicit operator string(Payload p) => p.Serialize();
    }
}