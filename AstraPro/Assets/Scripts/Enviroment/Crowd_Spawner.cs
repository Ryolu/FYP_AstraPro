using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Spawner : MonoBehaviour {
    //GameObject Spawner
    public GameObject[] People_1;
    public GameObject C_Spawner;
    //Time of Spawning objects
    float ElapseTime;
    public float EndTime = 1;
    public bool Reversed = false;

    private void onSpawnCrowd()
    {
        //Create type of customer from range 0 to type 4
        GameObject Spawner = ObjectPool.Instance.GetPooledObject(People_1[Random.Range(0,4)]);

        //if (Reversed)
        //if the custoemr move from opposite they will walk from reverse directions
        if (Spawner.transform.forward.z != gameObject.transform.forward.z)
            Spawner.transform.forward = Vector3.RotateTowards(Spawner.transform.forward, gameObject.transform.forward, Mathf.PI, 0.0f);

        //Change of their lighting so they won't be dark in color
        MeshRenderer renderer = Spawner.GetComponent<MeshRenderer>();
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

        LightProbeProxyVolume proxy = Spawner.GetComponent<LightProbeProxyVolume>();
        Destroy(proxy);

        //create human from positions
        Spawner.transform.position = C_Spawner.transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        //if pause stop the spawn
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        //create the human from time frame
        ElapseTime += Time.deltaTime;
        if (ElapseTime > EndTime)
        {
            onSpawnCrowd();
            ElapseTime = 0;
        }
	}
}
