using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShootCheckWall_Logic
{
	private const float UpY = 0.4f;
	private const float DownY = 0.4f;

	public static bool CanShoot(Transform pivot, Transform shootPos)
	{
		bool NoWallDetected(float yOffset)
		{
			return Physics2D.Raycast(
				GM.PlayerPos.Add(y: yOffset),
				shootPos.position - pivot.position,
				Vector3.Distance(shootPos.position, pivot.position),
				GM.SoildGroundLayer)
				.collider == null;
		}

		return NoWallDetected(UpY) || NoWallDetected(DownY);
	}
}
