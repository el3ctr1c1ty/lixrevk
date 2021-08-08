# LirxeVK
Lirxe это простой и легкий способ написать чат-бота с помощью MVC и .NET Core. 
[English documentation is here](README.md)

## Настройка
Все работает в Lirxe с помощью *источника* (source), который достает события которые должны быть направлены *обработчику* (runner), который ищет нужное *действие* (или команду), которая находится внутри контроллера.

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
Здесь `LongpoolSource` получает события от VK LongPool (который должен быть включен в настройках паблика), и эти события мы передаем в `Runner`, который выполняет нужное действие.



**Действия** находятся внутри контроллеров. Например:
```
public class Controller:BaseController
    {

        public Controller(ActionContext ctx) : base(ctx) { }
        [DefaultAction]
        public void Default() => Send("привет ето стандартное действие");

        [Action("привет")]
        public async void Hello()
        {
            Send("Как дела?");
            Send($"у тебя дела {(await Prompt("никак", ()=>{Send("ну ладно"); Default();})).Text}");
        }
}
```
*Действие по умолчанию*, оно выполняется когда обработик не находит ни одного подходящего действия. Сюда можно засунуть текст "Команда не найдена", либо список этих команд, либо просто ничего не делать.

Чтобы назначить метод как действие используем атрибут `Action`. Здесь мы спрашиваем юзера как у него дела с помощью *промта* (о нем расскажу потом) и отвечаем ему тем-же что он ответил, либо "ну ладно" в случае если промпт был отменен словом "никак".

## Подробная документация
потом сделаю