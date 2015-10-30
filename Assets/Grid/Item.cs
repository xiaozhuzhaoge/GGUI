using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Item : MonoBehaviour 
{
    private UILabel label;

    public void Init()
    {
        label = transform.FindChild("Label").GetComponent<UILabel>();
    }

    /// <summary>
    /// 文本内容
    /// </summary>
    public string text
    {
        set 
        {
            label.text = value;
        }
    }
}
