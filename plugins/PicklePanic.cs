// name: Pickle Panic
// author: Arre
// description: A giant pile of pickles chases you around the map. It gets faster the longer you survive. Don't get pickled!
using System;
using IsIT.Modding;

public class PicklePanic : IGamePlugin
{
    Vec3 face;
    bool placed;
    float time, cooldown, heartbeat, survived, best;
    int caught;
    bool down;

    public void OnLoad()
    {
        caught = (int)StorageAPI.GetNumber("caught", 0f);
        best = StorageAPI.GetNumber("best", 0f);
        HudAPI.Chat("RUN! The pickles are coming for you...");
        HudAPI.SetLabel("score", "Pickled " + caught + " times   •   Best: " + best.ToString("0") + " s");
        GameEvents.OnRespawned += () => placed = false;
    }

    public void OnUpdate(float deltaTime)
    {
        time += deltaTime;
        Vec3 me = MeAPI.Position + new Vec3(0f, 1.4f, 0f);
        if (!placed)
        {
            // Starts behind you, a fair way off.
            face = me - MeAPI.Forward * 18f + new Vec3(0f, 1f, 0f);
            placed = true;
            survived = 0f;
        }

        Vec3 to = me - face;
        float distance = to.Length;
        // Run too far and it catches up in a blink.
        if (distance > 45f) { face = me - to.Normalized * 25f; to = me - face; distance = to.Length; }

        if (cooldown > 0f)
        {
            cooldown -= deltaTime;
            // Back on your feet after the scare.
            if (down && cooldown < 1.5f) { MeAPI.Ragdoll(false); down = false; }
        }
        else
        {
            survived += deltaTime;
            // It speeds up the longer you get away (but you can always outrun it by sprinting).
            float speed = Math.Min(3f + survived * 0.06f, 6.5f);
            face = face + to.Normalized * Math.Min(speed * deltaTime, distance);
        }

        // It bobs and wobbles as it comes.
        Vec3 bob = new Vec3(0f, (float)Math.Sin(time * 6f) * 0.18f, 0f);
        SpriteAPI.Show("face", "pickles", face + bob, 2.6f);

        HudAPI.SetBar("near", distance < 6f ? "THE PICKLES ARE RIGHT BEHIND YOU!" : "Pickles " + distance.ToString("0") + " m away   •   " + survived.ToString("0") + " s", 1f - Math.Min(1f, distance / 30f));

        // A heartbeat that speeds up as it gets close.
        heartbeat -= deltaTime;
        if (distance < 12f && heartbeat <= 0f)
        {
            SoundAPI.Tone(55f, 0.12f);
            heartbeat = 0.25f + distance * 0.06f;
        }

        if (distance < 1.6f && cooldown <= 0f)
        {
            caught++;
            if (survived > best) { best = survived; StorageAPI.SetNumber("best", best); }
            StorageAPI.SetNumber("caught", caught);
            HudAPI.Big("PICKLED!");
            SoundAPI.Tone(110f, 0.5f);
            CameraAPI.Shake(0.8f);
            MeAPI.Ragdoll(true);
            down = true;
            cooldown = 3f;
            HudAPI.SetLabel("score", "Pickled " + caught + " times   •   Best: " + best.ToString("0") + " s");
            // It backs off for a new chase.
            face = me - MeAPI.Forward * 20f + new Vec3(0f, 1f, 0f);
            survived = 0f;
        }
    }
}
