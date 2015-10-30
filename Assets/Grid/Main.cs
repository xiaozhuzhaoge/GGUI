using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
    public int count;
    public bool isUpdate = true;


    private GGrid grid;
    private int index = 0;
    private int i = 0;
    private UILabel label;
    public int maxValue;

    void Awake()
    {
        grid = GetComponentInChildren<GGrid>();

        label = transform.FindChild("back/back/Label").GetComponent<UILabel>();
    }

	void Start () 
    {
        while (i < count)
        {
            Add("" + i++);
        }

        label.text = string.Format("创建 {0} 个Item", i.ToString());
	}

    

    void Update()
    {
        if (isUpdate)
        {
            if (index % 1 == 0 && index < maxValue - count)
            {
                Add(i.ToString());
                i++;

                label.text = string.Format("创建 {0} 个Item", i.ToString());
            }
            else
                isUpdate = false;
            index++;
        }
    }

    private void Add(string text)
    {
        grid.AddItem(text);
    }
}
