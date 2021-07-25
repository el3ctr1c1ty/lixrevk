using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Model;

namespace Lirxe
{
    public class PromptInfo
            {
                public long Owner { get; set; }
                public Action<Message, PromptProvider> Handler { get; set; }
                public string CancelKey { get; set; }
                public Guid Id { get; set; }
                public bool IsButton { get; set; }
                public System.Action OnCancel { get; set; }
                public PromptProvider Provider { get; set; }

                public bool Valid(string payload)
                {
                    if (!IsButton) return true;
                    
                    else
                    {
                        var myPayload = "{" + $"\"-\":[\"{Id:N}\"]" + "}";
                        return payload == myPayload;
                    }
                }
            }
        
            public class PromptProvider
            {
                private PromptStore _store;
                private Guid _id;
        
                public PromptProvider(PromptStore store, Guid id)
                {
                    _id = id;
                    _store = store;
                }
        
                public void Cancel() => _store.RemovePrompt(_id);
            }
            public class PromptStore
            {
                private Runner _runner;
                private List<PromptInfo> _pending = new();
        
                public IEnumerable<PromptInfo> Prompts => _pending.ToArray(); 
        
                public PromptStore(Runner current)
                {
                    _runner = current;
                }
        
                public void RemovePrompt(Guid id)
                {
                    _pending.RemoveAll(r => r.Id == id);
                }
                

                public void Prompt(Action<Message, PromptProvider> handler, long owner, string cancelKey = null, System.Action onCancel = null)
                {
                    _pending.RemoveAll(r => r.Owner == owner);
        
                    var guid = Guid.NewGuid();
                    _pending.Add(new PromptInfo
                    {
                        Owner = owner, Handler = handler, CancelKey = cancelKey, Id = guid,
                        Provider = new PromptProvider(this, guid), OnCancel = onCancel
                    });
                }

                public string ButtonPrompt(Action<Message, PromptProvider> handler, long owner)
                {
                    var guid = Guid.NewGuid();
                    _pending.Add(new PromptInfo
                    {
                        Owner = owner, Handler = handler, Id = guid,
                        Provider = new PromptProvider(this, guid), IsButton = true
                    });
                    return "{"+$"\"-\":[\"{guid:N}\"]"+"}";
                }
        
                public async Task<Message> PromptAsync(long owner, string cancelKey = null, System.Action onCancel = null)
                {
                    Message result = null;
                    Prompt((r,_) => result = r, owner, cancelKey, onCancel);
        
                    await Task.Run(() =>{while (result == null) ;});
                    return result;
                }
            }
    }