using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//refernce to the UI namespace
using UnityEngine.UI;
/**
 * StudentID: 300833478
 * Date: 07/11/2016
 */
public class MainMenuScript : MonoBehaviour {
    private GameObject _gameManagerObject;
    private GameManager _gameManager;
    // PUBLIC INSTANCE VARIABLES
    public AudioSource MainMenuSound;
    public Dropdown DifficultySelector;

    [Header("Text")]
    public Text VersionLabel;

    [Header("Buttons")]
    public Button StartButton;
    public Button ExitButton;
    // Use this for initialization
    void Start () {
        VersionLabel.text = "Version: " + Application.version;
        Cursor.visible = true;
        this._gameManagerObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerObject.GetComponent<GameManager>();
        DifficultySelector.onValueChanged.AddListener(delegate { SelectedDifficulty(DifficultySelector); });
        //setting default
        _gameManager.AmountOfSpooks = 5;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    // PUBLIC METHODS
    public void Start_Game()
    {
        MainMenuSound.Stop();
        SceneManager.LoadScene("Main");
    }
    public void Close_Game()
    {
        Application.Quit();
    }
    void SelectedDifficulty(Dropdown target)
    {
        switch (target.value)
        {
            case 0:
                break;
            case 1:
                _gameManager.AmountOfSpooks = 5;
                break;
            case 2:
                _gameManager.AmountOfSpooks = 7;
                break;
            case 3:
                _gameManager.AmountOfSpooks = 10;
                break;
        }
    }
}
