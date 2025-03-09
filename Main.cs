using Godot;
using System;

public partial class Main : Node
{
    public static Main Instance { get; private set; }
    Main()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        Load();
    }
    public void Save()
    {
        ConfigFile file = new();
        file.SetValue("window", "position", GetWindow().Position);
        file.SetValue("window", "size", GetWindow().Size);
        file.Save("user://config.cfg");
    }
    public void Load()
    {
        ConfigFile file = new();
        var error = file.Load("user://config.cfg");
        if (error == Error.Ok)
        {
            GetWindow().Position = file.GetValue("window", "position", DisplayServer.ScreenGetPosition(0) + DisplayServer.ScreenGetSize(0) / 2 - GetWindow().Size / 2).AsVector2I();
            GetWindow().Size = file.GetValue("window", "size", new Vector2I(24, 24)).AsVector2I();
        }

    }

}
