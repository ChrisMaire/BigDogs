//using UnityEngine;
//using System.Collections;

//public class EndlessScrollingGround : MonoBehaviour 
//{
//	public bool followX = true;
//	public bool followY = true;
//	public bool followZ = true;
	
//	// Update is called once per frame
//	void Update ()
//	{
//	    var target = SystemsManager.m_Player.transform.position;
//		if (target!= null && SystemsManager.m_Player.m_moveSpeedCurrentMax > 0f)
//		{
//			Vector3 newPosition = target;
//			if(!followX)
//			{
//				newPosition.x = transform.position.x;
//			}
//			if(!followY)
//			{
//				newPosition.y = transform.position.y;
//			}
//			if(!followZ)
//			{
//				newPosition.z = transform.position.z;
//			}
//			transform.position = newPosition;
//		}
//	}
//}
