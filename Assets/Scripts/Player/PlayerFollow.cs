using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The player to follow")]
    private Transform m_PlayerTransform;

    [SerializeField]
    [Tooltip("Distance from the Player")]
    private Vector3 m_Offset;

    [SerializeField]
    [Tooltip("How quickly the player will rotate")]
    private float m_RotationSpeed = 1;
    #endregion

    #region Main Update
    private void LateUpdate() {
        Vector3 newPos = m_PlayerTransform.position + m_Offset;

        transform.position  = Vector3.Slerp(transform.position, newPos, 1);

        float rotationAmount = m_RotationSpeed * Input.GetAxis("Mouse X");
        transform.RotateAround(m_PlayerTransform.position, Vector3.up, rotationAmount);

        m_Offset = transform.position - m_PlayerTransform.position;
    }
    #endregion
}
