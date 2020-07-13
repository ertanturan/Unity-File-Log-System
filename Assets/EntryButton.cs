using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EntryButton : MonoBehaviour
{

    public string TemplateEntry = "This is a test entry !";
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnButtonClick()
    {

    }
}
