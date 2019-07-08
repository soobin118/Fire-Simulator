using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveControllerInputTest : MonoBehaviour {


	private SteamVR_TrackedObject trackedObj; // 추적할 오브젝트의 레퍼런스 선언 


	private SteamVR_Controller.Device Controller // 컨트롤러 입력값 받기
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}


		
	// Update is called once per frame
	void Update () {

		//터치패드 손가락 위치 값(x,y)
		if (Controller.GetAxis () != Vector2.zero) 
		{
			Debug.Log (gameObject.name + Controller.GetAxis ());
		}

		//트리거 버튼 눌림
		if (Controller.GetHairTriggerDown ()) 
		{
			Debug.Log (gameObject.name + "Trigger Press");
		}

		//트리거 버튼 떼짐
		if (Controller.GetHairTriggerUp ()) 
		{
			Debug.Log (gameObject.name + "Trigger Release");
		}

		//그립 버튼 눌림
		if (Controller.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) 
		{
			Debug.Log (gameObject.name + "Grip Press");
		}

		//그립 버튼 떼짐
		if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Grip)) 
		{
			Debug.Log (gameObject + "Grip Release");
		}
			
	}
		
}
