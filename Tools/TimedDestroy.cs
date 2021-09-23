using UnityEngine;

public class TimedDestroy : MonoBehaviour {

    public float lifeTime = 0.5f;

	void Start () {
        Destroy(gameObject, lifeTime);	
	}
}
