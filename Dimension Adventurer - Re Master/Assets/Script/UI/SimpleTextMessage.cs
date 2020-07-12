using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleTextMessage : MonoBehaviour
{
    public static SimpleTextMessage singleton;

    [SerializeField] private GameObject pnlMessage;
    [SerializeField] private TextMeshProUGUI txtMessage;
    [SerializeField] private float DefaultDisplayTime = 2f;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }

    public void DisplayMessage(MonoBehaviour sender, string message)
    {
        sender.StartCoroutine(IDisplayMessage(message));
    }

    public void DisplayMessage(MonoBehaviour sender, string message, float duration)
    {
        sender.StartCoroutine(IDisplayMessage(message, duration));
    }

    private IEnumerator IDisplayMessage(string message)
    {
        txtMessage.text = message;
        pnlMessage.SetActive(true);
        yield return new WaitForSeconds(DefaultDisplayTime);
        pnlMessage.SetActive(false);
    }

    private IEnumerator IDisplayMessage(string message, float duration)
    {
        txtMessage.text = message;
        pnlMessage.SetActive(true);
        yield return new WaitForSeconds(duration);
        pnlMessage.SetActive(false);
    }

    #region Static
    public static void Show(MonoBehaviour sender, string message)
    {
        singleton.DisplayMessage(sender, message);
    }

    public static void Show(MonoBehaviour sender, string message, float duration)
    {
        singleton.DisplayMessage(sender, message);
    }

    #endregion
}
