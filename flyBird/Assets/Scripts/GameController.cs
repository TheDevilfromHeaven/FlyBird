using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    //管道预制作
    public GameObject pipesPrefabs;
    public Bird player;
    public Transform birdSpawn;
    public ScoreBoard scoreBoard;

    public Text countText;

    public GameObject welcomeNode;
    public GameObject readyStartNode;
    public GameObject gameOverNode;

    //管道产生的频率，每几秒产生一个
    public float createPipesRate = 3f;

    //管道中心位置的y最小值
    public float minPipPosY = -1f;
    //管道中心位置的y最大值
    public float maxPipPosY = 4f;
    //初始化管道的位置，x最好为负数不可见位置
    public Vector2 startPipPos = new Vector2(-12f, 0f);

    //统计已经成功过了几个管道
    private int count = 0;
    //小鸟是否已经死了
    public GameState state { get; private set; }

    public enum GameState
    {
        NotReady,
        Ready,
        Playing,
        Over
    }

    //上一次创建出管道的时间
    private float lastCreatePipTime = float.NegativeInfinity;

    //缓存管道的链表，用来复用管道
    private List<GameObject> pipes = new List<GameObject>();

    //管道缓存的个数
    private const int PIPESTOTAL = 8;
    //当前管道下标，用来更新管道
    private int currPipesIndex = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }

        welcomeNode.SetActive(true);
        readyStartNode.SetActive(false);
        gameOverNode.SetActive(false);
    }

    private void Start()
    {
        state = GameState.NotReady;
        player.SetFree();
        //开始时，创建出管道缓存
        InitPipesPool();
        setCountText();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.NotReady:

            case GameState.Ready:
                if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
                {
                    GameStart();
                }
                break;

            case GameState.Playing:
                //当可以创建出管道时，拿出缓存中一个管道来更新位置
                if (lastCreatePipTime + createPipesRate < Time.time)
                {
                    lastCreatePipTime = Time.time;
                    UpdatePipesPosition();
                    currPipesIndex = (currPipesIndex + 1) % PIPESTOTAL;
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    player.Fly();
                }
                break;

            case GameState.Over:
                break;
                //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
                //{
                //    GameStart();
                //}
                //break;
        }

    }

    public void PassOnePip()
    {
        CountScore(++count);
    }

    private void CountScore(int score)
    {
        countText.text = "分数:" + count.ToString();
    }

    public void setCountText()
    {
        countText.text = "分数:" + count.ToString();
    }

    public void GameReadyStart()
    {
        if (state != GameState.NotReady && state != GameState.Over) return;
        welcomeNode.SetActive(false);
        readyStartNode.SetActive(true);
        state = GameState.Ready;
    }

    public void GameStart()
    {
        if (state != GameState.Ready) return;
        readyStartNode.SetActive(false);
        player.SetControl();
        state = GameState.Playing;
    }

    public void GameOver()
    {
        if (state != GameState.Playing) return;
        readyStartNode.SetActive(false);
        gameOverNode.SetActive(true);
        SoundManager.instance.PlayDie();
        ReportScore();
        state = GameState.Over;
    }

    public void GameRestart()
    {
        readyStartNode.SetActive(true);
        gameOverNode.SetActive(false);

        count = 0;
        CountScore(0);

        foreach(GameObject pipe in pipes)
        {
            Destroy(pipe);
        }
        pipes.Clear();
        InitPipesPool();

        player.SetFree();
        player.transform.position = new Vector3(-8, 0, 0);
        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        state = GameState.Ready;
    }

    //初始化管道缓存池
    private void InitPipesPool()
    {
        for(int i = 0; i < PIPESTOTAL; ++i)
        {
            GameObject obj = Instantiate(pipesPrefabs, startPipPos, Quaternion.identity);
            pipes.Add(obj);
        }
    }

    //更新当前管道的位置
    private void UpdatePipesPosition()
    {
        float randomPosY = Random.Range(minPipPosY, maxPipPosY);
        Vector2 position = new Vector2(10f, randomPosY);
        pipes[currPipesIndex].transform.position = position;
    }

    private void ReportScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        if(count > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", count);
        }
        scoreBoard.SetScore(count);
        scoreBoard.SetBestScore(bestScore);
        scoreBoard.CreateRating();
    }
}
