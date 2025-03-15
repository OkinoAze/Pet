using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Main : Node
{
    public bool Dragging = false;
    public bool RandomMove = false;
    public bool FollowMouse = false;
    public float ClickThroughTime = 5f;
    public Vector2I WindowSize = new Vector2I(24, 24);
    public int Scale = 1;
    public int FullScale = 1;
    public Color Color1 = Colors.White;
    public Color Color2 = Colors.White;
    public Vector2 TaskBarSize = Vector2.Zero;//任务栏大小
    public Vector2 TaskBarPos = Vector2.Zero;//任务栏位置

    public Vector2 ForegroundWindowPos = -Vector2.One;
    public Vector2 ForegroundWindowSize = -Vector2.One;
    public static Main Instance { get; private set; }
    Main()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        var screenSize = DisplayServer.ScreenGetSize(0);
        FullScale = screenSize.X / WindowSize.X < screenSize.Y / WindowSize.Y ? screenSize.X / WindowSize.X - 1 : screenSize.Y / WindowSize.Y - 1;
        Load();
    }
    public void ChangeWindowSize(int n)
    {
        if (Scale + n >= FullScale)
        {
            Scale = FullScale;
        }
        else if (Scale + n > 1)
        {
            Main.Instance.Scale += n;

        }
        else
        {
            Main.Instance.Scale = 1;
        }
        var startSize = GetWindow().Size;


        GetWindow().Size = WindowSize * Scale;
        GetWindow().Position -= (GetWindow().Size - startSize) / 2;
    }
    public void Save()
    {
        ConfigFile file = new();
        file.SetValue("Window", "position", GetWindow().Position);
        file.SetValue("Window", "scale", GetWindow().Size);
        file.SetValue("Settings", "Color1", Color1);
        file.SetValue("Settings", "Color2", Color2);
        file.SetValue("Settings", "RandomMove", RandomMove);
        file.SetValue("Settings", "FollowMouse", FollowMouse);



        file.Save("user://config.cfg");
    }
    public void Load()
    {
        ConfigFile file = new();
        var error = file.Load("user://config.cfg");
        if (error == Error.Ok)
        {
            GetWindow().Size = file.GetValue("Window", "scale", WindowSize * Scale).AsVector2I();
            GetWindow().Position = file.GetValue("Window", "position", DisplayServer.ScreenGetPosition(0) + DisplayServer.ScreenGetSize(0) / 2 - GetWindow().Size / 2).AsVector2I();
            Color1 = file.GetValue("Settings", "Color1", Colors.White).AsColor();
            Color2 = file.GetValue("Settings", "Color2", Colors.White).AsColor();
            RandomMove = file.GetValue("Settings", "RandomMove", false).AsBool();
            FollowMouse = file.GetValue("Settings", "FollowMouse", false).AsBool();
        }

    }

}
