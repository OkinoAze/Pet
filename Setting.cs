using Godot;
using System;

public partial class Setting : Control
{
    Window Window;
    [Export]
    Button CloseButton;
    [Export]
    ColorPickerButton ColorPickerButton1;
    [Export]
    ColorPickerButton ColorPickerButton2;
    [Export]
    Button RandomMoveButton;
    [Export]
    Button FollowMouseButton;

    public override void _Ready()
    {
        Window = GetNode<Window>("%PopWindow");
        ColorPickerButton1.Color = Main.Instance.Color1;
        ColorPickerButton2.Color = Main.Instance.Color2;
        RandomMoveButton.ButtonPressed = Main.Instance.RandomMove;
        FollowMouseButton.ButtonPressed = Main.Instance.FollowMouse;
        CloseButton.Pressed += () =>
        {
            Window.EmitSignal("close_requested");
        };
        RandomMoveButton.Pressed += () =>
        {
            Main.Instance.RandomMove = RandomMoveButton.ButtonPressed;
            Main.Instance.FollowMouse = false;
            FollowMouseButton.ButtonPressed = Main.Instance.FollowMouse;
        };
        FollowMouseButton.Pressed += () =>
        {
            Main.Instance.FollowMouse = FollowMouseButton.ButtonPressed;
            Main.Instance.RandomMove = false;
            RandomMoveButton.ButtonPressed = Main.Instance.RandomMove;
        };
    }
    public override void _Process(double delta)
    {
        if (Window.Visible)
        {
            Main.Instance.Color1 = ColorPickerButton1.Color;
            Main.Instance.Color2 = ColorPickerButton2.Color;
        }
    }
}
