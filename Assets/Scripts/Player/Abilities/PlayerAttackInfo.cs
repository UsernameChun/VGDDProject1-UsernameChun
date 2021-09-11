using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerAttackInfo
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("A name for thsi attack")]
    private string m_name;
    public string AttackName
    {
        get
        {
            return m_name;
        }
    }

    [SerializeField]
    [Tooltip("The button to press that will use this attack, This button must be in input settings")]
    private string m_Button;
    public string Button
    {
        get{
            return m_Button;
        }
    }

    [SerializeField]
    [Tooltip("The trigger string to use to activate this attack in the animator")]
    private string m_TriggerName;
    public string TriggerName
    {
        get
        {
            return m_TriggerName;
        }
    }

    [SerializeField]
    [Tooltip("The prefab of the game object representing the ability")]
    private GameObject m_AbilityGO;
    public GameObject AbilityGO
    {
        get
        {
            return m_AbilityGO;
        }
    }

    [SerializeField]
    [Tooltip("Where to spawn the ability game object with respect to the player")]

    private Vector3 m_Offset;
    public Vector3 Offset
    {
        get
        {
            return m_Offset;
        }
    }

    [SerializeField]
    [Tooltip("How long to wait before this attack should be activated after the button is pressed")]
    private float m_WindUpTime;
    public float WindUpTime
    {
        get
        {
            return m_WindUpTime;
        }
    }

    [SerializeField]
    [Tooltip("How long to wait before the player can do everyting again")]
    private float m_FrozenTime;
    public float FrozenTime
    {
        get
        {
            return m_FrozenTime;
        }
    }


    [SerializeField]
    [Tooltip("How long to wait before the player can use this ability again")]
    private float m_Cooldown;

    [SerializeField]
    [Tooltip("How much health the ability costs")]
    private int m_HealthCost;
    public int HealthCost 
    {
        get 
        {
            return m_HealthCost;
        }
    }

    [SerializeField]
    [Tooltip("The color to change to when using this ability")]
    private Color m_Color;
    public Color Color 
    {
        get 
        {
            return m_Color;
        }
    }
    #endregion

    #region  Public Variables

    public float Cooldown 
    {
        get;
        set;
    }
    #endregion

    #region Cooldown Methods
    public void ResetCooldown() 
    {
        Cooldown = m_Cooldown;
    }

    public bool isReady() {
        return Cooldown <= 0;
    }
    #endregion
}
