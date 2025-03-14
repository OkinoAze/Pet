using Godot;
using System;

public partial class Calculator : Control
{
	string inputText = "";
	string formulaText = "";
	double num1 = 0;
	double num2 = 0;
	Label label_output;
	Label label_formula;

	Button btn_percent;
	Button btn_clearInput;
	Button btn_clearAll;
	Button btn_backSpace;
	Button btn_add;
	Button btn_sub;
	Button btn_divide;
	Button btn_multiply;
	Button btn_equal;
	Button btn_dot;
	Button btn_rooting;
	Button btn_square;
	Button btn_backwards;
	Button btn_negation;

	Button btn_0;
	Button btn_1;
	Button btn_2;
	Button btn_3;
	Button btn_4;
	Button btn_5;
	Button btn_6;
	Button btn_7;
	Button btn_8;
	Button btn_9;


	public override void _Ready()
	{
		label_output = GetNode<Label>("%输出");
		label_formula = GetNode<Label>("%公式");

		btn_0 = GetNode<Button>("%按键0");
		btn_1 = GetNode<Button>("%按键1");
		btn_2 = GetNode<Button>("%按键2");
		btn_3 = GetNode<Button>("%按键3");
		btn_4 = GetNode<Button>("%按键4");
		btn_5 = GetNode<Button>("%按键5");
		btn_6 = GetNode<Button>("%按键6");
		btn_7 = GetNode<Button>("%按键7");
		btn_8 = GetNode<Button>("%按键8");
		btn_9 = GetNode<Button>("%按键9");

		btn_percent = GetNode<Button>("%百分号");
		btn_clearInput = GetNode<Button>("%清除输入");
		btn_clearAll = GetNode<Button>("%清除全部");
		btn_backSpace = GetNode<Button>("%退格");
		btn_add = GetNode<Button>("%加号");
		btn_sub = GetNode<Button>("%减号");
		btn_divide = GetNode<Button>("%除号");
		btn_multiply = GetNode<Button>("%乘号");
		btn_equal = GetNode<Button>("%等号");
		btn_dot = GetNode<Button>("%点号");
		btn_rooting = GetNode<Button>("%开方");
		btn_square = GetNode<Button>("%平方");
		btn_backwards = GetNode<Button>("%倒数");
		btn_negation = GetNode<Button>("%取反");

		btn_0.Pressed += OnBtn0Pressed;
		btn_1.Pressed += OnBtn1Pressed;
		btn_2.Pressed += OnBtn2Pressed;
		btn_3.Pressed += OnBtn3Pressed;
		btn_4.Pressed += OnBtn4Pressed;
		btn_5.Pressed += OnBtn5Pressed;
		btn_6.Pressed += OnBtn6Pressed;
		btn_7.Pressed += OnBtn7Pressed;
		btn_8.Pressed += OnBtn8Pressed;
		btn_9.Pressed += OnBtn9Pressed;

		btn_dot.Pressed += OnBtnDotPressed;



		btn_clearAll.Pressed += OnBtnClearAllPressed;
		btn_backSpace.Pressed += OnBtnBackSpacePressed;


	}

	private void OnBtnDotPressed()
	{
		if (inputText.Find(".") == -1)
		{
			inputText += ".";
		}
	}

	private void OnBtn9Pressed()
	{
		inputText += "9";
	}


	private void OnBtn8Pressed()
	{
		inputText += "8";
	}


	private void OnBtn7Pressed()
	{
		inputText += "7";
	}


	private void OnBtn6Pressed()
	{
		inputText += "6";
	}


	private void OnBtn5Pressed()
	{
		inputText += "5";
	}


	private void OnBtn4Pressed()
	{
		inputText += "4";
	}


	private void OnBtn3Pressed()
	{
		inputText += "3";
	}


	private void OnBtn2Pressed()
	{
		inputText += "2";
	}

	private void OnBtn1Pressed()
	{
		inputText += "1";
	}

	private void OnBtn0Pressed()
	{
		if (inputText.Length > 0)
		{
			inputText += "0";
		}
	}
	private void OnBtnBackSpacePressed()
	{
		if (inputText.Length > 0)
		{
			inputText = inputText[..^1];
		}

	}

	private void OnBtnClearAllPressed()
	{
		inputText = "";
	}





	private string OutPutText(string str)
	{
		string str2 = "";
		int dot = str.Find(".");
		if (dot != -1)
		{
			str2 = str[dot..];
			str = str[..dot];
		}
		string result = "";
		int len = (str.Length - 1) / 3;
		int offset = str.Length - len * 3;
		result += str[..offset];
		for (int i = 0; i < len; i++)
		{
			var i2 = offset + i * 3;
			var i3 = i2 + 3;
			result += "," + str[i2..i3];
		}


		return result + str2;
	}

	public override void _Process(double delta)
	{
		if (inputText == "")
		{
			label_output.Text = "0";
		}
		else if (inputText.Length > 15)
		{
			inputText = inputText[..15];
		}
		else
		{
			label_output.Text = OutPutText(inputText);

		}
	}
}
