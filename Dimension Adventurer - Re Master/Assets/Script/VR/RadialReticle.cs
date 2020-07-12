using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialReticle : MonoBehaviour
{
    [SerializeField] private float _defaultDistance = 5f;
    [SerializeField] private bool _isNormalUsed;
    [SerializeField] private Transform _reticleTransform;
    [SerializeField] private Transform _camera;

    [SerializeField] private Image _radialImage;
    [SerializeField] private float _radialDuration = 2f;
    private bool _isRadialFilled = false;
    private float _timer;


    private Vector3 _originalScale;
    private Quaternion _originalRotation;

    public bool IsNormalUsed
    {
        get => _isNormalUsed;
        set => _isNormalUsed = value;
    }

    public Transform ReticleTransform { get => _reticleTransform; }

    private void Awake()
    {
        _originalScale = _reticleTransform.localScale;
        _originalRotation = _reticleTransform.localRotation;
    }

    public void ShowRadialImage(bool active)
    {
        _radialImage.gameObject.SetActive(active);
    }

    public void SetPosition()
    {
        _reticleTransform.position = _camera.position + _camera.forward * _defaultDistance;

        _reticleTransform.localScale = _originalScale * _defaultDistance;

        _reticleTransform.localRotation = _originalRotation;
    }

    public void SetPosition(RaycastResult hit)
    {
        _reticleTransform.localPosition = new Vector3(0f, 0f, hit.distance);
        _reticleTransform.localScale = _originalScale * hit.distance;

        if (_isNormalUsed)
            _reticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.worldNormal);
        else
            _reticleTransform.localRotation = _originalRotation;
    }

    public void StartProgress()
    {
        _isRadialFilled = false;
    }

    public bool ProgressRadialImage()
    {
        if(_isRadialFilled == false)
        {
            _timer += Time.deltaTime;
            _radialImage.fillAmount = _timer / _radialDuration;

            if(_timer >= _radialDuration)
            {
                ResetProgress();
                _isRadialFilled = true;
                return true;
            }
        }
        return false;
    }

    public void ResetProgress()
    {
        _timer = 0f;
        _radialImage.fillAmount = 0f;
    }
}
