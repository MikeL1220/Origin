using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

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
   
    private Image _currentLife;

    [SerializeField]
    private Sprite[] _shieldHealthVisualizer;
  

    [SerializeField]
    private Image _currentShieldHealthVisualizer;

    private Slider _thrusterMeter;

    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private GameObject _gameOverDisplay;
    [SerializeField]
    private bool _gameIsOver;


    void Start()
    {
      
        _scoreText.text = "Score: " + _score;

        
        _gameOverDisplay.SetActive(false);

        _currentShieldHealthVisualizer = GameObject.Find("Shield_Life").GetComponent<Image>();

        _thrusterMeter = GameObject.Find("Thruster_Charge").GetComponentInChildren<Slider>();

       
      
        _currentLife = GameObject.Find("Player_Lives").GetComponent<Image>();
      
    }


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
                _currentLife.sprite = _livesDisplay[3];
                break;
            case 1:
                _currentLife.sprite = _livesDisplay[2];
                break;
            case 2:
                _currentLife.sprite =_livesDisplay[1];
                break;
            case 3:
                _currentLife.sprite = _livesDisplay[0];
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
        switch (shieldHealth)
        {
            case 1:
                _currentShieldHealthVisualizer.sprite = _shieldHealthVisualizer[3];
                
                break;
            case 2:
                _currentShieldHealthVisualizer.sprite = _shieldHealthVisualizer[2];
                
                
                break;
            case 3:
                _currentShieldHealthVisualizer.sprite = _shieldHealthVisualizer[1];
               

                break;
            case 0:
                _currentShieldHealthVisualizer.sprite = _shieldHealthVisualizer[0];
                

                break;
            default:
                _currentShieldHealthVisualizer.sprite = _shieldHealthVisualizer[0];
               
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

        }
    }
}
