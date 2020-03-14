/*
    Author: baihan 2020-03-12
    划bezier曲线
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
    [System.Serializable]
    public class BezierCurveShowData
    {
        public List<Vector3> v3Points = new List<Vector3>();
    }

    [ExecuteAlways]
    public class PointCurve : MonoBehaviour
    {
        public List<BezierCurveShowData> m_data;
        public LineRenderer m_lineRenderer;
        public BezierCurveSystem m_scrSystem;
        // line render的最小长度
        [Range(0.1f, 20), Tooltip("线的细腻程度，越小越好，但性能也消耗越大")]
        public float m_fMinLineLength = 1f;

        private void Awake()
        {
            m_scrSystem = GetComponent<BezierCurveSystem>();
            m_lineRenderer = GetComponent<LineRenderer>();
            if (m_lineRenderer == null)
            {
                m_lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
        }

        public void Update()
        {
            m_lineRenderer.positionCount = 0;
            m_data = m_scrSystem.GetBezierPointDatas();
            
            int iCount = 0;
            List<Vector3> v3TotalPoints = new List<Vector3>();
            foreach (var scrPoints in m_data)
            {
                var lsPoints = scrPoints.v3Points;
                var fLineNum = (lsPoints[0] - lsPoints[lsPoints.Count - 1]).magnitude / m_fMinLineLength;
                for (float i = 0; i <= 1; i += 1.0f / fLineNum)
                {
                    var v3BezierPoint = GetLerpPoint(lsPoints.ToArray(), i);
                    v3TotalPoints.Add(v3BezierPoint);
                    ++iCount;
                }
            }

            m_lineRenderer.positionCount = iCount;
            m_lineRenderer.SetPositions(v3TotalPoints.ToArray());
        }

        /// <summary>
        /// 得到n阶bezier的点
        /// </summary>
        /// <param name="lsPoints"></param>
        /// <param name="fDelta"></param>
        /// <returns></returns>
        public Vector3 GetLerpPoint(Vector3[] lsPoints, float fDelta)
        {
            if (lsPoints.Length == 2)
            {
                return Vector3.Lerp(lsPoints[0], lsPoints[1], fDelta);
            }
            Vector3[] lsSubP = new Vector3[lsPoints.Length - 1];
            for (int i = 0; i < lsPoints.Length - 1; i++)
            {
                lsSubP[i] = Vector3.Lerp(lsPoints[i], lsPoints[i + 1], fDelta);
            }

            return GetLerpPoint(lsSubP, fDelta);
        }

        /// <summary>
        /// 得到bezier曲线值
        /// </summary>
        /// <param name="fDelta"></param>
        /// <param name="v3Start"></param>
        /// <param name="v3Effect"></param>
        /// <param name="v3End"></param>
        /// <returns></returns>
        Vector3 CalculateCubicBezierPoint(float fDelta, Vector3 v3Start, Vector3 v3Effect, Vector3 v3End)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp(v3Start, v3Effect, fDelta);
            Vector3 tangentLineVectex2 = Vector3.Lerp(v3Effect, v3End, fDelta);
            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVectex2, fDelta);

            return bezierPoint;
        }

        /// <summary>
        /// 得到三点的bezier曲线
        /// </summary>
        /// <param name="v3Start"></param>
        /// <param name="v3EffectOne"></param>
        /// <param name="v3EffectTwo"></param>
        /// <param name="v3End"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 GetCubicCurvePoint(float t, Vector3 v3Start, Vector3 v3EffectOne, Vector3 v3EffectTwo, Vector3 v3End)
        {
            t = Mathf.Clamp01(t);

            Vector3 part1 = Mathf.Pow(1 - t, 3) * v3Start;
            Vector3 part2 = 3 * Mathf.Pow(1 - t, 2) * t * v3EffectOne;
            Vector3 part3 = 3 * (1 - t) * Mathf.Pow(t, 2) * v3EffectTwo;
            Vector3 part4 = Mathf.Pow(t, 3) * v3End;

            return part1 + part2 + part3 + part4;
        }
    }

}
