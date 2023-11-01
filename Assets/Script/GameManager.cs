using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Bound[] bounds;
    PlayerManager thePlayer;
    CameraManager theCamera;

    // 카메라의 영역 지정을 위한 함수
    public void LoadStart()
    {
        StartCoroutine(LoadWaitCoroutine());
    }

    IEnumerator LoadWaitCoroutine()
    {
        // 씬이 로드되는 동안 대기
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerManager>();
        bounds = FindObjectsOfType<Bound>();
        theCamera = FindObjectOfType<CameraManager>();

        // Player를 찾아서 카메라의 타겟 설정
        theCamera.target = GameObject.Find("Player");

        // 플레이어가 현재 위치한 맵 이름과 같은 이름을 가진 Bound를 찾아서 카메라의 Bound를 설정
        for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].boundName == thePlayer.currentMapName)
            {
                bounds[i].SetBound();
                break;
            }
        }

    }
}
