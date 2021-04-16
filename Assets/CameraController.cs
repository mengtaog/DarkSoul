using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float HorizontalSpeed;
    public float VerticalSpeed;
    public float CameraDamp;
    private GameObject _playerHandle;
    private GameObject _cameraHandle;
    private GameObject _camera;
    private GameObject _model;
    private Vector3 _modelEuler;
    private Vector3 _cameraDampVelocity;
    
    private float _tmpEulerX;
    // Start is called before the first frame update
    void Awake()
    {
        _cameraHandle = transform.parent.gameObject;
        _playerHandle = _cameraHandle.transform.parent.gameObject;
        _camera = Camera.main.gameObject;
        _model = _playerHandle.GetComponent<ActorController>().model;
        _tmpEulerX = 20f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _modelEuler = _model.transform.eulerAngles;
        _playerHandle.transform.Rotate(Vector3.up, pi.jRight * HorizontalSpeed * Time.fixedDeltaTime);
        _tmpEulerX -= VerticalSpeed * Time.fixedDeltaTime * pi.jUp;
        _cameraHandle.transform.localEulerAngles = new Vector3(Mathf.Clamp(_tmpEulerX, -40, 30), 0, 0);
        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position, ref _cameraDampVelocity, CameraDamp);
        _camera.transform.eulerAngles = transform.eulerAngles;
        _model.transform.eulerAngles = _modelEuler;
    }
}
