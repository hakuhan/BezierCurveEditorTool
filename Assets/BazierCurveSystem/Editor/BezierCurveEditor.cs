using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BezierCurve
{
    [ExecuteAlways, CustomEditor(typeof(PointCurve))]
    public class BezierCurveEditor : Editor
    {
        public PointCurve m_curve;

        void OnEnable()
        {
            m_curve = target as PointCurve;
        }
    }

}
