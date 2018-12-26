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
        GameObject Spawner = ObjectPool.Instance.GetPooledObject(People_1);
        MeshRenderer renderer = Spawner.transform.GetChild(0).GetComponent<MeshRenderer>();
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

        LightProbeProxyVolume proxy = Spawner.GetComponent<LightProbeProxyVolume>();
        Destroy(proxy);

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
