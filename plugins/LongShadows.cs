// name: Long Shadows
// author: Arre
// description: Shadows drawn further away, a longer view, and a hello when it starts.
using IsIT.Modding;

public class LongShadows : IGamePlugin
{
    float time;

    public void OnLoad()
    {
        GraphicsAPI.Shadows = true;
        GraphicsAPI.ShadowDistance = 120f;
        GraphicsAPI.RenderDistance = 900f;
        UIAPI.Notify("Long Shadows", "Shadows reach 120 m now. Have fun!");
    }

    public void OnUpdate(float deltaTime)
    {
        time += deltaTime;
        if (time > 600f)
        {
            time = 0f;
            UIAPI.Notify("Long Shadows", "Ten minutes of play. Drink some water!");
        }
    }
}
