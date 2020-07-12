using DimensionAdventurer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMonoBehaviour : MonoBehaviour
{
    [SerializeField] protected EnvironmentData environmentData;
    [SerializeField] protected WorldObjectManager container;

    private void OnEnable()
    {
        GameManager.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.GameOverEvent -= OnGameOver;
    }

    private void OnGameOver(string reason)
    {
        this.enabled = false;
    }
}
