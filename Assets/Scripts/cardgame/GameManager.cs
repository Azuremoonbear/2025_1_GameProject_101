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

    void Start()
    {
        //배열 초기화
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null) //버튼 UI 체크
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked); //있을 경우 버튼을 누르면 OnDrawButtonClicked 함수 동작
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

    void OnDrawButtonClicked() //드로우 버튼 클릭 시 덱에서 카드 뽑기
    {
        DrawCardToHand();
    }

    //덱에서 카드를 뽑아 손패로 이동
    public void DrawCardToHand()
    {
        if (handCount >= maxHandSize)
        {
            Debug.Log("손패가 가득 찼습니다!");
            return;
        }

        if (deckCount <= 0) //덱에 카드가 남아있는지 확인
        {
            Debug.Log("덱에 더 이상 카드가 없습니다.");
            return;
        }

        GameObject drawnCard = deckCards[0]; //덱에서 맨 위에 카드를 가져오기

        for(int i = 0; i < deckCount - 1; i++) //덱 배열 정리 (앞으로 한칸씩 당기기)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true); //카드 활성화
        handCards[handCount] = drawnCard; //손패에 카드 추가
        handCount++;

        drawnCard.transform.SetParent(handArea); //카드의 부모를 손패 영역으로 설정

        ArrangeHand(); //손패 정렬
    }
}
