using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false; //
    public Vector3 startPosition; //드래그 시작 위치
    public Transform startParent; //드래그 시작 시 있던 영역

    private GameManager gameManager; //게임 매니저를 참조한다

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; //시작 위치와 부모 저장
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>(); //게임 매니저 참조
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging) //드래그 중일 때
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown() //마우스 클릭 시 드래그 시작
    {
        isDragging = true;

        startPosition = transform.position; //시작 위치와 부모 저장
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10; //드래그 중인 카드가 다른 카드보다 앞에 보이도록 한다
    }

    void OnMouseUp() //마우스 버튼을 놓을 때
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1; //드래그 중인 카드가 다른 카드보다 앞에 보이도록 한다

        if(gameManager == null)
        {
            RetrunToOriginalPosition();
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;

        if(IsOverArea(gameManager.handArea))
        {
            Debug.Log("손패 영역으로 이동");

            if(wasInMergeArea)
            {
                for(int i = 0; i < gameManager.mergeCount; i++)
                {
                    if (gameManager.mergeCards[i] == gameObject)
                    {
                        for(int j = i; j < gameManager.mergeCount -1; j++)
                        {
                            gameManager.mergeCards[j] = gameManager.mergeCards[j + 1];
                        }
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;  //맨 뒤의 카드를 null로 설정
                        gameManager.mergeCount--;                                   //카드 수를 줄인다

                        transform.SetParent(gameManager.handArea); //손패 카드 추가
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand(); //영역 정렬
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else if(IsOverArea(gameManager.mergeArea))              //머지 영역 위에 카드를 놓았는지 확인
            {
                if(gameManager.mergeCount >= gameManager.maxMergeSize)
                {
                    Debug.Log("머지 영역이 가득 찼습니다.");
                    RetrunToOriginalPosition();
                }
                else
                {
                    gameManager.MoveCardToMerge(gameObject);
                }
            }
            else
            {
                RetrunToOriginalPosition();   //아무 영역도 아니면 원래로 돌아가기
            }

            if(wasInMergeArea)
            {
                if(gameManager.mergeButton != null)
                {
                    bool canMerge = (gameManager.mergeCount == 2 || gameManager.mergeCount == 3);
                    gameManager.mergeButton.interactable = canMerge;
                }
            }
        }
    }
    
    void RetrunToOriginalPosition()         //원래 위치로 돌아가는 함수
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        if(gameManager != null)
        {
            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
            if (startParent == gameManager.mergeArea)
            {
                gameManager.ArrangeMerge();
            }
        }
    }

    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach(RaycastHit2D hit in hits)
        {
            if(hit.collider != null && hit.collider.transform == area)
            {
                Debug.Log(area.name + "영역 감지됨");
                return true;
            }
        }

        return false;

    }
}
