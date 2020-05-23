using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogExample : MonoBehaviour
{
    public Text title;
    private DialogNode current;
    private List<Button> buttons;
    CustomGraph customGraph = null;

    public AudioSource source;
    public Button button;
    public RectTransform panel;

    // Use this for initialization
    void Start ()
    {
        customGraph = Resources.Load<CustomGraph>("DialogData");
        StartNode startNode = GetStartNode(customGraph);
        Connection connection = GetConnection(customGraph,startNode.startPoint);
        current = connection.inPoint.node as DialogNode;

        setText();
        setAudio();
        createButtons();
    }

    private void setText()
    {
        title.text = current.text;
    }

    private void createButtons()
    {
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }

        buttons = new List<Button>();
        //BuildNode node = build.GetCurrent();

        for (int i = 0; i < current.Triggers.Count; i++)
        {
            int num = i;
            Button t = Instantiate(button, panel, false);
            t.transform.position = new Vector3(t.transform.position.x + (t.GetComponent<RectTransform>().rect.width * i) + 20, t.transform.position.y, t.transform.position.z);
            t.GetComponentInChildren<Text>().text = current.Triggers[i];
            t.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonClick(num));
            buttons.Add(t);
        }
    }

    private void setAudio()
    {
        //source.clip = build.GetCurrent().Clip;
        //source.Play();
    }

    private void OnButtonClick(int num)
    {
        ConnectionPoint outPoint = GetConnectionPoint(current,num);
        Connection connection = GetConnection(customGraph, outPoint);
        ConnectionPoint inPoint = connection.inPoint;

        if (inPoint.node.GetType()==typeof(DialogNode))
        {
            current = inPoint.node as DialogNode;
            setText();
            setAudio();
            createButtons();
        }
        else if (inPoint.node.GetType() == typeof(EndNode))
        {
            title.text = "";
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i].gameObject);
            }
        }
    }

    public StartNode GetStartNode(CustomGraph customGraph)
    {
        foreach (var item in customGraph.windows)
        {
            if (item.GetType() == typeof(StartNode))
            {
                return item as StartNode;
            }
        }
        return null;
    }

    public Connection GetConnection(CustomGraph customGraph,ConnectionPoint connectionPoint)
    {
        foreach (var item in customGraph.connections)
        {
            if (item.ExistConnectionPoint(connectionPoint))
            {
                return item;
            }
        }
        return null;
    }

    public ConnectionPoint GetConnectionPoint(DialogNode dialogNode,int num)
    {
        Debug.LogWarning(dialogNode.text);
        return dialogNode.outPoints[num];
    }
}
