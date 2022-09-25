using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AutoBindTextProPosition : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, 0);
    public Transform target;
    private TMP_Text self;

    private void Start()
    {
        self = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        Vector3 pos = new Vector3(offset.x * target.transform.localScale.x, offset.y * target.transform.localScale.y, offset.z * target.transform.localScale.z);

        if (target.transform.localScale.x >= 1)
        {
            self.alignment = TextAlignmentOptions.TopRight;
        }
        else if(target.transform.localScale.x <= -1)
        {
            self.alignment = TextAlignmentOptions.TopLeft;
        }
        else
        {
            self.alignment = TextAlignmentOptions.Top;
        }
        transform.position = pos + target.position;

    }
}

