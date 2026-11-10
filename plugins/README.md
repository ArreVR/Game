# IsIT plugins

Every `.cs` file in this folder shows up in the game's **Mods** window (main menu → MODS).

- A plugin can only be installed after the Creator has read it and approved it in the game. The approval is for that exact
  code: if the file changes, it stops loading until it's approved again.
- Plugins can only use the IsIT mod API (`using IsIT.Modding;`): `PlayerAPI` (speed / jump, only slower on public
  servers), `GraphicsAPI` (shadows, view distance) and `UIAPI` (messages). Files, the internet, other programs, Unity and
  the rest of the game are blocked; a plugin that tries is refused.
- Cheating plugins get the user banned and the plugin removed; whoever made a harmful plugin can be banned forever.

Start the file with:

```csharp
// name: My Plugin
// author: YourName
// description: What it does.
using IsIT.Modding;

public class MyPlugin : IGamePlugin
{
    public void OnLoad() { UIAPI.Notify("My Plugin", "Hello!"); }
    public void OnUpdate(float deltaTime) { }
}
```
