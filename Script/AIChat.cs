using Godot;
using System;

public partial class AIChat : HttpRequest
{
    [Export]
    TextEdit Edit;
    [Export]
    RichTextLabel Label;
    [Export]
    Button Button;

    bool Loading = false;

    public override void _Ready()
    {
        RequestCompleted += OnRequestCompleted;
        Button.Pressed += CallDeepSeekApi;
    }
    private void CallDeepSeekApi()
    {
        string url = "https://api.deepseek.com/v1/chat/completions";
        string apiKey = "sk-58a7183093a345aea3b7dfdf0072dd18";

        string[] headers =
         [
            "Content-Type: application/json",
            $"Authorization: Bearer {apiKey}"
         ];

        string requestBody =
        @"{
        ""model"": ""deepseek-chat"",
        ""messages"": [
            {
            ""role"": ""user"",
            ""content"": """ + Edit.Text + @"""
            }
        ],
        ""temperature"": 0.7,
        ""max_tokens"": 100
        }";
        if (!Loading && Edit.Text != "")
        {
            Loading = true;
            Label.Text = "Loading...";
            Edit.Text = "";
            Request(url, headers, HttpClient.Method.Post, requestBody);
        }
    }
    public override void _Process(double delta)
    {

    }

    private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        var json = new Json();
        json.Parse(body.GetStringFromUtf8());
        var response = json.Data.AsGodotDictionary();
        if (response.Count > 0)
        {
            Label.Text = response["choices"].AsGodotArray()[0].AsGodotDictionary()["message"].AsGodotDictionary()["content"].AsString();
        }
        else
        {
            Label.Text = "暂时无法回复";
        }
        Loading = false;
    }
}
