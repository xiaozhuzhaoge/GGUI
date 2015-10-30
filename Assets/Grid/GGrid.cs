using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GGrid : UIWidgetContainer
{
    public GameObject Item;

    public int m_cellHeight = 60;

    public int m_cellWidth = 700;

    public float m_height;

    public int m_maxLine;

    public Item[] m_cellList;

    public GScrollView mDrag;

    public float lastY = -1;

    public List<string> m_listData;

    public Vector3 defaultVec;

    public int PreLineAmout;

    public delegate void OnIndexChanged();
    public OnIndexChanged onChange;

    private int start;
    private bool Vchange = false;
    public Transform root;

    public int StartIndex
    {
        set {
            if (start != value)
            {
                if(onChange != null)
                    onChange();
                Vchange = false;
            }
            start = value;
        }
        get { return start; }
    }
    void Awake()
    {
        m_listData = new List<string>();

        defaultVec = new Vector3(0, m_cellHeight, 0);

        mDrag = NGUITools.FindInParents<GScrollView>(gameObject);

        m_height = mDrag.panel.height;

        m_maxLine = (Mathf.CeilToInt(m_height / m_cellHeight));

        m_cellList = new Item[PreLineAmout * m_maxLine];

        CreateItem();

    }

    public void ChangeDebug()
    {
        Debug.Log("Change");
    }

    void Update()
    {
        if (mDrag.transform.localPosition.y != lastY)
        {if(m_listData.Count > 0)
            Validate();

            lastY = mDrag.transform.localPosition.y;
        }
    }

    private void UpdateBounds(int count)
    {
        Vector3 vMin = new Vector3();
        vMin.x = -transform.localPosition.x;
        vMin.y = transform.localPosition.y - (((int)(count / PreLineAmout) + 1) * m_cellHeight);
        vMin.z = transform.localPosition.z;
        Bounds b = new Bounds(vMin, Vector3.one);
        b.Encapsulate(transform.localPosition);

        mDrag.bounds = b;
        mDrag.UpdateScrollbars(true);
        mDrag.RestrictWithinBounds(true);
    }

    public void AddItem(string name)
    {
        m_listData.Add(name);

        Validate();

        UpdateBounds(m_listData.Count);
    }
    [ContextMenu("Execute")]
    private void Validate()
    {
        Vector3 position = root.localPosition;
        float _ver = Mathf.Max(position.y, 0);
        int startIndex = Mathf.FloorToInt(_ver / (m_cellHeight / PreLineAmout));
        int endIndex = Mathf.Min(m_listData.Count, startIndex + PreLineAmout * m_maxLine);
        Item cell;
        int index = 0;
        int lineCount = 0;
        Vchange = true;
        if (startIndex % PreLineAmout != 0)
            return;
        StartIndex = startIndex;
        for (int i = startIndex; i < startIndex + PreLineAmout * m_maxLine; i++)
        {
            cell = m_cellList[index];
            if (i < endIndex)
            {
                cell.text = i.ToString();
                if(i % PreLineAmout == 0)
                {
                    lineCount = i;
                    cell.transform.localPosition = new Vector3(0, lineCount * - (m_cellHeight/PreLineAmout) , 0);
                }
                else
                {
                    cell.transform.localPosition = new Vector3(i % PreLineAmout * m_cellWidth, lineCount * (-m_cellHeight / PreLineAmout), 0);
                }
                cell.name = i.ToString();
                cell.gameObject.SetActive(true);
            }
            else
            {
                cell.transform.localPosition = defaultVec;
            }

            index++;
        }
    }

    private void CreateItem()
    {
        Debug.Log(PreLineAmout + "  " + m_maxLine);
        for (int i = 0; i < PreLineAmout * m_maxLine; i++)
        {
            GameObject go;
            go = Instantiate(Item) as GameObject;
            go.transform.parent = transform;
            go.transform.localScale = Vector3.one;
            go.SetActive(false);
            go.name = i.ToString();
            Item item = go.GetComponent<Item>();
            item.Init();
            m_cellList[i] = item;
        }
    }

    public List<Transform> GetChildList()
    {
        Transform myTrans = transform;
        List<Transform> list = new List<Transform>();

        for (int i = 0; i < myTrans.childCount; ++i)
        {
            Transform t = myTrans.GetChild(i);
            if ((t && NGUITools.GetActive(t.gameObject)))
                list.Add(t);
        }
        return list;
    }
}