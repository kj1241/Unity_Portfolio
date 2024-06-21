using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
 * 2023.02.22 ver0.3
클라이언트 개발과제 핵사

헥스 타일로 개발할까 고민하려다 주어진 에셋을 이용하여 개발하기로 결정
1)	드랍랍로직 : Toy Party - Hexa Blast 게임 헥스(육각형)으로 구현되어 있고 주어진 에셋은 가로 세로 그림 로직임으로 옆으로 흐르는 로직은 벽을 추가 구현하여 해결
2)	매칭 조건: 직선 3개 이상 match 3 알고리즘 이용
3)	추가구현:
-	벽(장애물) 구현
-	아이템 구현(가로 / 세로 폭발)
 */



public class GridManager : MonoBehaviour
{
    public static GridManager pGridManager;
    //전역변수로 젤리 선택되었는지?
    public static Jelly selected;
    public static Jelly[,] Grid;  //코드작성하다보니깐 커플링 조장하네 문제점있음 다음에 바꿀것
    public static int[,] InitJellyWell;
    public static bool isAnimation = false;
    public static Vector3 offset = new Vector3(3f, 2.5f, 0); //중앙에 가게

    public List<GameObject> ParticlesPrfab = new List<GameObject>();



    //행렬
    const int col = 7;
    const int row = 6;

    public static int MaxCol = col;
    public static int MaxRow = row;

    public float jellyCreateTime = 0.2f;
  
    public GameObject JellyParent; //젤리들을 관리하기 위한 오브젝트
    public GameObject JellyPrefab;

    public bool isAllMove = true; //젤리들이 전부 움직였는지

    Jelly JellyScript;

    bool isFirstTime = false;
    bool isJellyLogicCheckNothing = false;

    Jelly tempJelly; //임시





    [Header("UI")]
    public List<Sprite> timeImage;
    public TMP_Text text; //점수
    public Image timeBar;  //시간
    int count = 0;
    float time, curTime = 120f;
    public int spriteCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        pGridManager = this;
        Grid = new Jelly[col, row];
        init();
        StartCoroutine(UIChange());

        //메모리 정리를위해서
        ParticlesPrfab.Capacity = ParticlesPrfab.Count;
        timeImage.Capacity = timeImage.Count;

        

