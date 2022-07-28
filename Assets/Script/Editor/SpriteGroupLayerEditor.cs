using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

public class SpriteGroupLayerEditor : OdinEditorWindow
{
    [MenuItem("Miao/SpriteGroupLayer图片Layer归类")]
    public static void ShowEditor()
    {
        SpriteGroupLayerEditor spriteGroupLayerEditor =  EditorWindow.GetWindow<SpriteGroupLayerEditor>(false, "SpriteGroupLayer", true);

        SpriteRenderer select =  Selection.activeGameObject.GetComponentInChildren<SpriteRenderer>(true);
        spriteGroupLayerEditor.sortingOrder = select.sortingOrder;
        spriteGroupLayerEditor.sortingLayerName = select.sortingLayerName;

        //SpriteRenderer spriteRenderer = new SpriteRenderer();
    }
    
    public bool 应用order;
    public bool 应用sorting = true;

    [ShowIf("应用order")]//与HideIf成套
    public int sortingOrder;
    [ValueDropdown("GetSortingLayer",DisableGUIInAppendedDrawer = true)]
    [ShowIf("应用sorting")]
    public string sortingLayerName;
    public IEnumerable GetSortingLayer()
    {

        foreach (SortingLayer layer in SortingLayer.layers)
        {
            yield return layer.name;
        }
    }

    [Button("替换选中Layer")]
    public void ChangeSortingLayer()
    {
        GameObject target = Selection.activeGameObject;
        var childrens = target.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (SpriteRenderer childrenSprite in childrens)
        {
            if (应用order) childrenSprite.sortingOrder = sortingOrder;
            if (应用sorting)  childrenSprite.sortingLayerName = sortingLayerName;
        }
        Debug.Log($"替换{target.name}成功，总数{childrens.Length}");

    }


}
/// <summary>
/// 根据名字替换
/// </summary>
public class SpriteNameLayerEditor : OdinEditorWindow
{

    public int sortingOrder;
    [ValueDropdown("GetSortingLayer", DisableGUIInAppendedDrawer = true)]
    public string sortingLayerName;
    public IEnumerable GetSortingLayer()
    {

        foreach (SortingLayer layer in SortingLayer.layers)
        {
            yield return layer.name;
        }
    }

    [MenuItem("Miao/SpriteNameLayer名称Layer归类")]
    public static void ShowEditor()
    {
        SpriteNameLayerEditor spriteNameLayerEditor = EditorWindow.GetWindow<SpriteNameLayerEditor>(false, "SpriteGroupLayer", true);

        GameObject select = Selection.activeGameObject;
        spriteNameLayerEditor.HasChangeName = select.name;
        spriteNameLayerEditor.sortingOrder = select.GetComponent<SpriteRenderer>().sortingOrder;
        spriteNameLayerEditor.sortingLayerName = select.GetComponent<SpriteRenderer>().sortingLayerName;
    }


    //[Toggle("带有的名字")]
    public string HasChangeName;

    [Button("根据名字替换Layer")]
    public void ChangeNameSortingLayer()
    {
        SpriteRenderer[] obj = Transform.FindObjectsOfType<SpriteRenderer>();
        for(int i=0;i<obj.Length;i++)
        {
            if(obj[i].gameObject.name.Contains(HasChangeName))
            {
                obj[i].sortingLayerName = sortingLayerName;
                obj[i].sortingOrder = sortingOrder;
                Debug.Log($"替换成功{obj[i].name}");
            }
        }

    }
}

