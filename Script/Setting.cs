using Godot;
using System;

public partial class Setting : Control
{
    Window Window;
    Window Window2;
    Window Window3;

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
    [Export]
    Button CalculatorButton;
    [Export]
    Button AIChatButton;
    [Export]
    Button KeyboardPlay;

    public override void _Ready()
    {
        Window = GetNode<Window>("%PopWindow");
        Window2 = GetNode<Window>("%PopWindow2");
        Window3 = GetNode<Window>("%PopWindow3");

        ColorPickerButton1.Color = Main.Instance.Color1;
        ColorPickerButton2.Color = Main.Instance.Color2;
        RandomMoveButton.ButtonPressed = Main.Instance.RandomMove;
        FollowMouseButton.ButtonPressed = Main.Instance.FollowMouse;
        KeyboardPlay.ButtonPressed = Main.Instance.KeyboardPlay;

        Window.CloseRequested += () =>
        {
            Window.Visible = false;
        };

        Window2.CloseRequested += () =>
        {
            Window2.Visible = false;
        };
        Window3.CloseRequested += () =>
        {
            Window3.Visible = false;
        };

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

        KeyboardPlay.Pressed += () =>
        {
            Main.Instance.KeyboardPlay = KeyboardPlay.ButtonPressed;
        };
        CalculatorButton.Pressed += () =>
        {
            Window2.Visible = !Window2.Visible;
        };
        AIChatButton.Pressed += () =>
        {
            Window3.Visible = !Window3.Visible;
        };



    }
    public override void _Process(double delta)
    {
        if (Window.Visible)
        {
            Main.Instance.Color1 = ColorPickerButton1.Color;
            Main.Instance.Color2 = ColorPickerButton2.Color;
            if (Input.IsKeyPressed(Key.Escape))
            {
                Window.Visible = false;
            }
        }
        if (Window3.Visible)
        {
            Main.Instance.KeyboardPlay = false;
            KeyboardPlay.ButtonPressed = Main.Instance.KeyboardPlay;
        }
    }
}
