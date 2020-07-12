using DimensionAdventurer.Inputs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.UI
{
    public enum DialogResult
    {
        None = 0,
        OK = 1,
        Yes = 2,
        No = 3
    }

    public enum MessageBoxButtons
    {
        OK = 0,
        YesNo = 1
    }

    public class MessageBox : MonoBehaviour
    {
        #region Static
        private static MessageBox singleton;
        public static void Show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, Action<DialogResult> onDialogResult = null)
        {
            if (singleton == null)
                Debug.LogError("MessageBox: Singleton is null");

            singleton.show(text, caption, buttons, onDialogResult);
        }
        #endregion

        [SerializeField] private GameObject dialogBox;
        [SerializeField] private TextMeshProUGUI txtCaption;
        [SerializeField] private TextMeshProUGUI txtMessage;
        [SerializeField] private GameObject buttonYes;
        [SerializeField] private GameObject buttonNo;
        [SerializeField] private GameObject buttonOK;

        public float popUpSpeed = 2f;

        private bool _buttonPressed = false;
        private DialogResult _buttonResult = DialogResult.None;
        private Coroutine popUpCoro;

        private event Action<DialogResult> DialogResultEvent;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log("Instance already exists, destroying object . . . ");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            StandaloneInputEventSystem.EscClickedEvent += OnEscClicked;
        }

        private void OnDisable()
        {
            StandaloneInputEventSystem.EscClickedEvent -= OnEscClicked;
        }

        #region Public Methods
        public void show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, Action<DialogResult> onDialogResult = null)
        {
            // Reset value
            _buttonPressed = false;
            _buttonResult = DialogResult.None;

            // Set on result event
            this.DialogResultEvent = onDialogResult;

            //Set text
            txtCaption.text = caption;
            txtMessage.text = text;

            //Display Button
            switch (buttons)
            {
                case (MessageBoxButtons.OK):
                    buttonOK.SetActive(true);
                    buttonYes.SetActive(false);
                    buttonNo.SetActive(false);
                    break;

                case (MessageBoxButtons.YesNo):
                    buttonYes.SetActive(true);
                    buttonNo.SetActive(true);
                    buttonOK.SetActive(false);
                    break;
            }

            // Display Message box
            gameObject.SetActive(true);

            // Pop up animation
            if (popUpCoro != null)
                StopCoroutine(popUpCoro);
            else
                popUpCoro = StartCoroutine(IEPopUpAnimation(popUpSpeed));

            //Wait for button press
            StartCoroutine(IEWaitForButtonPress());
        }

        /// <summary>
        /// Call when pressing button.
        /// </summary>
        /// <param name="result">0 = none, 1 = OK, 2 = Yes, 3 = No</param>
        public void press(int result)
        {
            _buttonResult = (DialogResult)result;
            _buttonPressed = true;
        }
        #endregion

        #region Private methods
        private IEnumerator IEWaitForButtonPress()
        {
            while (!_buttonPressed)
                yield return null;

            OnButtonPress();
        }

        private IEnumerator IEPopUpAnimation(float speed)
        {
            if (speed <= 0) speed = 2f;
            dialogBox.transform.localScale = new Vector3(0, 0, 1);

            while (dialogBox.transform.localScale.x < 1 && dialogBox.transform.localScale.y < 1)
            {
                float deltaSpeed = speed * Time.unscaledDeltaTime;
                dialogBox.transform.localScale += new Vector3(deltaSpeed, deltaSpeed, 0);
                yield return null;
            }
        }

        private void OnButtonPress()
        {
            if (gameObject.activeInHierarchy && DialogResultEvent != null)
                DialogResultEvent.Invoke(_buttonResult);

            gameObject.SetActive(false);
        }

        #endregion

        #region Event
        private void OnEscClicked()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}