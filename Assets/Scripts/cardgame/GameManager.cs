using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //프리팹 리소스
    public GameObject cardPrefab;   //카드 프리팹
    public Sprite[] cardImages;     //카드 이미지

    //영역 Transfarm
    public Transform deckArea;      //덱 영역
    public Transform handArea;      //손패 영역

    //UI 요소
    public Button drawButton;       //드로우 버튼
    public TextMeshProUGUI deckCountText;

    //설정 값
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;


    //배열 선언
    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handCards;
    public int handCount;

    //미리 정의된 덱 카드 목록 (숫자 만)
    private int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,    //1이 8장
        2,2,2,2,2,2,        //2가 6장
        3,3,3,3,            //3이 4장
        4,4                 //4가 2장
    };

    public Transform mergeArea;     //머지 영역 추가
    public Button mergeButton;      //머지 버튼 추가
    public int maxMergeSize = 3;    //최대 머지 수

    public GameObject[] mergeCards; //머지 영역 배열
    public int mergeCount;          //현재 머지 영역에 있는 카드 수

    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null) //버튼 UI 체크
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked); //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
        }

        if (mergeButton != null) //버튼 UI 체크
        {
            mergeButton.onClick.AddListener(OnDrawButtonClicked); //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
            mergeButton.interactable = false; //처음에는 머지 버튼 비활성화
        }
    }

    //덱 셔플
    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount -1; i++)
        {
            int j = Random.Range(i, deckCount);
            //배열 내 카드 교환
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    //덱 초기화 - 정해진 카드 생성
    void InitializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];
            //이미지 인덱스 계산
            int imageIndex = value - 1;
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;
            }
            //카드 오브젝트 생성
            GameObject newCardOdj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardOdj.transform.SetParent(deckArea);
            newCardOdj.SetActive(false);

            //카드 컴포넌트 초기화
            Card cardComp = newCardOdj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardOdj;
        }
    }

    //손패 정렬 함수
    public void ArrangeHand()
    {
        if (handCount == 0)
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;

        for (int i = 0; i < handCount; i ++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                handCards[i].transform.position = newPos;
            }
        }
    }

    public void ArrangeMerge()
    {
        if (mergeCount == 0)
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;

        for (int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                Vector3 newPos = mergeArea.position + new Vector3(startX + i * cardSpacing, 0, -0.005f);
                mergeCards[i].transform.position = newPos;
            }
        }
    }

    void OnDrawButtonClicked() //드로우 버튼 클릭 시 덱에서 카드 뽑기
    {
        DrawCardToHand();
    }

    //덱에서 카드를 뽑아 손패로 이동
    public void DrawCardToHand()
    {
        if (handCount + mergeCount >= maxMergeSize)
        {
            Debug.Log("카드 수가 최대입니다. 공간을 확보하세요.");
            return;
        }
        if (deckCount <= 0)
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }
        GameObject drawnCard = deckCards[0];

        for (int i = 0; i < deckCount - 1; i++)                  //덱 배열 정리 (앞으로 한칸씩 당기기)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true);                              //카드 활성화
        handCards[handCount] = drawnCard;                       //손패에 카드 추가
        handCount++;

        drawnCard.transform.SetParent(handArea);                //카드의 부모를 손패 영역으로 설정

        ArrangeHand();                                          //손패 정렬
    }

        public void MoveCardToMerge(GameObject card)             //카드를 머지 영역으로 이동 [카드를 인수로 받는다]
    {
        if (mergeCount >= maxMergeSize)           //머지 영역이 가득 찼는지 확인
        {
            Debug.Log("머지 영역이 가득 찼습니다!");
            return;
        }
        for (int i = 0; i < handCount; i++) //카드가 손패에 있는지 확인하고 제거
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;
                handCount--;

                ArrangeHand();
                break;              //for문을 빠져나온다
            }
        }

        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);
        ArrangeMerge();
        UpdateMergeButtonState();
    }

    //머지 버튼 상태 업데이트
    void UpdateMergeButtonState()
    {
        if(mergeButton != null) //머지 버튼이 있을 경우
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3); //머지 영역에 카드가 2개 또는 3개만 있을때만 활성화
        }
    }

    //머지 영역의 카드들을 합쳐서 새 카드 생성
    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)                             //머지 영역에 카드가 2개나 3개 있는지 확인하고
        {
            Debug.Log("머지를 하려면 카드가 2개 또는 3개가 필요합니다!");  //카드가 부족하거나 많을 경우
            return;                                                         //함수를 종료한다
        }

        int firstCradValue = mergeCards[0].GetComponent<Card>().cardValue;  //첫번째 카드에 있는 값을 가져온다
        for (int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();                 //머지 배열에 있는 카드들을 순환하면서 검사한다
            if(card == null || card.cardValue != firstCradValue)            //null값이거나 카드값이 다른경우
            {
                Debug.Log("같은 숫자의 카드만 머지 할 수 있습니다.");       //로그를 남기고 함수를 종료한다
                return; //함수를 종료한다
            }
        }

        int newValue = firstCradValue + 1;

        if(newValue > cardImages.Length)
        {
            Debug.Log("최대 카드 값에 도달 했습니다.");
            return;
        }

        for(int i = 0; i < mergeCount; i++)     //머지 영역의 카드들을 비활성화
        {
            if (mergeCards[i] != null)          //머지 영역의 배열들을 순환해서
            {
                mergeCards[i].SetActive(false); //카드들을 비활성화 처리를 한다.
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity); //새 카드 생성

        Card newCardTemp = newCard.GetComponent<Card>();    //생성된 새로운 카드의 컴포넌트를 참조하기 위해 로컬로 선언
        if(newCardTemp != null)                             //생성된 새로운 카드의 컴포넌트가 존재하면 (문제가 없으면)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        //머지 영역 비우기
        for(int i = 0; i < maxMergeSize; i++)   //머지 배열을 순환하면서 null 값을 만든다.
        {
            mergeCards[i] = null;
        }
        mergeCount = 0;                         //머지 카운트 0으로 초기화 시켜준다.

        UpdateMergeButtonState();               //머지 버튼 상태 업데이트

        handCards[handCount] = newCard;         //새로 만들어진 카드를 손패로 이동
        handCount++;                            //핸드 카운트를 증가 시킨다.
        newCard.transform.SetParent(handArea);  //새로 만들어진 카드의 위치를 핸드 영역에 자식으로 놓는다.

        ArrangeHand(); //손패 정렬
    }

    //머지 버튼 클릭 시 머지 영역의 카드를 합성
    void OnMergeButtonClicked()
    {
        MergeCards();
    }
}
