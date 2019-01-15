using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AimControl : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How quickly the orb can turn.")]
    private float m_RotationSpeed = 1.5f;
    #endregion

    #region Private Variables
    private bool p_GameRunning = false;

    private float p_MinAngle = -30;
    private float p_MaxAngle = 50;
    #endregion

    #region Main Updates
    private void Update()
    {
        if (!p_GameRunning)
            return;

        Vector3 newRot = Vector3.zero;
        newRot.z = -Input.GetAxisRaw("Vertical") * m_RotationSpeed;
        float curAngle = transform.localEulerAngles.z;
        if (curAngle >= 180)
            curAngle -= 360;
        if (curAngle >= p_MaxAngle && newRot.z > 0)
            return;
        if (curAngle <= p_MinAngle && newRot.z < 0)
            return;
        transform.Rotate(newRot);
    }
    #endregion

    #region OnEnable and OnDisable
    private void OnEnable()
    {
        GameManager.GameStartedEvent += SetGameRunningTrue;
        GameManager.GameEndedEvent += SetGameRunningFalse;
    }

    private void OnDisable()
    {
        GameManager.GameStartedEvent -= SetGameRunningTrue;
        GameManager.GameEndedEvent -= SetGameRunningFalse;
    }
    #endregion

    #region Misc
    private void SetGameRunningTrue()
    {
        p_GameRunning = true;
    }

    private void SetGameRunningFalse()
    {
        p_GameRunning = false;
    }
    #endregion
}
