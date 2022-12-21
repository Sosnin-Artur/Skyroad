using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives = 1;    

    
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private FirebaseManager _firebase;
         
    private float _speedModifier = 1.0f;

    private int _score = 0;
    private int _hightScore = 0;
    private int _time = 0;
    private int _hightTime = 0;
    private int _asteroidsCount = 0;
    private int _hightActeroidsCount = 0;    

    private bool _isGameStarted = false;
    public GameObject _canvas;
    
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this);
            // DontDestroyOnLoad(this);
            //DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this.gameObject);
        }

        LoadData();
    }    

    private void Start()
    {            
        Time.timeScale = 0;
        //Cursor.visible = false;                
        //hints.SetActive(true);
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
    }
    
    private void Update()
    {        
        if (Input.anyKey && !_isGameStarted)
        {         
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveData();
            Application.Quit();
        }
    }    

    public void StartGame()
    {
        Debug.Log("Start game");
        _score = 0;
        scoreText.text = "0";
        _canvas.SetActive(false);
        mainMenu.SetActive(false);
        _isGameStarted = true;
        Time.timeScale = 1;
        StartCoroutine(Timer());
    }

    public void OpenMainMenu()
    {

    }

    private IEnumerator GameOver()
    {
        SaveData();
        Cursor.visible = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;       
        yield return new WaitForSeconds(2.0f);        
    }

    private IEnumerator Timer()
    {   
        while (true)
        {
            yield return new WaitForSeconds(1.0f);                
            _time++;
            AddScore((int)(1 * _speedModifier));
            if (_time > _hightTime)
            {
                _hightTime = _time;                
            }        
        }                             
    }

    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
            + "/SaveData.dat"); 
        
        SaveData data = new SaveData();
        data.HightTime = _hightTime;
        data.HightAsteroidsCount = _hightActeroidsCount;
        data.HightScore = _hightScore;        
        bf.Serialize(file, data);
        file.Close();


        Debug.Log(_score);
        _firebase.UpdateData(_score);
    }

    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath 
            + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
            File.Open(Application.persistentDataPath 
            + "/SaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            
            _hightTime = data.HightTime;
            _hightScore = data.HightScore;            
            _hightActeroidsCount = data.HightAsteroidsCount;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public void AddScore(int score)
    {
        _score += score;        
        if (_score > _hightScore)
        {
            _hightScore = _score;
        }
        scoreText.text = _score.ToString();
    }

    public void AddAsteroid(int value)
    {
        _asteroidsCount += value;        
        if (_asteroidsCount > _hightActeroidsCount)
        {
            _hightActeroidsCount = _asteroidsCount;
        }
    }
    
    public void AddLives(int value)
    {
        lives += value;        

        if (lives <= 0)
        {
            StartCoroutine(GameOver());
        }        
    }

    public void Restart()
    {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
        _canvas.SetActive(true);

        SceneManager.LoadScene("Game");

    }    

    public void Boost(float boost)
    {        
        _speedModifier = boost;        
    }
}
