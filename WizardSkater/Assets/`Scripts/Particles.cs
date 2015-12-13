using UnityEngine;

public class Particles : MonoBehaviour
{
    GameObjectPool poolOlli;
    GameObjectPool poolLittleX;

    public void InitObjectPools()
    {
        poolOlli = GameObjectPool.GetPool("Olli");
        poolLittleX = GameObjectPool.GetPool("LittleX");
    }

    public void Fire_Olli(Vector3 pos)
    {
        poolOlli.GetInstance(pos);
    }
}
