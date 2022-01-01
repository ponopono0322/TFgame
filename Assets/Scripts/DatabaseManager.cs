using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

//이게 있어야 실행이 가능하다함
[System.Serializable]
//구조체 만들어서 DB를 다룸
public class Item
{   //생성자 생성
    public Item(string _TYPE, string _CODE, string _CATEGORY,
        string _NAME, string _COST, string _INCOME, string _STATE,
        string _USEABLE, string _TIME, string _EXPLAIN)
    {
        //대분류, 고유코드, 중분류 
        TYPE = _TYPE; CODE = _CODE; CATEGORY = _CATEGORY;
        //이름, 소모 또는 전제조건, 수입량, 배운 혹은 얻어낸 상황인지 
        NAME = _NAME; COST = _COST; INCOME = _INCOME; STATE = _STATE;
        //배울 수 있는 혹은 배워야하는 상황인지, 시간비용, 설명
        USEABLE = _USEABLE; TIME = _TIME;  EXPLAIN = _EXPLAIN;
    }
    //변수로 받을 것들
    public string TYPE, CODE, CATEGORY, NAME, COST, INCOME, STATE, USEABLE, TIME, EXPLAIN;

}

public class DatabaseManager : MonoBehaviour
{
    private System.Diagnostics.Stopwatch curTime = TimeController.watch;    //시스템이 시작되면서 흘러온 시간

    public TextAsset ItemDatabase;  //DB를 텍스트파일로 저장했기 때문에 텍스트로 받음
    public List<Item> AllItemList, CurItemList; //db에서 받아온, 버튼에 의해 바뀌는 목록
    public static List<Item> MyItemList; //앞으로 유저가 사용하는 목록

    public string curType = "Science";  //초기설정, 탭 어떤걸 우선으로 볼지

    public GameObject SlotParent, SlotObject;   //동적으로 리스트를 보여주기 위해 부모가 누구인지, 복사할 오브젝트가 누구인지 설정
    public GameObject[] Slot;   //유니티 내에서 데이터베이스 길이만큼 숫자를 입력해줘야함

    public Image[] TabImage;    //강조하려는 탭 버튼들의 이미지
    public Sprite TabIdleSprite, TabSelectSprite;   //기본이미지, 강조하는 이미지
    public GameObject ExplainPanel, ConfirmPanel, WarningPanel; //팝업창들. 세부내용,승인,경고
    IEnumerator PointerCoroutine;   //0.5초 이내에 마우스를 뗐다면 코루틴 중지하기 위한 변수

    public RectTransform[] SlotPos; //캔버스 위치 변환을 통해 어디에 ExplainPanel 띄울지 정하는 것. 
    public RectTransform CanvasRect;    // 어느 캔버스에 띄울건가

    //string filePath;

