using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShootCheckWallData
{
	public readonly float CheckLength;
	public readonly float UpY;
    public readonly float DownY;

	public ShootCheckWallData(float checkLength)
	{
		this.CheckLength = checkLength;
		this.UpY = 0.3f;
		this.DownY = 0.3f;
	}
	public ShootCheckWallData(float checkLength, float upY, float downY)
	{
		this.CheckLength = checkLength;
		this.UpY = upY;
		this.DownY = downY;
	}
}

public static class ShootCheckWall_Logic
{
	public static bool CanShoot(this ShootCheckWallData data, Transform tf)
	{
		const int CHECK_UP = 0;
		const int CHECK_DOWN = 1;

		bool NoWallDetected(int dir)
		{
			float yOffset = dir == CHECK_UP ? data.UpY : -data.DownY;

			return Physics2D.Raycast(GM.PlayerPos.Add(y: yOffset), tf.right, data.CheckLength, GM.SoildGroundLayer).collider == null;
		}

		// When Aiming Up
		if (tf.right.y > 0) return NoWallDetected(CHECK_DOWN);
		// When Aiming Down
		else return NoWallDetected(CHECK_UP);
	}
}
