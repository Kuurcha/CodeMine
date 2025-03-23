using Godot;
using System;

public partial class Camera2d : Camera2D
{


    private void CenterScene()
    {
        Vector2 totalMin = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 totalMax = new Vector2(float.MinValue, float.MinValue);

        foreach (Node child in GetChildren())
        {
            if (child is TileMapLayer tileMap)
            {
                Rect2 usedRect = tileMap.GetUsedRect();
                Vector2 min = tileMap.MapToLocal((Vector2I)usedRect.Position);
                Vector2 max = tileMap.MapToLocal((Vector2I)usedRect.End);
                    
                totalMin = new Vector2(Mathf.Min(totalMin.X, min.X), Mathf.Min(totalMin.Y, min.Y));
                totalMax = new Vector2(Mathf.Max(totalMax.X, max.X), Mathf.Max(totalMax.Y, max.Y));
            }
        }

        Vector2 center = (totalMin + totalMax) * 0.5f;

        Position = center;
    }


    public override void _Ready()
    {



        //CenterScene();
        /*Zoom = new Vector2(3.0f, 3.0f);*/
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
