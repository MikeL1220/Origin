using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField]
    private int _cameraShakeIntens;

    private Vector3 _primaryCamPos; 
    

    // Start is called before the first frame update
    void Start()
    {
        _primaryCamPos = new Vector3(0, -0.75f, -10); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  public IEnumerator CameraShake()
    {
        transform.Translate(Vector3.up * _cameraShakeIntens * Time.deltaTime);
        transform.Translate(Vector3.left * _cameraShakeIntens * Time.deltaTime); 
        yield return new WaitForSeconds(.1F);
        transform.position = _primaryCamPos;
        transform.Translate(Vector3.down * _cameraShakeIntens * Time.deltaTime); 
        transform.Translate(Vector3.right * _cameraShakeIntens *Time.deltaTime);
        yield return new WaitForSeconds(.1f);
        transform.position = _primaryCamPos;


    }
}
