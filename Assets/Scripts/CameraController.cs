using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public IUserInput pi;
    public float HorizontalSpeed;
    public float VerticalSpeed;
    public float CameraDamp;
    public Image lockDot;
    public bool lockState = false;
    private GameObject _playerHandle;
    private GameObject _cameraHandle;
    private GameObject _camera;
    private GameObject _model;
    private Vector3 _modelEuler;
    private Vector3 _cameraDampVelocity;
    private GameObject _lockTarget;
    
    private float _tmpEulerX;
    // Start is called before the first frame update
    void Awake()
    {
        _cameraHandle = transform.parent.gameObject;
        _playerHandle = _cameraHandle.transform.parent.gameObject;
        _camera = Camera.main.gameObject;
        _model = _playerHandle.GetComponent<ActorController>().model;
        _tmpEulerX = 20f;
        Cursor.lockState = CursorLockMode.Locked;
        lockDot.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (null == _lockTarget)
        {
            _modelEuler = _model.transform.eulerAngles;
            _playerHandle.transform.Rotate(Vector3.up, pi.jRight * HorizontalSpeed * Time.fixedDeltaTime);
            _tmpEulerX -= VerticalSpeed * Time.fixedDeltaTime * pi.jUp;
            _cameraHandle.transform.localEulerAngles = new Vector3(Mathf.Clamp(_tmpEulerX, -40, 30), 0, 0);
            _model.transform.eulerAngles = _modelEuler;
        }
        else
        {
            Vector3 tmpForward = _lockTarget.transform.position - _model.transform.position;
            tmpForward.y = 0;
            _playerHandle.transform.forward = tmpForward;
        }


        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, transform.position, ref _cameraDampVelocity, CameraDamp);
        //_camera.transform.eulerAngles = transform.eulerAngles;
        _camera.transform.LookAt(_cameraHandle.transform);
    }

    public void LockUnLock()
    {
        
        if(_lockTarget == null)
        {
            //to lock sth
            Vector3 center = _model.transform.position + new Vector3(0, 1, 0);
            Vector3 boxCenter = center + _model.transform.forward * 5f;
            Collider[] colliders = Physics.OverlapBox(center, new Vector3(0.5f, 0.5f, 10.0f), _model.transform.rotation, LayerMask.GetMask("Enemy"));
            foreach (var col in colliders)
            {
                print(col);
                lockDot.enabled = true;
                _lockTarget = col.gameObject;
                lockState = true;
                break;
            }
        }
        else
        {
            //release lock
            _lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
    }
}
