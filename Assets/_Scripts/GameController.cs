using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//refernce to the UI namespace
using UnityEngine.UI;
/**
 * StudentID: 300833478
 * Date: 07/11/2016
 */
public class GameController : MonoBehaviour {
    // Private Instance Variables
    private float _time;
    private bool _isGameOver;
    private bool _isGamePause;
    private float _fillAmount;
    private bool _isLightOn;
    private float _lightpausevalue;
    private float timeBetweenFires = 1.0f;
    private float timeTilNextFire = 0.0f;
    private GameObject _respawnPoint;
    private GameObject[] _spooks;
    private bool _flickingstarted;
    private int _amountOfSpooks;
    private GameManager _gameManager;


    // PUBLIC INSTANCE VARIABLES
    public GameObject Spook;
    public AudioSource GamePlaySound;
    public AudioSource OutOfBattery;
    public AudioSource SpookLaugh;
    public AudioSource FlickerLight;
    public Image BatteryBar;
    public Light Light;
    public Light MiniMapLight;
    public int AmountOfSpooks
    {
        get
        {
            return this._amountOfSpooks;
        }
        set
        {
            this._amountOfSpooks = value;
        }
    }
    public bool IsGameOver
    {
        get
        {
            return this._isGameOver;
        }
        set
        {
            this._isGameOver = value;
            if(_isGameOver)
            {
                IsGamePause = true;
                GameOverLable.gameObject.SetActive(true);
                GamePlaySound.Stop();
                OutOfBattery.Play();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PlayerPrefs.Save();
                Invoke("BackToMainScreen", 5);
            }
        }
    }
    public bool IsGamePause
    {
        get
        {
            return this._isGamePause;
        }
        set
        {
            this._isGamePause = value;
            if(_isGamePause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }
    }

  
    public float TimeValue
    {
        get
        {
            return this._time;
        }
        set
        {
            this._time = value;
                this.TimeLable.text = Mathf.Round(this._time).ToString();
        }
    }
    public float FillAmount
    {
        get
        {
            return this._fillAmount;
        }
        set
        {
            this._fillAmount = value;
            if (_fillAmount <= 0f)
            {
                Light.intensity = 0;
                IsGameOver = true;
                IsGamePause = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    [Header("Menu")]
    public Text TimeLable;
    public Text MenuTitle;
    public Button BackToMainMenu;
    public Button Resume;
    [Header("GameOver")]
    public Text GameOverLable;

	// Use this for initialization
	void Start () {
        Initialize();
        _spawnSpooks();
	}
    
	// Update is called once per frame
	void Update () {
        if (!IsGamePause)
        {
            this.TimeValue += Time.deltaTime;
            _updateBattery();
            timeTilNextFire -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Escape)&&!IsGameOver)
        {
            _bringUpMenu();
            _lightpausevalue = FillAmount;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            if (this._isLightOn)
            {
                Light.intensity = 0;
                MiniMapLight.intensity = 0;
                this._isLightOn = false;
            }
            else
            {
                Light.intensity = 4;
                MiniMapLight.intensity = 2;
                this._isLightOn = true;
            }
        }
        //Saves score to memory
        PlayerPrefs.SetFloat("Score", TimeValue);
        if (_amountOfSpooks == 0)
        {
            _amountOfSpooks = 5;
            _spooks = new GameObject[_amountOfSpooks];
            _spawnSpooks();
        }
    }
    // Use this for initialization
    void Initialize()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _respawnPoint = GameObject.Find("SpookSpawn");
        this.TimeValue = 0.0f;
        MenuTitle.gameObject.SetActive(false);
        BackToMainMenu.gameObject.SetActive(false);
        Resume.gameObject.SetActive(false);
        GameOverLable.gameObject.SetActive(false);
        this.IsGamePause = false;
        this.IsGameOver = false;
        this._isLightOn = true;
        this.FillAmount = 1f;
        _amountOfSpooks = _gameManager.AmountOfSpooks;
        _spooks = new GameObject[_amountOfSpooks];
       
    }
    // Private METHODS*******************************
    private void _spawnSpooks()
    {
        for(int i = 0; i < _spooks.Length;i++)
        {
            _spooks[i] = Instantiate(Spook, _respawnPoint.transform.position,_respawnPoint.transform.rotation);
        }
    }
    private void _bringUpMenu()
    {
        IsGamePause = true;
        MenuTitle.gameObject.SetActive(true);
        BackToMainMenu.gameObject.SetActive(true);
        Resume.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void _updateBattery()
    {
        if (timeTilNextFire < 0)
        {
            if (_isLightOn && !_isGameOver)
            {
                FillAmount -= 0.02f;
                timeTilNextFire = timeBetweenFires;
            }
        }
        BatteryBar.fillAmount = _fillAmount;
        if (BatteryBar.fillAmount < 0.30 && BatteryBar.fillAmount > 0.01)
        {
            _startFlicker();
        }
    }
    private void _startFlicker()
    {
        if (!_flickingstarted)
        {
            StartCoroutine(Fliker());
        }
        _flickingstarted=true;
    }
    IEnumerator Fliker()
    {
        yield return new WaitForSeconds(0.7f);
        Light.intensity = 0;
        MiniMapLight.intensity = 0;
        FlickerLight.Play();
        yield return new WaitForSeconds(0.7f);
        MiniMapLight.intensity = 2;
        Light.intensity = 4;
        _flickingstarted = false;
    }

  

    // Public METHODS*******************************
    
    
    public void BackToMainScreen()
    {
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BringDownMenu()
    {
        IsGamePause = false;
        MenuTitle.gameObject.SetActive(false);
        BackToMainMenu.gameObject.SetActive(false);
        Resume.gameObject.SetActive(false);
        BatteryBar.fillAmount = _lightpausevalue;
    }
}
