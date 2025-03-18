using Godot;
using System;
public partial class AudioPlayer : AudioStreamPlayer
{
    [Export]
    Curve AudioCurve; // 曲线
    AudioStreamGeneratorPlayback PlayBack;// 生成器回放
    float SampleHz;//采样率
    float PulseHz = 440;//正弦波频率 A4
    double Phase = 0;//相位，用于计算正弦波
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
            // 计算正弦波值
            var data = (float)Mathf.Sin(Phase * Mathf.Tau);

            // 使用曲线来调整振幅，这里曲线已经归一化到0到1之间
            float curveValue = AudioCurve.Sample((float)Phase); // 根据相位获取曲线值
            data *= curveValue; // 调整正弦波振幅

            // 将数据推送到缓冲区
            PlayBack.PushFrame(Vector2.One * data);

            // 更新相位
            Phase = Mathf.PosMod(Phase + increment, 1.0);
        }
    }

    public override void _Process(double delta)
    {
        if (Main.Instance.KeyboardPlay)
        {
            if (Input.IsKeyPressed(Key.H))
            {
                PlayAudio('A');
            }
            if (Input.IsKeyPressed(Key.J))
            {
                PlayAudio('B');

            }
            if (Input.IsKeyPressed(Key.A))
            {
                PlayAudio('C');
            }
            if (Input.IsKeyPressed(Key.S))
            {
                PlayAudio('D');
            }
            if (Input.IsKeyPressed(Key.D))
            {
                PlayAudio('E');
            }
            if (Input.IsKeyPressed(Key.F))
            {
                PlayAudio('F');
            }
            if (Input.IsKeyPressed(Key.G))
            {
                PlayAudio('G');
            }
        }
    }
    public void PlayAudio(char s)
    {
        float[] frequencies =
        [
            440.0f, // A
            493.8f, // B
            261.6f, // C
            293.6f, // D
            329.6f, // E
            349.2f, // F
            392.0f  // G
        ];
        if (s - 'A' < frequencies.Length)
        {
            PulseHz = frequencies[s - 'A'];
        }
        FillBuffer();
    }
}


