using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imagesize_HI : MonoBehaviour
{
    public GameObject gameObject;
    public GameObject cover;
    public BoxCollider2D box;
    public SpriteRenderer sr;
    Vector3 worldSize = Vector3.zero;
    public Manager_HI manager;
    //bool build_possible = true;     // 다른 건물과 겹치는지 안겹치는지를 설명해주는 건물
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("object"))
        {
           // build_possible = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("object"))
        {
            //build_possible = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetSpriteSize(gameObject));
        transform.localScale = worldSize;
        transform.position = box.bounds.center;
        //manager = GetComponent<Manager_HI>();
    }

    // Update is called once per frame
    void Update()
    {
       
        //Debug.Log(build_possible);
        if(manager.building == true)
        {
            move();
        }

    }
    public Vector3 GetSpriteSize(GameObject _target)                        // 사실 베껴와서 내용은 정확하게 모르나 sprite를 pixel 단위로 나눈후 유니티 내의 단위로 변환시키는 과정인듯 그걸통해 sprite 크기 구하는것으로 추정
    {
       
        if(_target.GetComponent<SpriteRenderer>())
        {
            Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
            Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

            worldSize = localSpriteSize;
            worldSize.x *= _target.transform.lossyScale.x;
            worldSize.y *= _target.transform.lossyScale.y;
        }
        else
        {

        }

        return worldSize;
    }

    public void move()
    {
        float cx;
        float cy;
        Vector3 tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cx = (tmp.x >= 0) ? Mathf.Floor(tmp.x) : Mathf.Ceil(tmp.x);             // 픽셀의 한칸씩 맞춰서 움직일수 있도록 조정해줌
        cy = (tmp.y >= 0) ? Mathf.Floor(tmp.y) : Mathf.Ceil(tmp.y);
        cover.transform.position = new Vector3(cx,cy, 1);
        transform.position = new Vector3(cx,cy, 0);   // 화면상의 마우스 위치를 받아옴
        
        if (Input.GetMouseButtonDown(0))        // 한번더 마우스를 클릭하여 건물의 위치를 지정할경우
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255);      // 투명화 해제
            manager.build_off();
        }
        /*
        else if(Input.GetMouseButtonDown(0) && !build_possible)
        {
            Debug.Log("유효하지 않은 건설 위치 입니다.");
        }*/
    }
}
