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
    //bool build_possible = true;     // �ٸ� �ǹ��� ��ġ���� �Ȱ�ġ������ �������ִ� �ǹ�
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
    public Vector3 GetSpriteSize(GameObject _target)                        // ��� �����ͼ� ������ ��Ȯ�ϰ� �𸣳� sprite�� pixel ������ ������ ����Ƽ ���� ������ ��ȯ��Ű�� �����ε� �װ����� sprite ũ�� ���ϴ°����� ����
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

        cx = (tmp.x >= 0) ? Mathf.Floor(tmp.x) : Mathf.Ceil(tmp.x);             // �ȼ��� ��ĭ�� ���缭 �����ϼ� �ֵ��� ��������
        cy = (tmp.y >= 0) ? Mathf.Floor(tmp.y) : Mathf.Ceil(tmp.y);
        cover.transform.position = new Vector3(cx,cy, 1);
        transform.position = new Vector3(cx,cy, 0);   // ȭ����� ���콺 ��ġ�� �޾ƿ�
        
        if (Input.GetMouseButtonDown(0))        // �ѹ��� ���콺�� Ŭ���Ͽ� �ǹ��� ��ġ�� �����Ұ��
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 255);      // ����ȭ ����
            manager.build_off();
        }
        /*
        else if(Input.GetMouseButtonDown(0) && !build_possible)
        {
            Debug.Log("��ȿ���� ���� �Ǽ� ��ġ �Դϴ�.");
        }*/
    }
}
