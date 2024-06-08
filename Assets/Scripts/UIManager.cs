using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private int _score;
    [SerializeField]
    private int _scoreIncrease;


    [SerializeField]
    private TMP_Text _scoreText;

    [SerializeField]
    private TMP_Text _ammoCount;
    [SerializeField]
    private int _ammo; 

    [SerializeField]
    private Sprite[] _livesDisplay;
    [SerializeField]
    private Sprite _currentLife;

    [SerializeField]
    private Sprite[] _shieldHealthVisualizer;
    [SerializeField]
    private GameObject _shieldHealthObj; 

    private Sprite _currentShieldHealthVisualizer;

    private Slider _thrusterMeter; 

    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private GameObject _gameOverDisplay;
    [SerializeField]
    private bool _gameIsOver;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score: " + _score;

        _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];
        _gameOverDisplay.SetActive(false);

        _currentShieldHealthVisualizer = GameObject.Find("Shield_Health").GetComponent<SpriteRenderer>().sprite = _shieldHealthVisualizer[0];

        _thrusterMeter = GameObject.Find("Thruster_Charge").GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;
        _ammoCount.text = _ammo + "/15";
        RestartGame();
        ExitGame();

    }
    public void UpdateScore()
    {
        _score += _scoreIncrease;


    }
    public void UpdateLifeDisplay(int currentLife)
    {

        switch (currentLife)
        {
            case 0:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[3];
                break;
            case 1:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[2];
                break;
            case 2:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[1];
                break;
            case 3:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];
                break;
        }
    }

    public void ReduceAmmoCount()
    {
        _ammo--; 
    }

    public void ResetAmmoCount()
    {
        _ammo = 15; 
    }

    public void ShieldHealthVisualizer(int shieldHealth)
    { 
        switch(shieldHealth) 
        {
            case 1:
                _currentShieldHealthVisualizer = GameObject.Find("Shield_Health").GetComponent<SpriteRenderer>().sprite = _shieldHealthVisualizer[3];
               _shieldHealthObj.transform.position = new Vector3(-7.2f, 4.21f, 0.02f); 
                break;
            case 2:
                _currentShieldHealthVisualizer = GameObject.Find("Shield_Health").GetComponent<SpriteRenderer>().sprite = _shieldHealthVisualizer[2];
                _shieldHealthObj.transform.position = new Vector3(-6.8f, 4.21f, 0.02f);
                break;
            case 3:
                _currentShieldHealthVisualizer = GameObject.Find("Shield_Health").GetComponent<SpriteRenderer>().sprite = _shieldHealthVisualizer[1];
                _shieldHealthObj.transform.position = new Vector3(-6.51f, 4.21f, 0.02f);
                break;
            default:
                _currentShieldHealthVisualizer = GameObject.Find("Shield_Health").GetComponent<SpriteRenderer>().sprite = _shieldHealthVisualizer[0];
                break;

        }
    
    
    }

    public void ThrusterMeter(int thrusterCharge)
    {
        _thrusterMeter.value = thrusterCharge;
    }


    public IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameIsOver = true;
            _gameOverDisplay.SetActive(true);
            yield return new WaitForSeconds(1);
            _gameOverDisplay.SetActive(false);
            yield return new WaitForSeconds(1);

        }




    }
    private void RestartGame()
    {
        if (_gameIsOver == true && Input.GetKeyDown(KeyCode.R))
        {

            SceneManager.LoadScene(1);
            _gameIsOver = false;

        }

    }

    private void ExitGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        { 
        
            Application.Quit();
        
        }
    }
}
