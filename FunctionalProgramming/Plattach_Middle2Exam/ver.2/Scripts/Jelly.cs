using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyState
{
    public enum SpriteColor
    {
        none=-1,
        blue = 0,
        green = 1,
        orange = 2,
        purple = 3,
        red = 4,
        yellow = 5,
        total = 6
    }
    public enum iState
    {
        wall=-1,
        none= 0,
        isMoving =1
    }

 
    public struct iPosition
    { // 그리드에서의 좌표를 나타내는 구조체. 
        public int col; // 행
        public int row; // 렬
    }

    public struct iMove
    { //움직이면 시작점과 끝점
        public float step;
        public int moveCount;
        public List<Vector3> moveListPos;
        public Vector3 offset;
    }

    public struct MousePos
    {
        public int x;
        public int y;

    }

    public struct JellyDestory
    { //젤리가 죽기전 발생하는 일
        public bool isDeath;
        public int CountColCombo; //행의 콤보
        public int CountRowCombo; //열의 콤보
    }

    public enum itemJelly
    {
        none=0, //아이템 아님
        col=1,
        row=2,
        rainbow=3
    }

    public static float CollisionSize = 1.0f;
}




public class Jelly : MonoBehaviour
{
    public Sprite wall;
    public List<Sprite> SpritesList = new List<Sprite>();
    public List<Sprite> itemSpritesList = new List<Sprite>();
    SpriteRenderer spriteRender;
    SpriteRenderer itemRender;
    public JellyState.SpriteColor spriteIndex;
    public JellyState.iState iState = JellyState.iState.none;
    public JellyState.iPosition iPos;
    public JellyState.iMove iMove;
    public JellyState.MousePos mousePos;
    public JellyState.JellyDestory jellyDeath;
    public JellyState.itemJelly item;



    //젤리 자체 함수
    public float jellySpeed = 10f;


    void Awake()
    {
        //피벗 마춰주기위해서
        spriteRender =transform.GetChild(1).GetComponent<SpriteRenderer>();
        itemRender = transform.GetChild(0).GetComponent<SpriteRenderer>();
        jellyDeath.CountColCombo = 0;
        jellyDeath.CountRowCombo = 0;

        iMove.moveListPos = new List<Vector3>();

        //메모리정리를 위해
        SpritesList.Capacity = SpritesList.Count;
        itemSpritesList.Capacity = itemSpritesList.Count;


    }

    // Update is called once per frame
    void Update()
    {
        if (iMove.moveListPos .Count> 1  && iState == JellyState.iState.isMoving) //백터리스트에 있고, 움직이는중이라면
        {
            iMove.step += Time.deltaTime * jellySpeed;

            if (transform.position != (iMove.moveListPos[iMove.moveCount + 1]- iMove.offset)) //지금위치가 끝점이 아니라면
                transform.position = Vector3.MoveTowards(iMove.moveListPos[iMove.moveCount]- iMove.offset, iMove.moveListPos[iMove.moveCount + 1] - iMove.offset, iMove.step);


            if (transform.position == (iMove.moveListPos[iMove.moveCount + 1] - iMove.offset)) //도착했으면
            {
                if (iMove.moveCount < (iMove.moveListPos.Count - 2)) //아직 루트를 완성 못했다면
                {
                    iMove.moveCount++;
                    iMove.step = 0f;
                }
                else //완성시켰다면
                {
                    iPos.col = (int)iMove.moveListPos[iMove.moveCount + 1].x;
                    iPos.row = (int)iMove.moveListPos[iMove.moveCount + 1].y;
                    GridManager.Grid[iPos.col, iPos.row] = this; //그리드 설정

                    iMove.step = 0f; //혹시 모르니 예외처리
                    iState = JellyState.iState.none; //상태는 원래대로
                    iMove.moveListPos.Clear(); //혹시모르니 리스트는 제거
                }
            }
        }
    }

    public void GetPosition(int col, int row)
    {
        iPos.col = col;
        iPos.row = row;
    }

    public void ChangeSpriteNumber(int i) // 스프라이트 바꾸기
    {
        if (iState == JellyState.iState.wall)
            return;

        if (SpritesList.Count > i)
        {
            spriteIndex = (JellyState.SpriteColor)i;
            spriteRender.sprite = SpritesList[i];
        }
    }


    public void ChangeItemSpriteNumber(int i) // 아이템 바꾸기
    {
        if (iState == JellyState.iState.wall)
            return;

        if (itemSpritesList.Count > i)
        {
            itemRender.sprite = itemSpritesList[i]; 
        }
    }

    public void InitWallSprite()
    {
        if (wall != null)
        {
            itemRender.sprite = wall;
            spriteRender.sprite = null;
        }
        
    }


    //마우스랑 콜라이더 충돌 이벤트 아니면 레이 캐스트를 사용해서 카메라부터 해도되긴함
    public void OnMouseDown()
    {
        if (GridManager.isAnimation)
            return;

        if (GridManager.selected == null)
        {
            GridManager.selected = this;
            this.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void OnMouseUp()
    {
        if (GridManager.isAnimation)
            return;

        if (GridManager.selected != null)
        {
            GridManager.selected = null;
            this.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        }
    }


    public void OnMouseDrag()
    {
        if (GridManager.isAnimation)
            return;

        if (iState == JellyState.iState.wall ||spriteIndex==JellyState.SpriteColor.none)
            return;

        if (GridManager.selected != null)
        {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), GridManager.selected.transform.position) >= 0.6)
            {
                Vector2 MouseOffset = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) - GridManager.selected.transform.position;
                float angle = Mathf.Atan2(MouseOffset.x, MouseOffset.y) * Mathf.Rad2Deg;

                GridManager.selected = null;
                this.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);

                if (angle <= -45 && angle > -135)
                {
                    //왼쪽
                    mousePos.x = -1;
                    mousePos.y = 0;
                    GridManager.CheckJellyLogic(new Vector3(iPos.col, iPos.row, 0), mousePos.x, mousePos.y);
                    return;

                }
                else if (angle <= 135 && angle > 45)
                {
                    //오른쪽
                    mousePos.x = 1;
                    mousePos.y = 0;
                    GridManager.CheckJellyLogic(new Vector3(iPos.col, iPos.row, 0), mousePos.x, mousePos.y);

                    return;
                }
                else if (angle <= 45 && angle > -45)
                {
                    //위쪽
                    mousePos.x = 0;
                    mousePos.y = 1;
                    GridManager.CheckJellyLogic(new Vector3(iPos.col, iPos.row, 0), mousePos.x, mousePos.y);
                    return;
                }
                else
                {
                    //아래쪽
                    mousePos.x = 0;
                    mousePos.y = -1;
                    GridManager.CheckJellyLogic(new Vector3(iPos.col, iPos.row, 0), mousePos.x, mousePos.y);
                    return;
                }
            }
        }
    }

 


}
