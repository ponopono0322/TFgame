using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag_HI : MonoBehaviour
{
    Vector3 DragStart;
    Vector3 DragEnd;

    bool isDrag = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);            // ȭ����� ���콺 ��ġ�� dragstart�� �޾ƿ�
        }
        if (Input.GetMouseButton(0))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            if (!isDrag) isDrag = true;

            float cx = DragStart.x, cy = DragStart.y;
            Vector3 tmp2 = DragStart;
            Vector3 tmp = DragEnd - DragStart;
                                                                                    // tmp x,y �� 0�������� ��쿡�� �� -> �� ���ƴ� �� ->�� or �Ʒ� -> �� ���ƴ� �� -> �Ʒ����� �巡�׸� �ǹ���
            cx = (tmp.x >= 0) ? Mathf.Floor(tmp2.x) : Mathf.Ceil(tmp2.x);            //floor �� ���� ceil�� �ø�
            cy = (tmp.y >= 0) ? Mathf.Floor(tmp2.y) : Mathf.Ceil(tmp2.y);            // tmp���� 0���� ũ�鳻�� �ø��� �ø�
            tmp2 = new Vector3(cx, cy, 0f);

            DragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cx = Mathf.Round(DragEnd.x) + ((Mathf.Round(DragEnd.x) == tmp2.x) ? 1f : 0);        // �Ƹ� �������� ���� ��쿡�� �̰ɷ� �����̴� �ɷ� ���ִµ�
            cy = Mathf.Round(DragEnd.y) + ((Mathf.Round(DragEnd.y) == tmp2.y) ? 1f : 0);        

            Vector3 tmp3 = new Vector3(cx, cy, 0);      // ������ġ

            Debug.DrawRay(tmp2, tmp3 - tmp2, Color.red);    // ������ġ�� ���� ��ġ�� �׸� �׷���

            transform.position = new Vector3((tmp2.x + tmp3.x) / 2f, (tmp2.y + tmp3.y) / 2f, 0f);
            transform.localScale = new Vector3(tmp3.x - tmp2.x, tmp3.y - tmp2.y, 0f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<drag_HI>().enabled = false;
            isDrag = false;
        }

    }
}
