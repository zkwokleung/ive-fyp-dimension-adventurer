using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRTargetItem : MonoBehaviour
{
    public UnityEvent m_gazeEnterEvent;
    public UnityEvent m_gazeExitEvent;
    public UnityEvent m_completionEvent;

    private Selectable m_selectable;
    private ISubmitHandler m_submit;

    private void Awake()
    {
        m_selectable = GetComponent<Selectable>();
        m_submit = GetComponent<ISubmitHandler>();
    }

    public void GazeEnter(PointerEventData pointer)
    {
        if (m_selectable)
            m_selectable.OnPointerEnter(pointer);
        else
            m_gazeEnterEvent.Invoke();

        Debug.Log(name + ": Gaze Enter");
    }

    public void GazeExit(PointerEventData pointer)
    {
        if (m_selectable)
            m_selectable.OnPointerExit(pointer);
        else
            m_gazeExitEvent.Invoke();

        Debug.Log(name + ": Gaze Exit");
    }

    public void GazeComplete(PointerEventData pointer)
    {
        if (m_submit != null)
            m_submit.OnSubmit(pointer);
        else
            m_completionEvent.Invoke();

    }
}
