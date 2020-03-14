/*
    Author: baihan 2020-03-12
    用于导出bezier曲线数据 
*/


using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BezierCurve
{
    public class DataExporter : MonoBehaviour
    {
        public static void ExportBezierData(List<BezierPoint> points, BezierPoint firstPoint)
        {
            var vec3First = firstPoint.transform.position;
            StringBuilder strOutput = new StringBuilder("points:\n" + GetVector3String(vec3First) + "\n");

            bool bOver = false;
            var currentPoint = firstPoint;

            // bool bNewLine = false;
            while (bOver == false)
            {
                strOutput.Append(GetVector3String(currentPoint.transform.position));

                strOutput.Append("|");

                // 设置右控制点
                var v3Right = currentPoint.GetRightHandlerPosition();
                strOutput.Append(GetVector3String(v3Right));

                // 下一个节点
                if (currentPoint.m_data.objEnd != null)
                {
                    // 设置下一个节点的左控制点
                    var v3NextLeft = currentPoint.m_data.objEnd.GetComponent<BezierPoint>().GetLeftHandlerPosition();
                    strOutput.Append("|");
                    strOutput.Append(GetVector3String(v3NextLeft));
                    strOutput.Append("\n");

                    currentPoint = currentPoint.m_data.objEnd.GetComponent<BezierPoint>();
                }
                else
                {
                    bOver = true;
                }
            }

            ExportFile(strOutput.ToString());
            Debug.Log("输出完成");
        }

        // 格式化vector3
        static string GetVector3String(Vector3 point)
        {
            string strResult = "";

            strResult += point.x.ToString("F2") + "," + point.y.ToString("F2") + "," + point.z.ToString("F2");

            return strResult;
        }

        static void ExportFile(string strInfo)
        {
            if (Directory.Exists(Application.streamingAssetsPath) == false)
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            FileStream fs = new FileStream(Application.streamingAssetsPath + "/save.txt", FileMode.Create);
            //存储时时二进制,所以这里需要把我们的字符串转成二进制
            byte[] bytes = new UTF8Encoding().GetBytes(strInfo);
            fs.Write(bytes, 0, bytes.Length);
            //每次读取文件后都要记得关闭文件
            fs.Close();

            Debug.Log("Export path: " + Application.streamingAssetsPath + "/save.txt");
        }

    }

}
