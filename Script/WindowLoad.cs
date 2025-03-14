using System.Threading.Tasks;
using Godot;

public partial class WindowLoad : Node
{

	SetWindowStyle _mouseClickThrough;

	public override void _Ready()
	{
		//获取根节点
		_mouseClickThrough = GetNode("/root/Node") as SetWindowStyle;

	}

	public override async void _Process(double delta)
	{
		DetectPassThrough();
		await ToSignal(GetTree().CreateTimer(Main.Instance.ClickThroughTime), "timeout");
	}
	//设置鼠标可穿透的最小透明度
	const double Alpha = 0.5d;


	/// <summary>
	/// 该函数决定程序是否能被鼠标穿透
	/// </summary>
	private void DetectPassThrough()
	{
		//获取纹理大小
		var viewport = GetViewport();
		var img = viewport.GetTexture().GetImage();
		var rect = viewport.GetVisibleRect();
		var mousePosition = viewport.GetMousePosition();
		var viewX = (int)((int)mousePosition.X + rect.Position.X);
		var viewY = (int)((int)mousePosition.Y + rect.Position.Y);

		var x = (int)(img.GetSize().X * viewX / rect.Size.X);
		var y = (int)(img.GetSize().Y * viewY / rect.Size.Y);
		//判断透明度在大于0.5时将鼠标不穿透
		if (x < img.GetSize().X && x >= 0 && y < img.GetSize().Y && y >= 0)
		{
			var pixel = img.GetPixel(x, y);
			if (pixel.A > Alpha)
				SetClickAbility(true);
			else
				SetClickAbility(false);
		}
	}

	bool clickthrough = true;

	/// <summary>
	/// 调用Windows系统API来设置鼠标穿透，但目前只支持Windows
	/// </summary>
	private void SetClickAbility(bool clickable)
	{
		if (clickable != clickthrough)
		{
			clickthrough = clickable;
			_mouseClickThrough.SetClickThrough(!clickable);
		}
	}
}
