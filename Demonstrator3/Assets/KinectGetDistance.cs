using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectGetDistance : MonoBehaviour {
    private Body[] _Data = null;
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;

    public float _distance;


    // Use this for initialization
    void Start () {
        _Sensor = KinectSensor.GetDefault();
        
        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }

        _distance = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        


        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_Data);

                frame.Dispose();
                frame = null;
            }
        }

        _distance = -1;

        Body[] data = _Data;
        if (data == null)
        {
            return;
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {                
                var distance = body.Joints[JointType.SpineBase].Position.Z;
                //Debug.Log(distance);
                _distance = distance;
            }
        }


    }

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }

}
