using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj; // 추적할 오브젝트의 레퍼런스 선언 
	private GameObject collidingObject; 
	private GameObject objectInHand;

	private SteamVR_Controller.Device Controller // 컨트롤러 입력값 받기
	{
		get{ return SteamVR_Controller.Input ((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	private void SetCollidingObject(Collider col) //콜라이더 정보 저장 
	{
		if (collidingObject || !col.GetComponent<Rigidbody> ()) // 이미 쥐고있는 콜라이더가 있거나, 오브젝트에 rigidbody가 없다면
		{
			return;
		}
		collidingObject = col.gameObject;
	}

	//콜라이더 트리거 함수들 
	public void OnTriggerEnter(Collider other) //콜라이더가 겹치기 시작했을때
	{
		SetCollidingObject(other);
	}

	public void OnTriggerStay(Collider other) //콜라이더가 겹쳐지고 있을때
	{
		SetCollidingObject(other);
	}

	public void OnTriggerExit(Collider other) //콜라이더가 떨졌을때
	{
		if (!collidingObject) 
		{
			return;
		}
		collidingObject = null;
	}

	private void GrabObject() //물체 잡는 것
	{
		objectInHand = collidingObject;
		collidingObject = null;

		var joint = AddFixedJoint ();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>(); //컨트롤러 오브젝트에 추가된 FixedJoint에 collidingObject 연결
		//connectedBod : 다른 rigidbody를 해당 joint로 연결합니다.
	}

	private FixedJoint AddFixedJoint() //FixedJoint 컴포넌트를 생성해서 반환 
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>(); //컨트롤러 오브젝트에(left,right) FixedJoint 컴포넌트 추가
		fx.breakForce = 20000; //조인트가 제거되도록 하기위해 필요한 힘의 크기를 나타냅니다.
		fx.breakTorque = 20000; //조인트가 제거되도록 하기위해 필요한 토크(torque)의 크기를 나타냅니다.
		return fx;
	}

	private void ReleaseObject()// 물체 놓는 것 
	{
		if (GetComponent<FixedJoint> ()) //컨트롤러 오브젝트에 포함된 FixedJoint 컴포넌트가 있는지 확인 
		{
			GetComponent<FixedJoint>().connectedBody = null; //조인트 연결 끊기
			Destroy (GetComponent<FixedJoint> ()); //Fixed Joint 컴포넌트 삭제

			//물체를 놓았을때의 물리적 표현을 위한 속도와 회전값 설정 
			objectInHand.GetComponent<Rigidbody> ().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody> ().angularVelocity = Controller.angularVelocity;
		}

		objectInHand = null;
	}



	// Update is called once per frame
	void Update () {

		//그립관련
		if (Controller.GetHairTriggerDown ()) 
		{
			if (collidingObject) 
			{
				GrabObject ();
			}
		}

		if (Controller.GetHairTriggerUp ()) 
		{
			if (objectInHand) 
			{
				ReleaseObject ();
			}
		}


	}
}
