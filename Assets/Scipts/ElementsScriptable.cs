using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Element",menuName ="ScriptableObjects/Element")]
public class ElementsScriptable : ScriptableObject
{
    [SerializeField] private Element elementType;
    [SerializeField] private List<Element> canWinFrom;

    public bool CanWinFrom(Element element)
    {
        if (canWinFrom.Contains(element))
            return true;
        else
            return false;
    }

    public Element GetElement()
    {
        return elementType;
    }
}
