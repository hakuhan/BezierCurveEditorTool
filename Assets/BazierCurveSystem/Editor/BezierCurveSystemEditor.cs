using UnityEditor;
using UnityEngine;

namespace BezierCurve
{
    [CustomEditor(typeof(BezierCurveSystem)), ExecuteAlways]
    public class BezierCurveSystemEditor : Editor
    {
        BezierCurveSystem m_currentSystem;

        public void OnEnable()
        {
            m_currentSystem = (BezierCurveSystem)target;
        }

        private void OnSceneGUI()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                var v3Position = GetWorldPosition(Camera.main, m_currentSystem.transform);

                var newNode = Instantiate(m_currentSystem.m_BezierPref, v3Position, m_currentSystem.transform.rotation, m_currentSystem.transform);
                newNode.name = "point"+ m_currentSystem.m_lsPoints.Count;
                newNode.AddComponent<BezierPoint>();
                m_currentSystem.UpdateChildPoint();
            }
        }

        static Vector3 GetWorldPosition(Camera camera, Transform parent)
        {
            Vector3 mousepos = Event.current.mousePosition;
            mousepos.z = -camera.worldToCameraMatrix.MultiplyPoint(parent.position).z;
            mousepos.y = camera.pixelHeight - mousepos.y;
            mousepos = camera.ScreenToWorldPoint(mousepos);
            return mousepos;
        }

        public override void OnInspectorGUI()
        {
            // DrawDefaultInspector();
            base.OnInspectorGUI();

            GUILayout.Space(10);
            if (m_currentSystem.m_scrFristPoint == null)
            {
                var scrFirstBezier = BezierEditorUtil.PickObj<BezierPoint>(m_currentSystem.m_scrFristPoint, "选择第一个节点");
                if (scrFirstBezier != null)
                {
                    m_currentSystem.m_scrFristPoint = scrFirstBezier;
                }
            }
            // var objFirstBezier = BezierEditorUtil.GetObject(typeof(BezierPoint), "设置第一个节点");
            // if (objFirstBezier != null && objFirstBezier.Count > 0)
            // {
            //     m_currentSystem.AddFirstPoint((objFirstBezier[0] as GameObject).GetComponent<BezierPoint>());
            // }

            // GUILayout.Space(10);
            // var lsObjPoints = BezierEditorUtil.GetObject(typeof(BezierPoint), "添加bezier节点");
            // if (lsObjPoints != null && lsObjPoints.Count > 0)
            // {
            //     m_currentSystem.AddPoints(lsObjPoints);
            // }

            GUILayout.Space(10);

            if (GUILayout.Button("更新子节点"))
            {
                m_currentSystem.UpdateChildPoint();
            }

            GUILayout.Space(10);
            if (GUILayout.Button("导出"))
            {
                m_currentSystem.ExportPointsToFile();
            }
        }

    }
}