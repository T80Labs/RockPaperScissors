using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{
    [SerializeField] private ElementsScriptable element;
    [SerializeField] private Text elementText;

    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickButton);

        elementText.text = element.GetElement().ToString();
    }

    public void SetButtonInteractablity(bool state)
    {
        button.interactable = state;
    }

    public Element GetElement()
    {
        return element.GetElement();
    }

    private void OnClickButton()
    {
        GameController.instance.PlayUserTurn(element);
    }
}
