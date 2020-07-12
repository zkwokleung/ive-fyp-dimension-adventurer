using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace DimensionAdventurer.VR
{
    public class VRManager : MonoBehaviour
    {
        //VR Objects
        [Header("Prefabs")]
        [SerializeField] private GameObject EditorEmulator;
        [SerializeField] private GameObject EventSystem;
        [SerializeField] private GameObject TeleportIndicator;

        [Header("Settings")]
        [SerializeField] private Vector3 indicatorPosition;

        // Start is called before the first frame update
        void Awake()
        {
            //VR Component Control
#if UNITY_ANDROID || UNITY_EDITOR
            if (GamePreference.singleton.isPlayInVR)
                SpawnVRComponents();
            else
                Destroy(gameObject);
#endif
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            if (GamePreference.singleton.isPlayInVR)
                StartCoroutine(SwitchToVR());
#endif
        }

        private void SpawnVRComponents()
        {
            GameObject es = Instantiate(EventSystem) as GameObject;
            GameObject ti = Instantiate(TeleportIndicator, indicatorPosition, Quaternion.identity);
#if UNITY_EDITOR
            GameObject ee = Instantiate(EditorEmulator) as GameObject;
#endif
        }

        #region Google Vr
        /*
         *  Copied form: https://developers.google.com/vr/develop/unity/guides/hybrid-apps
         * */

        // Call via `StartCoroutine(SwitchToVR())` from your code. Or, use
        // `yield SwitchToVR()` if calling from inside another coroutine.
        public static IEnumerator SwitchToVR()
        {
            // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
            string desiredDevice = "cardboard";

            // Some VR Devices do not support reloading when already active, see
            // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
            if (String.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
            {
                XRSettings.LoadDeviceByName(desiredDevice);

                // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
                yield return null;
            }

            // Now it's ok to enable VR mode.
            XRSettings.enabled = true;
        }

        // Call via `StartCoroutine(SwitchTo2D())` from your code. Or, use
        // `yield SwitchTo2D()` if calling from inside another coroutine.
        public static IEnumerator SwitchTo2D()
        {
            // Empty string loads the "None" device.
            XRSettings.LoadDeviceByName("");

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;

            // Not needed, since loading the None (`""`) device takes care of this.
            // XRSettings.enabled = false;

            // Restore 2D camera settings.
            ResetCameras();
        }

        // Resets camera transform and settings on all enabled eye cameras.
        public static void ResetCameras()
        {
            // Camera looping logic copied from GvrEditorEmulator.cs
            for (int i = 0; i < Camera.allCameras.Length; i++)
            {
                Camera cam = Camera.allCameras[i];
                if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
                {

                    // Reset local position.
                    // Only required if you change the camera's local position while in 2D mode.
                    cam.transform.localPosition = Vector3.zero;

                    // Reset local rotation.
                    // Only required if you change the camera's local rotation while in 2D mode.
                    cam.transform.localRotation = Quaternion.identity;

                    // No longer needed, see issue github.com/googlevr/gvr-unity-sdk/issues/628.
                    // cam.ResetAspect();

                    // No need to reset `fieldOfView`, since it's reset automatically.
                }
            }
        }
        #endregion

    }
}