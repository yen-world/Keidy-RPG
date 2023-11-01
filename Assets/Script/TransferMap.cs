using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    // 이동할 맵의 이름
    public string transferMapName;

    // 이동하고자 하는 맵 위치
    public Transform target;

    PlayerManager thePlayer;
    CameraManager theCamera;

    // 이동할 맵의 bound를 활용해서 카메라 영역 지정
    public BoxCollider2D targetBound;

    FadeManager theFade;
    OrderManager theOrder;

    public Animator anim_1;
    public Animator anim_2;

    // 문의 갯수
    public int door_count;

    // 캐릭터가 봐야하는 방향
    [Tooltip("UP, DOWN, LEFT, RIHGT")]
    public string direction;

    // 캐릭터가 바라보고 있는 방향을 SetFloat로 받아오기 위한 Vector
    Vector2 vector;

    // 문이 있는지 없는지 Check
    [Tooltip("문이 있다: true, 문이 없다: false")]
    public bool door;


    // Start is called before the first frame update
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 문을 통해 이동하는 것이 아니라면 그냥 TransferCoroutine 실행
        if (!door)
        {
            if (other.gameObject.name == "Player")
            {
                StartCoroutine(TransferCoroutine());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 문을 통해 이동하는 것이라면
        if (door)
        {
            if (other.gameObject.name == "Player")
            {
                // Z 키를 눌렀을 때 vector의 값을 캐릭터가 바라보고 있는 방향으로 초기화
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {
                        case "UP":
                            if (vector.y == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "DOWN":
                            if (vector.y == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "LEFT":
                            if (vector.x == -1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        case "RIGHT":
                            if (vector.x == 1f)
                                StartCoroutine(TransferCoroutine());
                            break;
                        default:
                            StartCoroutine(TransferCoroutine());
                            break;
                    }
                }
            }
        }
    }


    IEnumerator TransferCoroutine()
    {
        theOrder.PreLoadCharacter();

        // 맵 이동이 완료되기 전까지 플레이어의 움직임을 제한하고 FadeOut연출
        theOrder.NotMove();
        theFade.FadeOut();

        thePlayer.currentMapName = transferMapName;
        // 문을 열고 들어가야 한다면 문의 갯수에 맞게 Animation 실행 후 0.5초 대기
        if (door)
        {
            anim_1.SetBool("Open", true);
            if (door_count == 2)
                anim_2.SetBool("Open", true);
        }
        yield return new WaitForSeconds(0.5f);

        // 문이 열리면 Player를 비활성화
        theOrder.SetTransparent("Player");

        // 문이 다시 닫히는 Animation 실행 후 0.5초 대기
        if (door)
        {
            anim_1.SetBool("Open", false);
            if (door_count == 2)
                anim_2.SetBool("Open", false);
        }

        yield return new WaitForSeconds(0.5f);

        // 다시 Player를 활성화
        theOrder.SetUnTransparent("Player");

        // 카메라 영역지정, 카메라 위치 이동, 플레이어 위치 이동 후 FadeIn 연출
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);

        // FadeIn연출이 끝나기 전에 플레이어 움직이면 부자연스러우니 0.5초 대기 후 움직이게함
        theOrder.Move();
    }
}
