/*
    Author: baihan 2020-03-12
    Manage all bezier points 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
    [ExecuteAlways, RequireComponent(typeof(PointCurve))]
    public class BezierCurveSystem : MonoBehaviour
    {
        public List<BezierPoint> m_lsPoints = new List<BezierPoint>();
        public BezierPoint m_scrFristPoint;
        public GameObject m_BezierPref;

        public void AddPoints(List<Object> lsObjPoints)
        {
            for (int i = 0; i < lsObjPoints.Count; ++i)
            {
                var scrBezierPoint = (lsObjPoints[i] as GameObject).GetComponent<BezierPoint>();
                m_lsPoints.Add(scrBezierPoint);
            }
        }

        // public void RemovePoint(BezierPoint scrPoint)
        // {
        //     m_lsPoints.Remove(scrPoint);

        //     if (m_scrFristPoint.gameObject.GetInstanceID() == scrPoint.gameObject.GetInstanceID())
        //     {
        //         m_scrFristPoint = null;
        //     }
        // }

        public void AddFirstPoint(BezierPoint scrFrist)
        {
            m_scrFristPoint = scrFrist;

            if (m_lsPoints.Contains(scrFrist) == false)
            {
                m_lsPoints.Add(scrFrist);
            }
        }

        public void ExportPointsToFile()
        {
            DataExporter.ExportBezierData(m_lsPoints, m_scrFristPoint);
        }

        /// <summary>
        /// 得到每一行的bezier点
        /// </summary>
        /// <returns></returns>
        public List<BezierCurveShowData> GetBezierPointDatas()
        {
            var lsDatas = new List<BezierCurveShowData>();

            bool bOver = false;

            var scrCurrentPoint = m_scrFristPoint;
            while (bOver == false)
            {
                if (scrCurrentPoint.m_data.objEnd != null)
                {
                    var scrPoints = new BezierCurveShowData();
                    
                    // Add self
                    var v3SelfPoint = scrCurrentPoint.transform.position;
                    scrPoints.v3Points.Add(v3SelfPoint);

                    // Self right point
                    var v3SelfRightPoint = scrCurrentPoint.GetRightHandlerPosition();
                    scrPoints.v3Points.Add(v3SelfRightPoint);

                    
                    var scrEnd = scrCurrentPoint.m_data.objEnd.GetComponent<BezierPoint>();
                    // next left
                    var v3NextLeft = scrEnd.GetLeftHandlerPosition();
                    scrPoints.v3Points.Add(v3NextLeft);

                    // next
                    scrPoints.v3Points.Add(scrEnd.transform.position);

                    lsDatas.Add(scrPoints);

                    scrCurrentPoint = scrEnd;
                }
                else
                {
                    bOver = true;
                }
            }

            return lsDatas;
        }

        /// <summary>
        /// 更新所有的子节点
        /// </summary>
        public void UpdateChildPoint()
        {
            // 第一个节点
            if (m_lsPoints.Count > 0)
            {
                m_scrFristPoint = m_lsPoints[0];
            }

            int iChildCount = 0;
            m_lsPoints.Clear();
            for (int i = 0; i < transform.childCount; ++i)
            {
                var objChild = transform.GetChild(i).gameObject;
                var scrChild = objChild.GetComponent<BezierPoint>();
                if (scrChild != null)
                {
                    iChildCount++;
                    objChild.name = "point" + iChildCount;
                    m_lsPoints.Add(scrChild);
                }
            }

            // link nodes
            for (int j = 0; j < m_lsPoints.Count; ++j)
            {
                if (j + 1 < m_lsPoints.Count)
                {
                    m_lsPoints[j].UpdateEndNode(m_lsPoints[j + 1].gameObject);
                }
                if (j - 1 >= 0)
                {
                    m_lsPoints[j].UpdatePreNode(m_lsPoints[j - 1].gameObject);
                }
            }

        }
    }

}
