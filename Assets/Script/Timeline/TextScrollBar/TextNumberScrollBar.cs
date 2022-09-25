using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Miao
{
    public class TextNumberScrollBar : MonoBehaviour
    {
        private Vector2 startOffset;
        private Vector2 endOffset;
        private Vector2 childSize;

        public RectTransform rectTransform;
        public GameObject NumberPrefab;
        public RectTransform content;
        public Image mask;

        [Range(0, 1)]
        public float percentage = 0;
        /// <summary>
        /// 启动时生成的字符串数值区间
        /// </summary>
        [Range(0, 100)]
        public int numberInterval = 20;
        [Button("刷新区间")]
        public void CreateInterval()
        {
            CreateText(numberInterval);
        }
     
        public void Start()
        {
            Init();
        }
        public void Update()
        {
            //MoveTo();
        }

        public void ClearChildText()
        {
            while (content.childCount != 0)
            {
                GameObject.DestroyImmediate(content.GetChild(0).gameObject);
            }


        }
        public void CreateText(int numberInterval)
        {
            ClearChildText();
            for (int i = 0; i <= numberInterval; i++)
            {
                CreateChild(i);
            }
        }
        public void CreateText(int start, int end)
        {
            int size = start - end;
            if (size > 0)
            {
                for (int i = start; i < end; i++)
                {
                    CreateChild(i);
                }
            }
        }

        public void CreateChild(int number)
        {
            var go = GameObject.Instantiate(NumberPrefab);
            TMP_Text text = go.GetComponent<TMP_Text>();
            TextMeshProUGUI Text = go.GetComponent<TextMeshProUGUI>();
            text.text = number.ToString();
            go.transform.SetParent(content, false);

        }

        public void Init()
        {
            childSize = (content.GetChild(0) as RectTransform).sizeDelta;
            //居中处理
            startOffset = Vector2.zero;
            endOffset = Vector2.zero;
            //开始是startOffset为0，0，去向上拉一半的高度让数字到中间，再删掉数字本身一半的高度。

            startOffset += rectTransform.sizeDelta / 2 - childSize / 2;
            for (int i = 0; i < content.childCount; i++)
            {
                endOffset += -childSize;
            }
            //这个也是一样，拉到顶时没有数字，去向下拉一半的高度让数字到中间，再删掉数字本身一半的高度。
            endOffset += rectTransform.sizeDelta/ 2 + childSize / 2;
            startOffset.x = 0;
            endOffset.x = 0;
        }
        /// <summary>
        /// 局限区间
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        [Button("设置区间测试")]
        public void SetMoveInterval(int startIndex, int endIndex)
        {
            if (endIndex > numberInterval)
            {
                Debug.LogError("填入区间不能大于已有区间");
            }

            startOffset = Vector2.zero;
            endOffset = Vector2.zero;
            for (int i = 0; i < numberInterval - startIndex; i++)
            {
                startOffset += -childSize;
            }
            startOffset += rectTransform.sizeDelta / 2 - childSize / 2;

            for (int i = 0; i <= numberInterval - endIndex; i++)
            {
                endOffset += -childSize;
            }
            endOffset += rectTransform.sizeDelta / 2 + childSize / 2;
            startOffset.x = 0;
            endOffset.x = 0;
            //MoveTo();
        }

        public void MoveTo()
        {
            content.anchoredPosition = Vector2.Lerp(startOffset, endOffset, percentage);
        }

    }
}