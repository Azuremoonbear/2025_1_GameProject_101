using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //������ ���ҽ�
    public GameObject cardPrefab;   //ī�� ������
    public Sprite[] cardImages;     //ī�� �̹���

    //���� Transfarm
    public Transform deckArea;      //�� ����
    public Transform handArea;      //���� ����

    //UI ���
    public Button drawButton;       //��ο� ��ư
    public TextMeshProUGUI deckCountText;

    //���� ��
    public float cardSpacing = 2.0f;
    public int maxHandSize = 6;


    //�迭 ����
    public GameObject[] deckCards;
    public int deckCount;

    public GameObject[] handCards;
    public int handCount;

    //�̸� ���ǵ� �� ī�� ��� (���� ��)
    private int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,    //1�� 8��
        2,2,2,2,2,2,        //2�� 6��
        3,3,3,3,            //3�� 4��
        4,4                 //4�� 2��
    };

    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];

        InitializeDeck();
        ShuffleDeck();

        if(drawButton != null) //��ư UI üũ
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked); //���� ��� ��ư�� ������ OnDrawButtonClicked �Լ� ����
        }
    }

    //�� ����
    void ShuffleDeck()
    {
        for (int i = 0; i < deckCount -1; i++)
        {
            int j = Random.Range(i, deckCount);
            //�迭 �� ī�� ��ȯ
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    //�� �ʱ�ȭ - ������ ī�� ����
    void InitializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];
            //�̹��� �ε��� ���
            int imageIndex = value - 1;
            if(imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;
            }
            //ī�� ������Ʈ ����
            GameObject newCardOdj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardOdj.transform.SetParent(deckArea);
            newCardOdj.SetActive(false);

            //ī�� ������Ʈ �ʱ�ȭ
            Card cardComp = newCardOdj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardOdj;
        }
    }

    //���� ���� �Լ�
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

    void OnDrawButtonClicked() //��ο� ��ư Ŭ�� �� ������ ī�� �̱�
    {
        DrawCardToHand();
    }

    //������ ī�带 �̾� ���з� �̵�
    public void DrawCardToHand()
    {
        if (handCount >= maxHandSize)
        {
            Debug.Log("���а� ���� á���ϴ�!");
            return;
        }

        if (deckCount <= 0) //���� ī�尡 �����ִ��� Ȯ��
        {
            Debug.Log("���� �� �̻� ī�尡 �����ϴ�.");
            return;
        }

        GameObject drawnCard = deckCards[0]; //������ �� ���� ī�带 ��������

        for(int i = 0; i < deckCount - 1; i++) //�� �迭 ���� (������ ��ĭ�� ����)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true); //ī�� Ȱ��ȭ
        handCards[handCount] = drawnCard; //���п� ī�� �߰�
        handCount++;

        drawnCard.transform.SetParent(handArea); //ī���� �θ� ���� �������� ����

        ArrangeHand(); //���� ����
    }
}
