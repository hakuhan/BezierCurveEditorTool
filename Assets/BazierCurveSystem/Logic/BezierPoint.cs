using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
    [RequireComponent(typeof(BezierPointData))]
    public class BezierPoint : MonoBehaviour
    {
        public BezierPointData m_data;

        void Awake()
        {
            InitData();
        }

        public void InitData()
        {
            m_data = gameObject.GetComponent<BezierPointData>();
            // if (m_data == null)
            // {
            //     m_data = gameObject.AddComponent<BezierPointData>();
            // }
        }

        public void UpdateEndNode(GameObject obj)
        {
            m_data.objEnd = obj;
        }

        public void UpdatePreNode(GameObject obj)
        {
            m_data.objPre = obj;
        }

        /// <summary>
        /// 得到left handle的坐标
        /// </summary>
        /// <returns></returns>
        public Vector3 GetLeftHandlerPosition()
        {
            m_data.v3HandlerLeft = m_data.v3HandlerDirectionLeft * m_data.fHandlerLength + transform.position;
            return m_data.v3HandlerLeft;
        }

        /// <summary>
        /// 得到right handle的坐标
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRightHandlerPosition()
        {
            m_data.v3HandlerRight = m_data.v3HandlerDirectionRight * m_data.fHandlerLength + transform.position;
            return m_data.v3HandlerRight;
        }
    }

}
