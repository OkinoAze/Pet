using Godot;
using System;
public partial class AudioPlayer : AudioStreamPlayer
{
    AudioStreamGeneratorPlayback PlayBack;
    float SampleHz;
    float PulseHz = 440;
    double Phase = 0;
    public override void _Ready()
    {
        if (Stream is AudioStreamGenerator generator) // 键入生成器以访问混合率
        {
            SampleHz = generator.MixRate;
            Play();
            PlayBack = (AudioStreamGeneratorPlayback)GetStreamPlayback();
        }
    }
    public void FillBuffer()
    {
        float increment = PulseHz / SampleHz;
        int framesAvailable = PlayBack.GetFramesAvailable();

        for (int i = 0; i < framesAvailable; i++)
        {
            PlayBack.PushFrame(Vector2.One * (float)Mathf.Sin(Phase * Mathf.Tau));
            Phase = Mathf.PosMod(Phase + increment, 1.0);
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_focus_next"))
        {
            FillBuffer();
        }
    }

}

