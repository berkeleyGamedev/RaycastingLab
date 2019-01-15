using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Attack : MonoBehaviour
{
    #region Delegates and Events
    public delegate void EmptyDelegate();
    public static event EmptyDelegate EnemyKilledEvent;
    #endregion

    #region Editor Variables
    [SerializeField]
    [Tooltip("The prefab to use for the laser.")]
    private LineRenderer m_LaserPrefab;

    [SerializeField]
    [Tooltip("The prefab to use for the mega laser.")]
    private LineRenderer m_MegaLaserPrefab;

    [SerializeField]
    [Tooltip("The explosion to use when the mega laser hits an enemy.")]
    private ParticleSystem m_MegaLaserExplosion;
    #endregion

    #region Private Variables
    private bool p_GameRunning = false;

    private float p_LaserCooldownTimeLeft;

    private float p_MegaLaserCooldownTimeLeft;

    private float p_LaserRange = 100;
    #endregion

    #region Main Updates
    private void Update()
    {
        if (!p_GameRunning)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && p_LaserCooldownTimeLeft <= 0)
        {
            ShootLaser();
            p_LaserCooldownTimeLeft = 0.2f;
        }
        else if (p_LaserCooldownTimeLeft > 0)
            p_LaserCooldownTimeLeft -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && p_MegaLaserCooldownTimeLeft <= 0)
        {
            ShootMegaLaser();
            p_MegaLaserCooldownTimeLeft = 3f;
        }
        else if (p_MegaLaserCooldownTimeLeft > 0)
            p_MegaLaserCooldownTimeLeft -= Time.deltaTime;
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

    #region Attack Methods
    private void ShootLaser()
    {
        DrawLaser(m_LaserPrefab, transform.position, -Vector2.right, p_LaserRange);
    }

    private void ShootMegaLaser()
    {
        DrawLaser(m_MegaLaserPrefab, transform.position, -Vector2.right, p_LaserRange);
    }
    #endregion

    #region Misc
    private void AddScore()
    {
        EnemyKilledEvent?.Invoke();
    }

    private void DrawLaser(LineRenderer laserPrefab, Vector2 start, Vector2 dir, float dist)
    {
        DrawLaser(laserPrefab, start, start + dir * dist);
    }

    private void DrawLaser(LineRenderer laserPrefab, Vector2 start, Vector2 end)
    {
        LineRenderer laser = Instantiate(laserPrefab);
        laser.SetPosition(0, start);
        laser.SetPosition(1, end);
        Destroy(laser.gameObject, 0.2f);
    }

    private void PlayMegaLaserExplosion(Vector2 pos)
    {
        ParticleSystem ps = Instantiate(m_MegaLaserExplosion, pos, Quaternion.identity);
        Destroy(ps.gameObject, 2f);
    }

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
