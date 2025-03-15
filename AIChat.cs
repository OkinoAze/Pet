using Godot;
using System;

public partial class AIChat : HttpRequest
{
    public override void _Ready()
    {
        RequestCompleted += OnRequestCompleted;
    }

    private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {

    }

    public override void _Process(double delta)
    {

    }

}
