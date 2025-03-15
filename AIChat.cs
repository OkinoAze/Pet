using Godot;
using System;

public partial class AIChat : HttpRequest
{
    String text = "1+1";
    public override void _Ready()
    {
        RequestCompleted += OnRequestCompleted;
        //CallDeepSeekApi();
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
            ""content"": """ + text + @"""
            }
        ],
        ""temperature"": 0.7,
        ""max_tokens"": 100
        }";

        Request(url, headers, HttpClient.Method.Post, requestBody);
    }
    public override void _Process(double delta)
    {

    }

    private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        var json = new Json();
        json.Parse(body.GetStringFromUtf8());
        var response = json.Data.AsGodotDictionary();
        GD.Print(response["choices"].AsGodotArray()[0].AsGodotDictionary()["message"].AsGodotDictionary()["content"].AsString());

    }
}
