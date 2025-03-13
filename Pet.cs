using Godot;
using System;


public partial class Pet : Node2D
{

    float Speed = 2;
    bool EnterEnd = false;
    int StateID = 0;
    readonly IState[] States = new IState[Enum.GetNames(typeof(State)).Length];
    Vector2 Direction = Vector2.Zero;
    Sprite2D Sprite;
    AnimationTree AnimationTree;
    AnimationNodeStateMachinePlayback StateMachine;

    Vector2I ScreenSize = Vector2I.Zero;
    Vector2I ScreenPosition = Vector2I.Zero;
    Vector2 MouseStartPosition = Vector2.Zero;
    Vector2 MousePosition = Vector2.Zero;

    Window Window;

    Timer Timer;
    Vector2I GotoPosition = Vector2I.Zero;
    public enum State
    {
        Idle,
        Run,
        FlyIdle,
        Fly
    }
    interface IState
    {

        public int GetId { get; }

        public bool Enter()
        {
            return true;
        }
        public int Update(double delta)
        {
            return Exit();
        }
        public int Exit()
        {
            return GetId;
        }

    }
    public void SwitchState(State id, bool enter = false)
    {
        EnterEnd = enter;
        StateID = (int)id;
    }
    public override void _Ready()
    {
        _ = new StateIdle(this, States);
        _ = new StateRun(this, States);
        _ = new StateFlyIdle(this, States);

        //GetTree().Root.MousePassthroughPolygon = GetNode<Polygon2D>("%WindowPolygon").Polygon;

        Window = GetNode<Window>("%PopWindow");

        Window.CloseRequested += () =>
        {
            Window.Visible = false;
        };
        ScreenSize = DisplayServer.ScreenGetSize(0);
        ScreenPosition = DisplayServer.ScreenGetPosition(0);
        Sprite = GetNode<Sprite2D>("Sprite2D");
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        StateMachine = AnimationTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
        Timer = GetNode<Timer>("Timer");

        ColorChange();
    }

    void ColorChange()
    {
        Color color1 = Main.Instance.Color1;
        Color color2 = Main.Instance.Color2;
        (Sprite.Material as ShaderMaterial).SetShaderParameter("replacement_color1", new Vector3(color1.R, color1.G, color1.B));
        (Sprite.Material as ShaderMaterial).SetShaderParameter("replacement_color2", new Vector3(color2.R, color2.G, color2.B));

    }
    public override void _Process(double delta)
    {
        Direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (EnterEnd == false)
        {
            EnterEnd = States[StateID].Enter();
        }
        var id = States[StateID].Update(delta);
        if (id != StateID)
        {
            EnterEnd = false;
            StateID = id;
        }
        if (Window.Visible)
        {
            ColorChange();
        }

    }

    public override void _Input(InputEvent @event)
    {

        if (@event is InputEventMouseButton)
        {
            if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.Left && (@event as InputEventMouseButton).Pressed)
            {
                MouseStartPosition = GetViewport().GetMousePosition();
                Main.Instance.Dragging = true;
            }
            else
            {
                Main.Instance.Dragging = false;

            }
            if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.Right && (@event as InputEventMouseButton).Pressed)
            {
                if (Window.Visible == false)
                {
                    Window.Visible = true;
                    Window.Position = DisplayServer.MouseGetPosition();
                }
                else
                {
                    Window.Visible = false;
                }
            }


            if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.Middle && (@event as InputEventMouseButton).Pressed)
            {
                Main.Instance.Save();
                GetTree().Quit();
            }
            if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.WheelUp)
            {
                Main.Instance.ChangeWindowSize(-1);

            }
            else if ((@event as InputEventMouseButton).ButtonIndex == MouseButton.WheelDown)
            {

                Main.Instance.ChangeWindowSize(1);

            }

        }
        if (@event is InputEventMouseMotion && Main.Instance.Dragging == true)
        {
            MousePosition = GetViewport().GetMousePosition();
            GetWindow().Position = GetWindow().Position += (Vector2I)(MousePosition - MouseStartPosition);
        }
    }
    private partial class StateIdle : Node, IState
    {
        Pet this2D;
        public int GetId { get; } = (int)State.Idle;
        public StateIdle(Pet pet, IState[] states)
        {
            this2D = pet;
            states[GetId] = this;
        }
        public bool Enter()
        {
            this2D.StateMachine.Travel("Idle");
            return true;
        }

        public int Update(double delta)
        {
            return Exit();
        }
        public int Exit()
        {
            if (this2D.Direction != Vector2.Zero)
            {
                return (int)State.Run;
            }
            if (Main.Instance.Dragging)
            {
                return (int)State.FlyIdle;
            }
            return GetId;
        }
    }
    private partial class StateRun : Node, IState
    {
        Pet this2D;
        public int GetId { get; } = (int)State.Run;
        public StateRun(Pet pet, IState[] states)
        {
            this2D = pet;
            states[GetId] = this;
        }
        public bool Enter()
        {
            this2D.StateMachine.Travel("Run");
            return true;
        }

        public int Update(double delta)
        {
            Vector2 velocity = this2D.Direction * this2D.Speed;
            if (this2D.Direction.X < 0)
            {
                this2D.Sprite.FlipH = true;
            }
            else if (this2D.Direction.X > 0)
            {
                this2D.Sprite.FlipH = false;
            }
            var p = this2D.GetWindow().Position + (Vector2I)velocity - DisplayServer.ScreenGetPosition(0);
            this2D.GetWindow().Position = this2D.ScreenPosition + new Vector2I(Mathf.Clamp(p.X, 0, this2D.ScreenSize.X - this2D.GetWindow().Size.X), Mathf.Clamp(p.Y, 0, this2D.ScreenSize.Y - this2D.GetWindow().Size.Y));
            return Exit();
        }
        public int Exit()
        {
            if (Main.Instance.Dragging || this2D.Direction == Vector2.Zero)
            {
                return (int)State.Idle;
            }
            return GetId;
        }
    }
    private partial class StateFlyIdle : Node, IState
    {
        Pet this2D;
        public int GetId { get; } = (int)State.FlyIdle;
        public StateFlyIdle(Pet pet, IState[] states)
        {
            this2D = pet;
            states[GetId] = this;
        }
        public bool Enter()
        {
            this2D.StateMachine.Travel("FlyIdle");
            return true;
        }

        public int Update(double delta)
        {
            return Exit();
        }
        public int Exit()
        {
            if (Main.Instance.Dragging == false)
            {
                return (int)State.Idle;
            }

            return GetId;
        }
    }

}
