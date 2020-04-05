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
		this.UpY = 0.4f;
		this.DownY = 0.4f;
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
		bool NoWallDetected(float yOffset)
		{
			return Physics2D.Raycast(GM.PlayerPos.Add(y: yOffset), tf.right, data.CheckLength, GM.SoildGroundLayer).collider == null;
		}

		return NoWallDetected(data.DownY) || NoWallDetected(data.UpY);
	}
}