        InitJellyWell = new int[col, row]{
            {1,1,1,1,1,1 },
            {1,1,1,1,1,1 },
            {1,1,1,1,1,1 },
            {3,3,2,0,1,1 },
            {1,1,1,1,1,1 },
            {1,1,1,1,1,1 },
            {1,1,1,1,1,1 }
        };



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        pGridManager = null;
    }

    void init()
    {
        for (int i = 0; i < col; ++i)
            for (int j = 0; j < row; ++j)
            {
                //이미지 줄때 일부로 피벗 안마춰줬는데 원래 피벗은 가운데로 마춰서주는게 기본이긴하지만
                GameObject Jelly = Instantiate(JellyPrefab, new Vector3(i, j, 0)- offset, Quaternion.identity);
                Jelly.transform.SetParent(JellyParent.transform); //정리용
                JellyScript = Jelly.GetComponent<Jelly>();
                JellyScript.GetPosition(i, j);
                JellyScript.ChangeSpriteNumber(Random.Range(0, 5));
                JellyScript.iState = JellyState.iState.none;
                JellyScript.jellyDeath.isDeath = false;
                JellyScript.jellyDeath.CountColCombo = 0;
                Grid[i, j] = JellyScript;
            }

        //벽
        Grid[3, 3].iState = JellyState.iState.wall;
        Grid[3, 3].InitWallSprite();
        Grid[3, 3].spriteIndex = JellyState.SpriteColor.none;
        Grid[3, 3].jellyDeath.isDeath = false;

        Grid[1, 1].ChangeSpriteNumber(1);
        Grid[2, 1].ChangeSpriteNumber(1);
        Grid[3, 1].ChangeSpriteNumber(1);

        Grid[1, 0].ChangeSpriteNumber(1);
        Grid[2, 0].ChangeSpriteNumber(1);
        Grid[3, 0].ChangeSpriteNumber(1);

        Grid[5, 0].ChangeSpriteNumber(1);
        Grid[5, 1].ChangeSpriteNumber(1);
        Grid[5, 2].ChangeSpriteNumber(1);
        Grid[5, 3].ChangeSpriteNumber(1);

        StartCoroutine(JellyCheck());
    }

    IEnumerator JellyCheck()
    {
        isAnimation = true;
        // 가로체크.
        for (int i = 0; i < col - 2; ++i)
        {
            for (int j = 0; j < row; ++j)
            {

                //if (Grid[i, j] == null || Grid[i + 1, j] == null || Grid[i + 2, j] == null)
                //    continue;

                //벽이면 리턴하자
                if (Grid[i, j] != null && Grid[i, j].spriteIndex == JellyState.SpriteColor.none)
                    continue;

                if (Grid[i, j].spriteIndex == Grid[i + 1, j].spriteIndex && Grid[i, j].spriteIndex == Grid[i + 2, j].spriteIndex)
                {
                    Grid[i, j].jellyDeath.isDeath = true;
                    Grid[i + 1, j].jellyDeath.isDeath = true;
                    Grid[i + 2, j].jellyDeath.isDeath = true;

                    Grid[i, j].jellyDeath.CountColCombo += 1;
                    Grid[i+1, j].jellyDeath.CountColCombo += 1;
                    Grid[i+2, j].jellyDeath.CountColCombo += 1;

                }
            }
        }

        //세로
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row - 2; ++j)
            {
                //if (Grid[i, j] == null || Grid[i, j + 1] == null || Grid[i, j + 2] == null)
                //    continue;

                //벽이면 리턴하자
                if (Grid[i, j] != null && Grid[i, j].spriteIndex == JellyState.SpriteColor.none)
                    continue;

                if (Grid[i, j].spriteIndex == Grid[i, j + 1].spriteIndex && Grid[i, j].spriteIndex == Grid[i, j + 2].spriteIndex)
                {
                    Grid[i, j].jellyDeath.isDeath = true;
                    Grid[i, j + 1].jellyDeath.isDeath = true;
                    Grid[i, j + 2].jellyDeath.isDeath = true;

                    Grid[i, j].jellyDeath.CountRowCombo += 1;
                    Grid[i, j+1].jellyDeath.CountRowCombo += 1;
                    Grid[i, j+2].jellyDeath.CountRowCombo += 1;
                }
            }
        }

     

        // 매치된 젤리이 없으면.
        if (!isMatch())
        {
            isAnimation = false;
            if (isFirstTime)
                isJellyLogicCheckNothing = true;

        }
        // 매치된 젤리이 있으면.
        else
        {
            //최초 시작이거나 의도치않은 아이템 획득상황이면
            if (!isFirstTime)
            {
                itemCheck();
            }
            else //잡혀있으면
            {
                //잡혀있는건
                if (pGridManager.tempJelly.item != JellyState.itemJelly.none) //아이템이면
                    pGridManager.tempJelly.jellyDeath.isDeath = true;

                OneCheckJellyItem(pGridManager.tempJelly);

                pGridManager.tempJelly = null;
            }
            isFirstTime = false;

            // 요 위치에서 애니메이션 실행.
            yield return new WaitForSeconds(0.2f);
            // 매치된 블럭을 체크하여 삭제.
            JellyDelete();
        }
    }

    // 젤리중 매치된 젤리가 있으면
    bool isMatch()
    {
        foreach (Jelly jelly in Grid)
        {
            if (jelly.jellyDeath.isDeath)
                return true;
        }
        return false;
    }

    //매치된 젤리가 있으면
    void JellyDelete()
    {
        foreach (Jelly jelly in Grid)
        {
            if (jelly == null)
                continue;

            if (jelly.iState == JellyState.iState.wall || jelly.spriteIndex == JellyState.SpriteColor.none)
            {
                jelly.jellyDeath.isDeath = false;
            }

            if (jelly.jellyDeath.isDeath)
            {
                itemNumberDistroy(jelly);
                //파티클생성
                if ((int)jelly.spriteIndex < ParticlesPrfab.Count)
                {
                    GameObject paricles = Instantiate(ParticlesPrfab[(int)jelly.spriteIndex], new Vector3(jelly.iPos.col, jelly.iPos.row, 0) - offset, Quaternion.identity);
                }
                Grid[jelly.iPos.col, jelly.iPos.row] = null;
                count += 10;
                Destroy(jelly.gameObject);
            }
            else
            {
                jelly.jellyDeath.CountColCombo = 0; //나머지 콤보 초기화
                jelly.jellyDeath.CountRowCombo = 0;
            }
        }
        JellyDown(); //내리자
    }

    //젤리를 내리자 (순서는 고민중) 벽있으면 로직이 달라질수도
    void JellyDown()
    {
        
        for (int i = 0; i < col; ++i)
        {
            int tempCount = -1;// 최적화로 사용하기 위한 변수
            for (int j = 0; j < row; ++j)
            {
                if (InitJellyWell[i, j] == 0) // 벽이면
                    continue;


                if (Grid[i, j] != null && Grid[i, j].iState == JellyState.iState.none) //블록이 존재하고 블록스타일이 평범이면 넘어감
                    continue;

                if (tempCount == -1)
                    tempCount = j + 1;

                ////주석처리한 이유: 삭제된 블럭 에서 생성과 빈칸 이동을 같이 했더니 애니메이션 처리가 안이쁨
                //if (tempCount == row - 1) //로직의 순서는 고민되긴하지만 병렬처리로 같이 떨어진다면
                //{
                //    //생성
                //    GameObject Jelly = Instantiate(JellyPrefab, new Vector3(i, row - 1, 0) - offset, Quaternion.identity); //없으니 생성해주고
                //    Jelly.transform.SetParent(JellyParent.transform); //정리용
                //    JellyScript = Jelly.GetComponent<Jelly>();
                //    JellyScript.ChangeSpriteNumber(Random.Range(0, 5));
                //    JellyScript.isDeath = false;

                //    //이동시키기
                //    JellyScript.iState = JellyState.iState.isMoving; //이동중인상태
                //    JellyScript.iMove.step = 0;
                //    JellyScript.iMove.start = new Vector3(i, row - 1, 0) - offset; //이게 시작점
                //    JellyScript.iMove.end = new Vector3(i, j, 0) - offset;//이것이 끝점
                //    JellyScript.iMove.x = i;
                //    JellyScript.iMove.y = j;
                //    continue;
                //}



                if (InitJellyWell[i, j] == 3) //3이면
                {
                  

                    for (int k = tempCount; k < row; ++k)
                    {
                        if (Grid[i, k] != null && InitJellyWell[i, k] > 1) //벽을 안만났다면
                        {
                            Grid[i, k].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                            Grid[i, k].iMove.step = 0;
                            //이동관련
                            Grid[i, k].iMove.moveCount = 0;
                            Grid[i, k].iMove.moveListPos.Clear();
                            Grid[i, k].iMove.moveListPos.Add(new Vector3(i, k, 0)); //출발
                            Grid[i, k].iMove.moveListPos.Add(new Vector3(i, j, 0)); //도착
                            Grid[i, k].iMove.offset = offset;

                            Grid[i, k].iPos.col = i;
                            Grid[i, k].iPos.row = j;

                            //그리드 설정
                            Grid[i, j] = Grid[i, k];
                            Grid[i, k] = null;
                            tempCount = k;
                            break;

                        }
                        else if (InitJellyWell[i, k] == 0) //벽이라면
                        {
                           

                            for (int l = k; l < MaxRow; ++l)
                            {
                                if (i + 1 < MaxCol && Grid[i + 1, l] != null) //오른쪽 탐색
                                {
                                    Grid[i + 1, l].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                                    Grid[i + 1, l].iMove.step = 0;
                                    Grid[i + 1, l].iMove.moveCount = 0;
                                    if (Grid[i+1, l].iMove.moveListPos.Count != 0)
                                        Grid[i+1, l].iMove.moveListPos.Clear();
                                    Grid[i + 1, l].iMove.moveListPos.Add(new Vector3(i + 1, l, 0)); //출발
                                    Grid[i + 1, l].iMove.moveListPos.Add(new Vector3(i + 1, k, 0)); //출발
                                    Grid[i + 1, l].iMove.moveListPos.Add(new Vector3(i, k - 1, 0)); //중간
                                    Grid[i + 1, l].iMove.moveListPos.Add(new Vector3(i, j, 0)); //끝
                                    Grid[i + 1, l].iMove.offset = offset;

                                    Grid[i + 1, l].iPos.col = i;
                                    Grid[i + 1, l].iPos.row = j;

                                    Grid[i, j] = Grid[i+1, l];
                                    Grid[i+1, l] = null;

                                    break;
                                }
                                else if (i - 1 >= 0 && Grid[i - 1, l] != null) //왼쪽
                                {
                                    Grid[i - 1, l].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                                    Grid[i - 1, l].iMove.step = 0;
                                    Grid[i - 1, l].iMove.moveCount = 0;
                                    if (Grid[i-1, l].iMove.moveListPos.Count != 0)
                                        Grid[i-1, l].iMove.moveListPos.Clear();
                                    Grid[i - 1, l].iMove.moveListPos.Add(new Vector3(i - 1, l, 0)); //출발
                                    Grid[i - 1, l].iMove.moveListPos.Add(new Vector3(i - 1, k, 0)); //출발
                                    Grid[i - 1, l].iMove.moveListPos.Add(new Vector3(i, k - 1, 0)); //중간
                                    Grid[i - 1, l].iMove.moveListPos.Add(new Vector3(i, j, 0)); //끝
                                    Grid[i - 1, l].iMove.offset = offset;
                                    Grid[i - 1, l].iPos.col = i;
                                    Grid[i - 1, l].iPos.row = j;


                                    Grid[i, j] = Grid[i - 1, l];
                                    Grid[i-1, l] = null;


                                    leftSideAlignment(i - 1);

                                    break;
                                }
                                continue;
                            }
                            break;
                        }

                    }
                }
                else if (InitJellyWell[i, j] == 2) //무조건 위에 벽이있음
                {

                 

                    for (int k = tempCount; k < row; ++k)
                    {
                        if (i + 1 < MaxCol && Grid[i + 1, k] != null) //오른쪽 탐색
                        {
                            Grid[i + 1, k].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                            Grid[i + 1, k].iMove.step = 0;
                            Grid[i + 1, k].iMove.moveCount = 0;
                            if (Grid[i+1, k].iMove.moveListPos.Count != 0)
                                Grid[i+1, k].iMove.moveListPos.Clear();
                            Grid[i + 1, k].iMove.moveListPos.Add(new Vector3(i + 1, k, 0)); //출발
                            Grid[i + 1, k].iMove.moveListPos.Add(new Vector3(i + 1, j+1, 0)); //출발
                            Grid[i + 1, k].iMove.moveListPos.Add(new Vector3(i, j, 0)); //중간
                            Grid[i + 1, k].iMove.offset = offset;
                            Grid[i + 1, k].iPos.col = i;
                            Grid[i + 1, k].iPos.row = j;


                            Grid[i, j] = Grid[i + 1, k];
                            Grid[i+1, k] = null;

                            break;
                        }
                        else if (i - 1 >= 0 && Grid[i - 1, k] != null) //왼쪽
                        {
                            Grid[i - 1, k].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                            Grid[i - 1, k].iMove.step = 0;
                            Grid[i - 1, k].iMove.moveCount = 0;
                            if (Grid[i-1, k].iMove.moveListPos.Count != 0)
                                Grid[i-1, k].iMove.moveListPos.Clear();
                            Grid[i - 1, k].iMove.moveListPos.Add(new Vector3(i - 1, k, 0)); //출발
                            Grid[i - 1, k].iMove.moveListPos.Add(new Vector3(i - 1, j+1, 0)); //출발
                            Grid[i - 1, k].iMove.moveListPos.Add(new Vector3(i, j, 0)); //끝
                            Grid[i - 1, k].iMove.offset = offset;
                            Grid[i - 1, k].iPos.col = i;
                            Grid[i - 1, k].iPos.row = j;


                            Grid[i, j] = Grid[i - 1, k];
                            Grid[i - 1, k] = null;

                            leftSideAlignment(i - 1);

                            break;
                        }
                        continue;
                    }

                }
                else if (InitJellyWell[i, j] == 1)
                {
                    tempCount = j + 1;
                    for (int k = tempCount; k < row; ++k)
                    {
                        if (Grid[i, k] != null && Grid[i, k].iState == JellyState.iState.none) // 멈춰있던 블럭이면
                        {
                            Grid[i, k].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                            Grid[i, k].iMove.step = 0;
                            //이동관련
                            Grid[i, k].iMove.moveCount = 0;
                            if (Grid[i, k].iMove.moveListPos.Count != 0)
                                Grid[i, k].iMove.moveListPos.Clear();
                            Grid[i, k].iMove.moveListPos.Add(new Vector3(i, k, 0)); //출발
                            Grid[i, k].iMove.moveListPos.Add(new Vector3(i, j, 0)); //도착
                            Grid[i, k].iMove.offset = offset;

                            Grid[i, k].iPos.col = i;
                            Grid[i, k].iPos.row = j;

                            //그리드 설정
                            Grid[i, j] = Grid[i, k];
                            Grid[i, k] = null;
                            tempCount = k;
                            break;
                        }

                    }


                }
               
            }

         }
     
        StartCoroutine(JellyCreation());// 생성단계
    }

    //생성 및 이동
    IEnumerator JellyCreation()
    {
        yield return new WaitForEndOfFrame(); // 한프레임지나고

        for (int j = 0; j < row; ++j)
        {
            for (int i = 0; i < col; ++i)
            {
                if (Grid[i, j] != null)
                    continue;

                else if (InitJellyWell[i, j] == 3)
                {
                    int temp = -1;
                    for (int k = j + 1; k < col; ++k)
                    {
                        if (InitJellyWell[i, k] == 0)
                            temp = k;
                    }

                    //생성
                    GameObject Jelly = Instantiate(JellyPrefab, new Vector3(i, row - 1, 0) - offset, Quaternion.identity);
                    Jelly.transform.SetParent(JellyParent.transform); //정리용
                    JellyScript = Jelly.GetComponent<Jelly>();
                    JellyScript.ChangeSpriteNumber(Random.Range(0, 5));

                    JellyScript.iState = JellyState.iState.isMoving; //움직이는중
                    JellyScript.jellyDeath.isDeath = false;
                    JellyScript.iMove.step = 0;
                    //이동관련
                    JellyScript.iMove.moveCount = 0;


                    if (i + 1 < MaxCol && Grid[i + 1, j + 1] == null && temp>-1) //오른
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i + 1, row - 1, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i + 1, temp, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i , temp-1, 0)); //출발
                    }
                    else if (i - 1 >= 0 && Grid[i - 1, j + 1] == null && temp > -1) //왼쪽
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i - 1, row - 1, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i - 1, temp, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i, temp - 1, 0)); //출발
                    }
                    else
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i, row -1, 0)); //출발
                    }
                    JellyScript.iMove.moveListPos.Add(new Vector3(i, j, 0)); //도착

                    JellyScript.iMove.offset = offset;
                    JellyScript.jellyDeath.CountColCombo = 0;
                    JellyScript.iPos.col = i;
                    JellyScript.iPos.row = j;

                    Grid[i, j] = JellyScript;

                }
                else if (InitJellyWell[i, j] == 2)
                {
                    //생성
                    GameObject Jelly = Instantiate(JellyPrefab, new Vector3(i, row - 1, 0) - offset, Quaternion.identity);
                    Jelly.transform.SetParent(JellyParent.transform); //정리용
                    JellyScript = Jelly.GetComponent<Jelly>();
                    JellyScript.ChangeSpriteNumber(Random.Range(0, 5));

                    JellyScript.iState = JellyState.iState.isMoving; //움직이는중
                    JellyScript.jellyDeath.isDeath = false;
                    JellyScript.iMove.step = 0;
                    //이동관련
                    JellyScript.iMove.moveCount = 0;

                    if (i + 1 < MaxCol && Grid[i + 1, j + 1] == null)
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i + 1, row - 1, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i + 1, j + 1, 0)); //출발
                    }
                    else if (i - 1 >= 0 && Grid[i - 1, j + 1] == null)
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i - 1, row - 1, 0)); //출발
                        JellyScript.iMove.moveListPos.Add(new Vector3(i - 1, j + 1, 0)); //출발
                    }
                    else //못넘어가니깐 위에서 떨어지기
                    {
                        JellyScript.iMove.moveListPos.Add(new Vector3(i, row - 1, 0)); //출발
                    }
                    JellyScript.iMove.moveListPos.Add(new Vector3(i, j, 0)); //도착


                    JellyScript.iMove.offset = offset;
                    JellyScript.jellyDeath.CountColCombo = 0;
                    JellyScript.iPos.col = i;
                    JellyScript.iPos.row = j;

                    Grid[i, j] = JellyScript;
                }



                else if (InitJellyWell[i, j] == 1)
                {
                    //생성
                    GameObject Jelly = Instantiate(JellyPrefab, new Vector3(i, row - 1, 0) - offset, Quaternion.identity);
                    Jelly.transform.SetParent(JellyParent.transform); //정리용
                    JellyScript = Jelly.GetComponent<Jelly>();
                    JellyScript.ChangeSpriteNumber(Random.Range(0, 5));

                    JellyScript.iState = JellyState.iState.isMoving; //움직이는중
                    JellyScript.jellyDeath.isDeath = false;
                    JellyScript.iMove.step = 0;
                    //이동관련
                    JellyScript.iMove.moveCount = 0;
                    JellyScript.iMove.moveListPos.Add(new Vector3(i, row - 1, 0)); //출발
                    JellyScript.iMove.moveListPos.Add(new Vector3(i, j, 0)); //도착

                    JellyScript.iMove.offset = offset;
                    JellyScript.jellyDeath.CountColCombo = 0;
                    JellyScript.iPos.col = i;
                    JellyScript.iPos.row = j;

                    Grid[i, j] = JellyScript;
                }


            }
            yield return new WaitForSeconds(jellyCreateTime); //0.x초 마다 한줄씩 블럭생성
        }

        yield return StartCoroutine(JellyCheck()); //생성된게있는지 체크
    }


    void DragMoving(Vector3 start, int i, int j)
    {

        int col = (int)start.x;
        int row = (int)start.y;

        //마우스 범위 밖으로 못나가게
        if (col + i >= GridManager.MaxCol || col + i < 0)
            return;
        if (row + j >= GridManager.MaxRow || row + j < 0)
            return;

        if (GridManager.Grid[col + i, row + j] == null)
            return;

        Jelly starttemp = GridManager.Grid[col, row];
        Jelly endtemp = GridManager.Grid[col + i, row + j];

        //벽은 넘어감
        if (starttemp.spriteIndex == JellyState.SpriteColor.none || endtemp.spriteIndex == JellyState.SpriteColor.none||starttemp.iState ==JellyState.iState.wall||endtemp.iState==JellyState.iState.wall)
            return;

        //잡은 것을 움직이자
        starttemp.iMove.step = 0;
        starttemp.iMove.moveCount = 0;
        starttemp.iMove.moveListPos.Add(new Vector3(col, row, 0)); //출발
        starttemp.iMove.moveListPos.Add(new Vector3(col + i, row + j, 0)); //도착
        starttemp.iMove.offset = GridManager.offset;
        starttemp.iState = JellyState.iState.isMoving;

        endtemp.iMove.step = 0;
        endtemp.iMove.moveCount = 0;
        endtemp.iMove.moveListPos.Add(new Vector3(col + i, row + j, 0)); //출발
        endtemp.iMove.moveListPos.Add(new Vector3(col, row, 0)); //도착

        endtemp.iMove.offset = GridManager.offset;
        endtemp.iState = JellyState.iState.isMoving;


        //그리드 
        GridManager.Grid[col + i, row + j] = starttemp;
        GridManager.Grid[col, row] = endtemp;

        starttemp.iPos.col = col + i;
        starttemp.iPos.row = row + j;

        endtemp.iPos.col = col;
        endtemp.iPos.row = row;
    }


    //jelly 로직에 사용할 체크로직
    public static void CheckJellyLogic(Vector3 start, int i, int j)
    {
        pGridManager.isFirstTime = true;
        pGridManager.isJellyLogicCheckNothing = false;
        pGridManager.tempJelly = GridManager.Grid[(int)start.x, (int)start.y];
        pGridManager.DragMoving(start, i, j);
        pGridManager.StartCoroutine(pGridManager.JellyCheck());

        if (pGridManager.isJellyLogicCheckNothing)
            pGridManager.StartCoroutine(pGridManager.reverseCheckLogic(start, i, j));
    }

    IEnumerator reverseCheckLogic(Vector3 start, int i, int j)
    {
        yield return new  WaitForEndOfFrame();
            pGridManager.DragMoving(start, i, j);

    }

    //탐색을 빨리하려면 해쉬테이블을 썼어야될꺼 같긴한데
    void itemCheck()
    {
        ////콤보 3일때
        foreach (Jelly jelly in Grid)
        {
            OneCheckJellyItem(jelly);
        }
    }

    void OneCheckJellyItem(Jelly jelly)
    {
        //가로 검사
        if (jelly.jellyDeath.CountColCombo == 3)
        {
            //무지개 젤리 예약
            if (jelly.jellyDeath.CountRowCombo > 0)
            {
                return;
            }

            //스프라이트 아이템으로 바꾸자
            if (jelly.jellyDeath.CountRowCombo == 0)
            {
                if (jelly.item == JellyState.itemJelly.none)
                    jelly.jellyDeath.isDeath = false; 

                jelly.item = JellyState.itemJelly.row; //아이템표시하고
                jelly.jellyDeath.CountColCombo = 0;
                jelly.jellyDeath.CountRowCombo = 0;

                itemNumberChange(jelly, (int)jelly.spriteIndex + 6);
                int thisCol = jelly.iPos.col;
                int thisRow = jelly.iPos.row;

                if (Grid[thisCol - 1, thisRow] != null && Grid[thisCol - 1, thisRow].jellyDeath.CountColCombo > 1)
                    Grid[thisCol - 1, thisRow].jellyDeath.CountColCombo = 1;
                if (Grid[thisCol + 1, thisRow] != null && Grid[thisCol + 1, thisRow].jellyDeath.CountColCombo > 1)
                    Grid[thisCol + 1, thisRow].jellyDeath.CountColCombo = 1;
                return;
            }
        }
        //세로검사
        else if (jelly.jellyDeath.CountRowCombo == 3)
        {
            //무지개 젤리 예약
            if (jelly.jellyDeath.CountColCombo > 0)
            {
                return;
            }

            //스프라이트 아이템으로 바꾸자
            if (jelly.jellyDeath.CountColCombo == 0)
            {
                if (jelly.item == JellyState.itemJelly.none)
                    jelly.jellyDeath.isDeath = false;

                jelly.item = JellyState.itemJelly.col; //아이템표시하고
                jelly.jellyDeath.CountColCombo = 0;
                jelly.jellyDeath.CountRowCombo = 0;

                itemNumberChange(jelly, (int)jelly.spriteIndex);
                int thisCol = jelly.iPos.col;
                int thisRow = jelly.iPos.row;

                if (Grid[thisCol, thisRow - 1] != null && Grid[thisCol, thisRow - 1].jellyDeath.CountRowCombo > 1)
                    Grid[thisCol, thisRow - 1].jellyDeath.CountRowCombo = 1;
                if (Grid[thisCol, thisRow + 1] != null && Grid[thisCol, thisRow + 1].jellyDeath.CountRowCombo > 1)
                    Grid[thisCol, thisRow + 1].jellyDeath.CountRowCombo = 1;
                return;
            }
        }

        else if (jelly.jellyDeath.CountColCombo == 2)
        {
            int thisCol = jelly.iPos.col;
            int thisRow = jelly.iPos.row;
 
            if (Grid[thisCol - 1, thisRow].jellyDeath.CountColCombo == 3 && Grid[thisCol - 1, thisRow] != null)
                return;
            if (Grid[thisCol + 1, thisRow].jellyDeath.CountColCombo == 3 && Grid[thisCol + 1, thisRow] != null)
                return;

            //무지개 젤리
            if (jelly.jellyDeath.CountRowCombo > 0)
            {
                return;
            }
            //스프라이트
            if (jelly.jellyDeath.CountRowCombo == 0)
            {
                if (jelly.item == JellyState.itemJelly.none)
                    jelly.jellyDeath.isDeath = false;

                jelly.item = JellyState.itemJelly.row; //아이템표시하고
                jelly.jellyDeath.CountColCombo = 0;
                jelly.jellyDeath.CountRowCombo = 0;

                itemNumberChange(jelly, (int)jelly.spriteIndex + 6);
                if (Grid[thisCol - 1, thisRow] != null && Grid[thisCol - 1, thisRow].jellyDeath.CountColCombo > 1)
                    Grid[thisCol - 1, thisRow].jellyDeath.CountColCombo = 1;
                if (Grid[thisCol + 1, thisRow] != null && Grid[thisCol + 1, thisRow].jellyDeath.CountColCombo > 1)
                    Grid[thisCol + 1, thisRow].jellyDeath.CountColCombo = 1;
                return;
            }

        }
        else if (jelly.jellyDeath.CountRowCombo == 2)
        {
            int thisCol = jelly.iPos.col;
            int thisRow = jelly.iPos.row;

            if (Grid[thisCol, thisRow - 1].jellyDeath.CountRowCombo == 3 && Grid[thisCol, thisRow - 1] != null)
                return;
            if (Grid[thisCol, thisRow + 1].jellyDeath.CountRowCombo == 3 && Grid[thisCol, thisRow + 1] != null)
                return;

            //무지개 젤리 예약
            if (jelly.jellyDeath.CountColCombo > 0)
            {
                return;
            }

            //스프라이트 아이템으로 바꾸자
            if (jelly.jellyDeath.CountColCombo == 0)
            {
                if (jelly.item == JellyState.itemJelly.none)
                    jelly.jellyDeath.isDeath = false;

                jelly.item = JellyState.itemJelly.col; //아이템표시하고
                jelly.jellyDeath.CountColCombo = 0;
                jelly.jellyDeath.CountRowCombo = 0;

                itemNumberChange(jelly, (int)jelly.spriteIndex);
                if (Grid[thisCol, thisRow - 1] != null && Grid[thisCol, thisRow - 1].jellyDeath.CountRowCombo > 1)
                    Grid[thisCol, thisRow - 1].jellyDeath.CountRowCombo = 1;
                if (Grid[thisCol, thisRow + 1] != null && Grid[thisCol, thisRow + 1].jellyDeath.CountRowCombo > 1)
                    Grid[thisCol, thisRow + 1].jellyDeath.CountRowCombo = 1;
                return;
            }
        }
        //무지개 젤리
        else if (jelly.jellyDeath.CountColCombo == 1 && jelly.jellyDeath.CountRowCombo == 1)
        {
            return;
        }

    }



    void itemNumberChange(Jelly jelly,int i )
    {
        jelly.jellyDeath.CountColCombo = 0;
        jelly.jellyDeath.isDeath = false; //죽음을 무효화하고

        jelly.ChangeItemSpriteNumber(i); //아이템으로 둔갑한다 +6은 세로줄
    }

    //아이템을 지운다면
    void itemNumberDistroy(Jelly jelly)
    {
        if (jelly.item == JellyState.itemJelly.none)
            return;

        if (jelly.item == JellyState.itemJelly.col) //행 삭제
        {
            int thisRow = jelly.iPos.row;
            
            for (int i  = 0; i < MaxRow; ++i)
            {
                if (Grid[i, thisRow] != null&& Grid[i, thisRow].spriteIndex!=JellyState.SpriteColor.none)
                {
                    Jelly temp = Grid[i, thisRow];
                    Grid[i, thisRow] = null;
                    count += 10;
                    Destroy(temp.gameObject);
                }
            }
        }

        if (jelly.item == JellyState.itemJelly.row) //열 삭제
        {
            int thisCol = jelly.iPos.col;
            for (int j = 0; j < MaxRow; ++j)
            {
                if (Grid[thisCol, j] != null && Grid[thisCol, j].spriteIndex != JellyState.SpriteColor.none)
                {
                    Jelly temp = Grid[thisCol, j];
                    Grid[thisCol, j] = null;
                    count += 10;
                    Destroy(temp.gameObject);
                }
            }
        }

    }


    IEnumerator UIChange()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (pGridManager == null)
            {
                break;
            }

            text.text = "Score: " + count.ToString();
            curTime -= Time.deltaTime;
            if (timeBar.fillAmount > 0)
                timeBar.fillAmount = (curTime) / 120f;
            else
                timeBar.fillAmount = 0;

            //timeBar
            if (spriteCount >= timeImage.Count)
                spriteCount %= timeImage.Count;

            timeBar.sprite = timeImage[spriteCount];
            spriteCount++;


        }

    }


    void leftSideAlignment(int iCol)
    {
        for (int i = 0 ; i < MaxRow; ++i)
        {
            for (int j = i + 1; j < MaxRow; ++j)
            {

                if (Grid[iCol, i]==null && Grid[iCol, j]!=null)
                {
                    Grid[iCol, j].iState = JellyState.iState.isMoving; //이동으로 변화시키고
                    Grid[iCol, j].iMove.step = 0;
                    //이동관련
                    Grid[iCol, j].iMove.moveCount = 0;
                    if (Grid[iCol, j].iMove.moveListPos.Count != 0)
                        Grid[iCol, j].iMove.moveListPos.Clear();
                    Grid[iCol, j].iMove.moveListPos.Add(new Vector3(iCol, j, 0)); //출발
                    Grid[iCol, j].iMove.moveListPos.Add(new Vector3(iCol, i, 0)); //도착
                    Grid[iCol, j].iMove.offset = offset;

                    Grid[iCol, j].iPos.col = i;
                    Grid[iCol, j].iPos.row = j;

                    //그리드 설정
                    Grid[iCol, i] = Grid[iCol, j];
                    Grid[iCol, j] = null;
                    break;
                }
            }
        }




    }



}
