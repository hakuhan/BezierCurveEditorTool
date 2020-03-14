using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BezierCurve
{
    [CustomEditor(typeof(BezierPoint)), ExecuteAlways]
    public class BezierPointEditor : Editor
    {
        BezierPoint m_bezierPoint;

        void OnEnable()
        {
            m_bezierPoint = (BezierPoint)target;
            m_bezierPoint.InitData();
        }

        void OnSceneGUI()
        {
            // 修改自身位置
            Handles.color = Color.green;
            Vector3 v3NewNodePos = Handles.FreeMoveHandle(m_bezierPoint.transform.position, m_bezierPoint.transform.rotation, HandleUtility.GetHandleSize(m_bezierPoint.transform.position) * 0.5f, Vector3.zero, Handles.SphereHandleCap);
            m_bezierPoint.transform.position = v3NewNodePos;

            // 修改handle角度
            var v3HandlerLeftPosition = m_bezierPoint.GetLeftHandlerPosition();
            m_bezierPoint.m_data.v3HandlerDirectionLeft = UpdateHandlerDirection(v3HandlerLeftPosition, m_bezierPoint.transform.rotation, m_bezierPoint.transform.position);
            m_bezierPoint.m_data.v3HandlerDirectionRight = m_bezierPoint.m_data.v3HandlerDirectionLeft * -1;

            var v3HandlerRightPosition = m_bezierPoint.GetRightHandlerPosition();
            m_bezierPoint.m_data.v3HandlerDirectionRight = UpdateHandlerDirection(v3HandlerRightPosition, m_bezierPoint.transform.rotation, m_bezierPoint.transform.position);
            m_bezierPoint.m_data.v3HandlerDirectionLeft = m_bezierPoint.m_data.v3HandlerDirectionRight * -1;
            
            // 显示handler
            Handles.color = Color.yellow;
            Handles.DrawLine(m_bezierPoint.transform.position, m_bezierPoint.GetLeftHandlerPosition());
            Handles.DrawLine(m_bezierPoint.transform.position, m_bezierPoint.GetRightHandlerPosition());

            EditorUtility.SetDirty(m_bezierPoint.gameObject);
        }

        // 更新handler角度
        Vector3 UpdateHandlerDirection(Vector3 v3OriginPosition, Quaternion rotation, Vector3 nodePosition)
        {
            var v3NewhandlerPos = Handles.FreeMoveHandle(v3OriginPosition, rotation, HandleUtility.GetHandleSize(v3OriginPosition) * 0.2f, Vector3.zero, Handles.SphereHandleCap);
            var v3NewLeftDirection = (v3NewhandlerPos - nodePosition).normalized;

            return v3NewLeftDirection;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            
            // string strEndNode = "选择结束节点";
            // BezierPoint scrEnd = null;

            // if (m_bezierPoint.m_data.objEnd != null)
            // {
            //     scrEnd = m_bezierPoint.m_data.objEnd.GetComponent<BezierPoint>();
            //     strEndNode = "当前节点：" + scrEnd.gameObject.name + "，更换结束节点";
            // }
            // scrEnd = BezierEditorUtil.PickObj<BezierPoint>(scrEnd, strEndNode);

            // UpdateEndPoint(m_bezierPoint, scrEnd);

        //     // 结束节点
        //     var strEndBezier = "选择结束的bezier节点";
        //     if (m_bezierPoint.m_data.objEnd != null)
        //     {
        //         strEndBezier = "结束节点的id是：" + m_bezierPoint.m_data.objEnd + "名字:" + m_bezierPoint.m_data.objEnd.name;
        //     }
        //     // 抓取
        //     var lsObjEnds = BezierEditorUtil.GetObject(typeof(BezierPoint), strEndBezier);
        //     if (lsObjEnds.Count > 0)
        //     {
        //         var scrNextBezier = (lsObjEnds[0] as GameObject).GetComponent<BezierPoint>();
        //         UpdateEndPoint(m_bezierPoint, scrNextBezier);
        //     }

        //     if (GUILayout.Button("删除结束的节点"))
        //     {
        //         m_bezierPoint.m_data.objEnd = null;
        //     }

        //     // 效果节点
        //     var strEffectNode = "请拖选效果节点";
        //     if (m_bezierPoint.m_data.lsObjEffect != null && m_bezierPoint.m_data.lsObjEffect.Count > 0)
        //     {
        //         strEffectNode = "效果节点为:";
        //         foreach (var objEffectPoint in m_bezierPoint.m_data.lsObjEffect)
        //         {
        //             strEffectNode += " " + objEffectPoint.name + " ";
        //         }
        //     }
        //     var lsObjEffets = BezierEditorUtil.GetObject(typeof(Transform), strEffectNode);
        //     if (lsObjEffets != null && lsObjEffets.Count > 0)
        //     {
        //         foreach (var obj in lsObjEffets)
        //         {
        //             var objEffect = (obj as GameObject);
        //             if (objEffect.GetInstanceID() != m_bezierPoint.gameObject.GetInstanceID()
        //                 && m_bezierPoint.m_data.lsObjEffect.Contains(objEffect) == false)
        //             {
        //                 // m_bezierPoint.AddEffectPoint(objEffect);
        //                 m_bezierPoint.ChangeEffectPoint(objEffect);
        //             }
        //         }
        //     }

        //     // TODO 单个删除
        //     if (GUILayout.Button("删除效果节点的节点"))
        //     {
        //         m_bezierPoint.m_data.lsObjEffect.Clear();
        //     }

        }

        // /// <summary>
        // /// 更新结束节点
        // /// </summary>
        // /// <param name="scrTarget"></param>
        // /// <param name="scrNewEnd"></param>
        // void UpdateEndPoint(BezierPoint scrTarget, BezierPoint scrNewEnd)
        // {
        //     if (scrNewEnd != null)
        //     {
        //         if (scrNewEnd.gameObject.GetInstanceID() == scrTarget.gameObject.GetInstanceID())
        //         {
        //             Debug.Log("自己作为结束");
        //             scrTarget.m_data.objEnd = null;
        //         }
        //         else
        //         {
        //             scrTarget.UpdateEndPoint(scrNewEnd.gameObject);
        //         }
        //     }
        // }

    }

}
