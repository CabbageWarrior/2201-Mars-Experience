
using UnityEngine;

namespace Lab_Minigame {

	public class MinigameLinkChecker : MonoBehaviour {

		[HideInInspector]
		public	MinigameNode Node1, Node2;


		private void OnTriggerEnter( Collider other )
		{
			// transform.parent is the link
			if ( other.transform.parent == null )
				return;

			MinigameLink collided = other.transform.parent.GetComponent<MinigameLink>();
			if ( collided == null )
				return;

			if ( collided.IsOK )
			{
				if ( ( collided.Node1 == Node1 && collided.Node2 == Node2 ) || ( collided.Node1 == Node2 && collided.Node2 == Node1 ) )
					return;

				Destroy( transform.parent.gameObject );
			}
		}

		private void OnDrawGizmos()
		{
			foreach ( BoxCollider c in GetComponents<BoxCollider>() )
			{
				if ( c.enabled )
				{
					Gizmos.matrix = c.transform.localToWorldMatrix;
					Gizmos.DrawCube( c.center, c.size );
				}
			}
		}
	}

}