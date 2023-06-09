﻿using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Scabs
{
	public class MovementCurvesController : MonoBehaviour
    {
        public static MovementCurvesController Instance;

        private MovementCurve[] Curves;

        private void Awake()
        {
            Instance = this;
            Curves = GetComponentsInChildren<MovementCurve>();
        }

        public MovementCurve[] GetCurvesForLevelIndex(int levelIndex)
        {
            return Curves.Where(c => levelIndex >= c.LevelIndexUnlocked).ToArray();
        }
    }
}