    void Start()
    {
        //엔터 기준으로 db를 한줄씩 나눈것
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        //line을 또 탭 기준으로 잘라내서 AllItemList에 저장
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //항상 데이터베이스 값 추가될 때 아래 row 하나씩 증가시키기!!
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6], row[7], row[8], row[9]));
            Slot[i] = (GameObject)Instantiate(SlotObject);
            Slot[i].transform.SetParent(SlotParent.transform, false);
        }

        //아직 개인 db를 만들지 않았기 때문에 우선 AllItemList를 그대로 사용함. 나중에는 변경해야함
        MyItemList = AllItemList;

        //기본 준비가 완료되었으면, 기본 탭 띄우고 정보 보여주기
        TabClick(curType);

        //저장경로나 저장법 코드인데, 완벽하지도 않고 저장할 내용이 많이 달라질것 같아서 주석처리함
        //filePath = Application.persistentDataPath + "/MyItemText.txt";
        //Load();
    }

    private void Update()
    {
        //Update마다 마우스 위치 받아서 상세정보 띄워줄지 결정하는 코드(2줄)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, Input.mousePosition, Camera.main, out Vector2 anchoredPos);
        ExplainPanel.GetComponent<RectTransform>().anchoredPosition = anchoredPos + new Vector2(180, 250);
    }

    //탭 버튼들을 눌렀을때 동작하는 함수
    public void TabClick(string tabName)
    {
        //탭 바꿨는데 기존에 아웃라인이 적용되어 있다면 꺼버리기
        if (seletedSlot) { seletedSlot.GetComponent<Outline>().enabled = false; }

        //현재 아이템 리스트에 클릭한 타입만 추가
        curType = tabName;
        //탭에 해당되는 항목만 가져오기
        CurItemList = MyItemList.FindAll(x => x.TYPE == tabName);

        int count = CurItemList.Count; //CurItemList의 항목 수 저장

        //CurItemList의 항목을 보여주는 반복문
        for (int i = 0; i < Slot.Length; i++)
        {
            //슬롯수가 CurItemList보다 많다면 나머지는 끄기
            Slot[i].SetActive(i < count);
            //슬롯 아래에 이름을 출력하기
            Slot[i].GetComponentInChildren<Text>().text = i < count ? CurItemList[i].NAME : "";
            //만약 현재 탭이 Science라면
            if(curType == "Science")
            {
                if (i < count)  //count 갯수 아래로만
                {
                    //기술을 배웠거나 배워야 하는 기술 중에 아직 해금이 안된 상황인 경우 button 비활성화
                    if (CurItemList[i].USEABLE == "FALSE") {Slot[i].GetComponent<Button>().interactable = false;}
                    //기술을 배워야 하는 상황일때 button 활성화
                    else { Slot[i].GetComponent<Button>().interactable = true;}
                }
            }
            //Science 탭이 아니라면 모두 활성화(개발해야함)
            else
            {
                if (i < count)
                {
                    if (CurItemList[i].STATE == "FALSE") { Slot[i].GetComponent<Button>().interactable = false; }
                    else { Slot[i].GetComponent<Button>().interactable = true; }
                }

            }

        }
        
        int tabNum = 0; //string 대신 int 쓰려고
        //매칭시켜줌
        switch (tabName)
        {
            case "Science": tabNum = 0; break;
            case "Facility": tabNum = 1; break;
            case "Market": tabNum = 2; break;
        }
        //선택한 탭이면 강조 이미지로 바꾸고 아니면 기본 이미지로 변경
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    private GameObject before_selected, seletedSlot;    //이전에 선택했던 Slot, 현재 선택할 Slot
    private int curIndex;   //현재 선택한 Slot의 index, 부모로부터 얼마나 아래에 있는지
    private Item CurItem;   //현재 선택한 Slot의 item 변수
    //Slot을 클릭했을때 발동되는 함수
    public void SlotClick()
    {
        //마우스 위치를 2d로 받아냄
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Raycast 가능한 물체들을 감지
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        //Slot(Clone) 즉, 우리가 선택한 슬롯이라면
        if (hit.collider.gameObject.name == "Slot(Clone)")
        {
            seletedSlot = hit.collider.gameObject;  //감지된 object를 받음
            //앞전꺼까지 두번 입력받았는데 둘다 같은 항목이였다면 확인팝업창이 뜨게 해주는 것
            if (before_selected == seletedSlot)
            {
                if (CurItemList[0].TYPE == "Science") { ConfirmPanel.SetActive(true); }
            }
            //이전에 받았던 데이터가 있으면 Outline을 끔
            else if (before_selected) { before_selected.GetComponent<Outline>().enabled = false; }
            seletedSlot.GetComponent<Outline>().enabled = true; //그리고 지금 받은 Slot의 아웃라인을 킴

            before_selected = seletedSlot;  //현재 object를 과거 object로 덮어씀
        }

        //지금 받은 Slot의 위치로 몇번째인지 알아냄
        curIndex = seletedSlot.transform.GetSiblingIndex();
        CurItem = CurItemList[curIndex];    //그 인덱스로 아이템변수 형식의 데이터를 찾아냄
        Debug.Log("selected item index: "+ curIndex.ToString());

    }

    //네 버튼을 눌러 연구를 진행한다면
    public void YES_ConfirmClick()
    {
        //코루틴 진행중에 입력값이 바뀔 수도 있으므로 새로운 변수로 받아 이걸로 수행하려함
        Item SelectedItem = CurItem;

        int ConfirmIndex = MyItemList.FindIndex(x => x.CODE == CurItem.CODE) ;
        Debug.Log("confirm by code:" + ConfirmIndex.ToString() + " and that data is:" + MyItemList[ConfirmIndex].NAME);
        //만약 이미 배운 연구라면 경고창을 띄우도록 함
        if(MyItemList[ConfirmIndex].STATE == "TRUE")
        {
            WarningPanel.SetActive(true);
        }
        //배우지 않은 연구라면 연구수행 코루틴을 진행함
        else
        {
            StartCoroutine(USEABLEChanger(ConfirmIndex, Converter.StringToTime(SelectedItem.TIME), curIndex));
        }

        ConfirmPanel.SetActive(false);
    }

    //연구 수행하는 코루틴
    IEnumerator USEABLEChanger(int i, float t, int c)
    {
        Debug.Log("USEABLEChanger activated");
        yield return new WaitForSeconds(t);     //연구시간이 지난 후에 작동
        Debug.Log("USEABLEChanger waited :" + t.ToString() + "seconds");
        MyItemList[i].STATE = "TRUE";       //작동시킨 연구의 개발 상태를 TRUE로 변경
        MyItemList[i].USEABLE = "FALSE";    //작동시킨 연구의 개발 가능 상태를 FALSE로 변경
        MyItemList[i+1].USEABLE = "TRUE";       //앞으로 작동시킬 연구의 개발 가능 상태를 TRUE로 변경
        Converter.LinkedTech(i);
        //열려있는 탭이 그대로라면 Slot 버튼과 아웃라인 비활성화 진행, 앞으로 개발 가능한 Slot을 해금
        if(CurItem.TYPE == MyItemList[i].TYPE)
        {
            Slot[c].GetComponent<Outline>().enabled = false;
            Slot[c].GetComponent<Button>().interactable = false;
            Slot[c + 1].GetComponent<Button>().interactable = true;
        }
    }

    //아니오 버튼을 눌러 취소하는 경우
    public void NO_ConfirmClick()
    {
        ConfirmPanel.SetActive(false);
        WarningPanel.SetActive(false);
    }

    //Slot의 위를 감지하는 이벤트
    public void PointerEnter()
    {
        PointerCoroutine = PointerEnterDelay();
        StartCoroutine(PointerCoroutine);
    }

    //결과를 뿌려주는 코루틴
    IEnumerator PointerEnterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider.gameObject.name == "Slot(Clone)")
        {
            int numData = hit.collider.gameObject.transform.GetSiblingIndex();
            ExplainPanel.transform.GetChild(0).GetComponent<Text>().text = CurItemList[numData].NAME;
            ExplainPanel.transform.GetChild(3).GetComponent<Text>().text = CurItemList[numData].COST;
            ExplainPanel.transform.GetChild(5).GetComponent<Text>().text = CurItemList[numData].INCOME;
            ExplainPanel.transform.GetChild(6).GetComponent<Text>().text = CurItemList[numData].EXPLAIN;
            ExplainPanel.SetActive(true);
        }
    }

    //Slot 위를 벗어났다면 진행하는 코루틴
    public void PointerExit()
    {
        StopCoroutine(PointerCoroutine);
        ExplainPanel.SetActive(false);
    }


    /*
    void Save()
    {

        File.WriteAllText(filePath + "/Resources/MyItemText.txt");

        TabClick(curType);
    }

    void Load()
    {
        string jdata = File.ReadAllText(filePath + "/Resources/MyItemText.txt");
        TabClick(curType);
    }
    */
}