using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Spawner : MonoBehaviour {
    //GameObject Spawner
    public GameObject People_1;
    public GameObject C_Spawner;
    //Time of Spawning objects
    float ElapseTime;
    public float EndTime = 1;

    private void onSpawnCrowd()
    {
        GameObject Spawner = CObjectPool.m_sInstance.GetPooledObject(People_1);

        Spawner.transform.position = C_Spawner.transform.position;
    }

	// Update is called once per frame
	void Update () {
        ElapseTime += Time.deltaTime;
        if (ElapseTime > EndTime)
        {
            onSpawnCrowd();
            ElapseTime = 0;
        }
	}
}
