#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



public class PolygonToShadowCasterWindow : EditorWindow
{
    [MenuItem("Miao/PolygonToShadowCaster(多边形转阴影投射器)")]
    static void PolygonToShadowCaster()
    {
        BindingFlags accessFlagsPrivate = BindingFlags.NonPublic | BindingFlags.Instance;


        Transform tr = Selection.activeTransform;
        PolygonCollider2D polygon = tr.GetComponent<PolygonCollider2D>();
        UnityEngine.Rendering.Universal.ShadowCaster2D shadowCaster = tr.GetComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>();
        SpriteRenderer spriteRenderer = tr.GetComponent<SpriteRenderer>();

        FieldInfo shapePathField = typeof(UnityEngine.Rendering.Universal.ShadowCaster2D).GetField("m_ShapePath", accessFlagsPrivate);
        FieldInfo meshField = typeof(UnityEngine.Rendering.Universal.ShadowCaster2D).GetField("m_Mesh", accessFlagsPrivate);
        MethodInfo enableInfo = typeof(UnityEngine.Rendering.Universal.ShadowCaster2D).GetMethod("OnEnable", accessFlagsPrivate);
        //MethodInfo spriteRendererInfo = typeof(SpriteRenderer).GetMethod("OnEnable", accessFlagsPrivate);

        Vector3[] positions = new Vector3[polygon.GetTotalPointCount()];
        for (int i = 0; i < polygon.points.Length; i++)
        {
            positions[i] = new Vector3(polygon.points[i].x, polygon.points[i].y, 0);
        }
        shapePathField.SetValue(shadowCaster, positions);//设置阴影投射同步，鬼知道为什么用Vector3
        meshField.SetValue(shadowCaster, null);//OnEnable的条件为必须mesh为空
        //理论上应该调用 ShadowUtility.GenerateShadowMesh(m_Mesh, m_ShapePath);来完成，但实际这是个内联类，为保证泛用性
        //只能调用含有ShadowUtility.GenerateShadowMesh的OnEnable进行调用
        //但这个函数有点问题，调用后因未知原因阴影会遮盖在图片前，但运行一次后层级排序便会正常
        enableInfo.Invoke(shadowCaster, null);
        //spriteRendererInfo.Invoke(spriteRendererInfo, null);

    }

}
#endif
