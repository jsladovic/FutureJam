﻿using Assets.Scripts.Extensions;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PicketLinerModel : MonoBehaviour
{
	private SpriteRenderer SpriteRenderer;
	private Sprite DefaultSprite;
	[SerializeField] private Sprite CarriedSprite;
	[SerializeField] private SpriteRenderer MergeSpriteRenderer;
	[SerializeField] private Transform ClickingPointsParent;

	public Transform[] ClickingPoints { get; set; }

	public void Initialize()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
		DefaultSprite = SpriteRenderer.sprite;
		DisplayMergeSprite(false);
		ClickingPoints = ClickingPointsParent.GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
	}

	public void SetCarriedSprite(bool isCarried)
	{
		SpriteRenderer.sprite = isCarried ? CarriedSprite : DefaultSprite;
	}

	public void DisplayMergeSprite(bool isVisible)
	{
		MergeSpriteRenderer.gameObject.SetActive(isVisible);
	}

	public Vector3 GetClosestClickingPoint()
	{
		if (ClickingPoints.Length == 0)
			throw new UnityException("No clicking points found");
		if (ClickingPoints.Length == 1)
			return ClickingPoints[0].position;

		Vector2 currentMousePosition = Camera.main.MouseWorldPosition();
		Vector3 closestClickingPoint = ClickingPoints[0].position;
		float minDistance = Vector3.Distance(currentMousePosition, closestClickingPoint);

		for (int i = 1; i < ClickingPoints.Length; i++)
		{
			Vector3 position = ClickingPoints[i].position;
			float distance = Vector3.Distance(currentMousePosition, position);
			if (distance < minDistance)
			{
				minDistance = distance;
				closestClickingPoint = position;
			}
		}

		return closestClickingPoint;
	}
}

