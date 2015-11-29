using UnityEngine;

public class Particles : MonoBehaviour
{
    GameObjectPool poolSkate;
    GameObjectPool poolOlli;
    

    public void InitObjectPools()
    {
        poolSkate = GameObjectPool.GetPool("Skate");
        poolOlli = GameObjectPool.GetPool("Olli");
    }

    public void Fire_Skate(Vector3 pos)
    {
        poolSkate.GetInstance(pos);
    }

    public void Fire_Olli(Vector3 pos)
    {
        poolOlli.GetInstance(pos);
    }
}
