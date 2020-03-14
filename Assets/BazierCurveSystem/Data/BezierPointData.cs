/*
    Author: baihan 2020-03-12
    bezier point data
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BezierCurve
{
    [System.Serializable]
    public class BezierPointData : MonoBehaviour
    {
        public GameObject objPre = null;
        public GameObject objEnd = null;

        // 用于调节的手柄
        public Vector3 v3HandlerDirectionLeft = Vector3.forward;
        public Vector3 v3HandlerDirectionRight = Vector3.back;

        public Vector3 v3HandlerLeft = Vector3.zero;
        public Vector3 v3HandlerRight = Vector3.zero;

        // 手柄长度
        [Range(1, 100), Tooltip("手柄长度")]
        public float fHandlerLength = 10f;
    }

}
