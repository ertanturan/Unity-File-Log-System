using UnityEngine;
using UnityEngine.UI;

public class DataLogButton : MonoBehaviour
{
    public string MessageToLog;
    public Button Button { get; set; }


    public virtual void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(delegate { OnButtonClick(); });
    }

    public virtual void OnButtonClick()
    {
        FileLogManager.LogEntry(MessageToLog);
    }
}