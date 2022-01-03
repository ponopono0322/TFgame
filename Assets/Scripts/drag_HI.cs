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
            DragStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);            // 화면상의 마우스 위치를 dragstart로 받아옴
        }
        if (Input.GetMouseButton(0))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            if (!isDrag) isDrag = true;

            float cx = DragStart.x, cy = DragStart.y;
            Vector3 tmp2 = DragStart;
            Vector3 tmp = DragEnd - DragStart;
                                                                                    // tmp x,y 가 0보다작을 경우에는 좌 -> 우 가아닌 우 ->좌 or 아래 -> 위 가아닌 위 -> 아래로의 드래그를 의미함
            cx = (tmp.x >= 0) ? Mathf.Floor(tmp2.x) : Mathf.Ceil(tmp2.x);            //floor 는 내림 ceil는 올림
            cy = (tmp.y >= 0) ? Mathf.Floor(tmp2.y) : Mathf.Ceil(tmp2.y);            // tmp에서 0보다 크면내림 올리면 올림
            tmp2 = new Vector3(cx, cy, 0f);

            DragEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cx = Mathf.Round(DragEnd.x) + ((Mathf.Round(DragEnd.x) == tmp2.x) ? 1f : 0);        // 아마 움직이지 않은 경우에는 이걸로 움직이는 걸로 쳐주는듯
            cy = Mathf.Round(DragEnd.y) + ((Mathf.Round(DragEnd.y) == tmp2.y) ? 1f : 0);        

            Vector3 tmp3 = new Vector3(cx, cy, 0);      // 현재위치

            Debug.DrawRay(tmp2, tmp3 - tmp2, Color.red);    // 과거위치와 현재 위치를 그림 그려줌

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
