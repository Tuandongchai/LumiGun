using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    static int enemyCount = 0;
    private void Start()
    {
        ++enemyCount;
    }

    private void OnDestroy()
    {
        --enemyCount;
        if(enemyCount<=0)
        {
            LevelManager.levelFinished();
        }
    }
}
