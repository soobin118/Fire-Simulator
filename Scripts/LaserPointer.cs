using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	public GameObject laserPrefab; //레이저 프리팹
	private GameObject laser; //레이저 프리팹의 인스턴스
	private Transform laserTransfom; //레이저 위치
	private Vector3 hitPoint; //레이저 hit point

	public Transform cameraRigTransform;
	public GameObject teleportReticlePrefab;
	private GameObject reticle;
	private Transform teleportRerticleTransform;
	public Transform headTransform;
	public Vector3 teleportReticleOffset;
	public LayerMask teleportMask;
	private bool shouldTeleport;

    GameObject Fire;
    public GameObject warter_paticle_obj; //물 파티클을 포함하는 객체
    int flag = 0;


    private SteamVR_Controller.Device Controller
	{
		get{ return SteamVR_Controller.Input((int)trackedObj.index);}
	}

    void Start()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		laser = Instantiate (laserPrefab);
		laserTransfom = laser.transform;

		reticle = Instantiate (teleportReticlePrefab);
		teleportRerticleTransform = reticle.transform;

        warter_paticle_obj.SetActive(false); // 초기상태는 꺼진상태로 설정
    }

	// Update is called once per frame
	void Update () {

        Fire = GameObject.FindGameObjectWithTag("Fire");
        

        //vive 컨트롤러의 터치패드가 눌린다면 
        if (Controller.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) 
		{
			RaycastHit hit;
            GameObject hitObject;
          //  Vector3 v;
         //   v.x = 0.01f;
         //   v.y = 0.01f;
          //  v.z = 0.01f;


            //Ray를 쏜다.
            if (Physics.Raycast (trackedObj.transform.position, transform.forward, out hit, 100, teleportMask)) 
			{
                hitObject = hit.collider.gameObject;
            //    Debug.Log(Fire);
           //     Debug.Log("hitObject"+ hitObject);

            /*    if (hitObject == Fire)
                {
                    Debug.Log("fdfddffddfddffdfdfdfdfdfd");
                    hitObject.transform.localScale -= v;
                }
                */

				hitPoint = hit.point;
				ShowLaser (hit); //hit 포인트를 넘겨줘서 레이저 인스턴스를 그려준다.

				reticle.SetActive (true); //reticle 그려준다.
				teleportRerticleTransform.position = hitPoint + teleportReticleOffset;
				//Z-fighting을 피기 위해 약간의 오프셋을 raycast가 부딪힌 지점에 추가시켜 이동 (?)
				shouldTeleport = true; //텔레포팅을 위한 유효위치가 발견됨 
			} 

		}
		else 
		{
			laser.SetActive (false); //안눌렸을 경우 레이저 인스턴스 비활성화 
			reticle.SetActive(false);
		}

		if (Controller.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport) 
		{
			Teleport ();
		}

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)) //
        {
            
            if (flag == 0)   
                flag = 1;
            else
                flag = 0;

            if (flag == 1)
                warter_paticle_obj.SetActive(true);
            else
                warter_paticle_obj.SetActive(false);
        }
       
        if(flag == 1)
            Controller.TriggerHapticPulse(1000);
    }

	private void Teleport()
	{
        Vector3 v2;
        v2.x = 0f;
        v2.y = 1.5f;
        v2.z = 0f;

        shouldTeleport = false;
		reticle.SetActive (false);

		Vector3 difference = cameraRigTransform.position - headTransform.position;
		difference.y = 0; //카메라 리그 중심값과 플레이어의 위치차이를 구한다.

		cameraRigTransform.position = hitPoint + difference+v2;
	}

	private void ShowLaser(RaycastHit hit)
	{
		laser.SetActive(true); //laser 인스턴스 활성화

		//레이저 position 지정
		laserTransfom.position = trackedObj.transform.position;//Vector3.Lerp (trackedObj.transform.position, hitPoint, 5f);

		//레이저 방향은 hitPoint로
		laserTransfom.LookAt (hitPoint);

		//레이저 스케일 조정
		laserTransfom.localScale = new Vector3 (laserTransfom.localScale.x, laserTransfom.localScale.y, hit.distance);

	}
}
