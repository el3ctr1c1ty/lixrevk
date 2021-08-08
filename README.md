# LirxeVK
Lirxe provides a lightweight and MVC-like way to build your chatbot application using .NET Core.
[Russian readme is here](README.RU.md)

## Getting started
The base idea of Lirxe is a *source* (or listener) getting events should be passed to *runner* which executes an **action**, or command inside a controller. 

```
private static async Task Main(string[] args)
{
    var bot = new
    {
        AccessToken = "ACCESS_TOKEN",
        PublicId = (ulong)1
    };
    
    
    var vk = new VkApi();
    vk.Authorize(new ApiAuthParams(){AccessToken = bot.AccessToken});
    var source = new LongpoolSource(vk, bot.PublicId);
    var run = new Runner(new[] {Assembly.GetExecutingAssembly()});
    source.Request += ctx => run.Run(ctx);
    await source.RunAsync();
}
``` 
Here we are using a `LongpoolSource` to get events from VK LongPool API (make sure it's enabled in your group's settings), then pass these to `Runner`, which will execute an action.



**Actions** are living inside Controllers. This is an example:
```
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
}
```
There is the `Default action` being executed when no actions are matching. You can be creative for putting any "Command not found" text here, or usage help, or maybe just ignore it. 

`Action` attribute is used for defining method as actual action. In this example, it just asks the user about hyd using `Prompt` (i will explain it later), then returns his input (or "sad" when it cancels prompt using "no" word)

## Some more documentation
Comming soon.