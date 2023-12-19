using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public void SetCanvasGroupActive(bool active)
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = active ? 1 : 0;
        group.blocksRaycasts = active;
        group.interactable = active;
    }
}
