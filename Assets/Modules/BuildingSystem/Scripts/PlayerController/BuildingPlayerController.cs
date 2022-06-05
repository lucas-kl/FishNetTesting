using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(PlayerInput))]
public class BuildingPlayerController : MonoBehaviour
{

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject cinemachineCameraTarget;
	public CinemachineInputProvider cinemachineInput;

	[SerializeField]
	private float moveSpeed = 50f;
    private PlayerBuildingControlInputs input;
    private GameObject mainCamera;

	private void Awake()
	{
		// get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

	private void Start()
	{
		input = GetComponent<PlayerBuildingControlInputs>();
	}

	// Update is called once per frame
	void Update()
    {
        Move();
		Rotate();
		Zoom();
	}

    private void Move()
    {

        if (input.move == Vector2.zero)
        {
            return;
        }


        Vector3 moveDir = new Vector3(input.move.x, 0, input.move.y).normalized;
		moveDir = mainCamera.transform.up.normalized * input.move.y + mainCamera.transform.right.normalized * input.move.x;
		cinemachineCameraTarget.transform.position += moveDir * moveSpeed * Time.deltaTime;

    }

	private void Rotate()
    {
		if(!input.specialMove)
        {
			cinemachineInput.enabled = false;
			return;
        }

		cinemachineInput.enabled = true;
	}

	private void Zoom()
    {
		if (input.zoom == Vector2.zero)
		{
			return;
		}

		//cinemachineCameraTarget.transform.position += this.mainCamera.transform.forward * input.zoom.y * moveSpeed * Time.deltaTime;
	}

}
