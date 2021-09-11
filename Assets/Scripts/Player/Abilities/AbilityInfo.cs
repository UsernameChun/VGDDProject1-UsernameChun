using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AbilityInfo
{   
    #region Editor Variables
    [SerializeField]
    [Tooltip("How much power this abilities has")]
    private int m_power;
    public int Power {
        get {
            return m_power;
        }
    }

    [SerializeField]
    [Tooltip("How much range this ability has")]
    private int m_Range;
    public float Range {
        get {
            return m_Range;
        }
    }
    #endregion
}
