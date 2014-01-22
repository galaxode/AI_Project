using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{

	private CharacterController controller;

	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<CharacterController>;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.getAxisRaw ("Veritical"));

		//transform.rotation = Quartenion.lookRotation (input);
	}
}
