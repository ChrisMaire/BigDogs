using UnityEngine;
using System.Collections;

public class DestroyInXSeconds : MonoBehaviour {
    public float X = 5f;

    public bool Pooled = false;
    public string PoolName = "";

    GameObjectPool pool;

	void Start () {
        StartCoroutine(DestroyMe(X));

        if(Pooled)
        {
            pool = GameObjectPool.GetPool(PoolName);
        }
	}
	
	IEnumerator DestroyMe(float t)
    {
        yield return new WaitForSeconds(t);
        
        if(Pooled)
        {
            pool.ReleaseInstance(transform);
        } else
        {
            Destroy(gameObject);
        }
    }
}
