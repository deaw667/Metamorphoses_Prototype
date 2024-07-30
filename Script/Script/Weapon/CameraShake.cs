using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    #region singleton
    public static CameraShake instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion

    void Start()
    {

    }
}