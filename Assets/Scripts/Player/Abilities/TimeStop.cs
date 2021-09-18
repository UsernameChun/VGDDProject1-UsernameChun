using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : Ability
{
    // Start is called before the first frame update
    public override void Use(Vector3 spawnPos) {
        GameObject[] freezeList = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.LogError(freezeList.Length);
        foreach(GameObject i in freezeList) {
            i.GetComponent<EnemyController>().freeze();
        }
    }

    private void OnDestroy() {
        GameObject[] freezeList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject i in freezeList) {
            i.GetComponent<EnemyController>().unfreeze();
        }
    }
}
