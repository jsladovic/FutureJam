using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class CameraExtensions
	{
		public static Vector3 MouseWorldPosition(this Camera camera) => camera.ScreenToWorldPoint(Input.mousePosition);
	}
}
