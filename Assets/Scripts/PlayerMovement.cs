using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _speed = 1f;
	[SerializeField] private BoolReference _canMove;

	#region UNITY_METHODS

	void Update()
	{
		if (_canMove.toggle)
		{
			Move();
		}
	}

	#endregion

	#region PRIVATE_METHODS

	private void Move()
	{
		Vector3 totalMovement = Vector3.zero;


		// Each key adds to the total movement so it can be normalized before sent
		// Also sets animator bool on key press

		if (Input.GetKey(KeyCode.W))
		{
			totalMovement += transform.up;
		}

		if (Input.GetKey(KeyCode.A))
		{
			totalMovement -= transform.right;
			//transform.localScale = new Vector3(-1, 1, 1);
		}

		if (Input.GetKey(KeyCode.S))
		{
			totalMovement -= transform.up;
		}

		if (Input.GetKey(KeyCode.D))
		{
			totalMovement += transform.right;
			//transform.localScale = new Vector3(1, 1, 1);
		}
		transform.Translate(totalMovement.normalized * Time.deltaTime * _speed);
	}

	#endregion
}