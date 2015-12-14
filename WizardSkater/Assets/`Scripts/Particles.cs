using UnityEngine;

public class Particles : MonoBehaviour
{
    GameObjectPool poolOlli;

    public void InitObjectPools()
    {
        poolOlli = GameObjectPool.GetPool("Olli");
    }

    public void Fire_Olli(Vector3 pos)
    {
        poolOlli.GetInstance(pos);
    }
}
