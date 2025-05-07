using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false; //
    public Vector3 startPosition; //�巡�� ���� ��ġ
    public Transform startParent; //�巡�� ���� �� �ִ� ����

    private GameManager gameManager; //���� �Ŵ����� �����Ѵ�

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>(); //���� �Ŵ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging) //�巡�� ���� ��
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseDown() //���콺 Ŭ�� �� �巡�� ����
    {
        isDragging = true;

        startPosition = transform.position; //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10; //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�
    }

    void OnMouseUp() //���콺 ��ư�� ���� ��
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1; //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�

        if(gameManager == null)
        {
            RetrunToOriginalPosition();
            return;
        }

        bool wasInMergeArea = startParent == gameManager.mergeArea;

        if(IsOverArea(gameManager.handArea))
        {
            Debug.Log("���� �������� �̵�");

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
                        gameManager.mergeCards[gameManager.mergeCount - 1] = null;  //�� ���� ī�带 null�� ����
                        gameManager.mergeCount--;                                   //ī�� ���� ���δ�

                        transform.SetParent(gameManager.handArea); //���� ī�� �߰�
                        gameManager.handCards[gameManager.handCount] = gameObject;
                        gameManager.handCount++;

                        gameManager.ArrangeHand(); //���� ����
                        gameManager.ArrangeMerge();
                        break;
                    }
                }
            }
            else if(IsOverArea(gameManager.mergeArea))              //���� ���� ���� ī�带 ���Ҵ��� Ȯ��
            {
                if(gameManager.mergeCount >= gameManager.maxMergeSize)
                {
                    Debug.Log("���� ������ ���� á���ϴ�.");
                    RetrunToOriginalPosition();
                }
                else
                {
                    gameManager.MoveCardToMerge(gameObject);
                }
            }
            else
            {
                RetrunToOriginalPosition();   //�ƹ� ������ �ƴϸ� ������ ���ư���
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
    
    void RetrunToOriginalPosition()         //���� ��ġ�� ���ư��� �Լ�
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
                Debug.Log(area.name + "���� ������");
                return true;
            }
        }

        return false;

    }
}
