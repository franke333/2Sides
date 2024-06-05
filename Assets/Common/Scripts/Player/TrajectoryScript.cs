using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryScript : MonoBehaviour
{
    TrajectoryPredictorScript trajectoryPredictor;

    [SerializeField]
    Rigidbody objectToThrow;

    [SerializeField]
    Transform StartPosition;

    float throwForce;

    void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictorScript>();

        if (StartPosition == null)
            StartPosition = transform;

    }

    private void OnDisable()
    {
        trajectoryPredictor.SetTrajectoryVisible(false);
    }

    public void SetObjectToThrow(Rigidbody obj)
    {
        objectToThrow = obj;
        if(obj == null)
        {
            trajectoryPredictor.SetTrajectoryVisible(false);
            return;
        }
        if (obj.TryGetComponent<ItemScript>(out var iscript))
            throwForce = iscript.throwForce;
        else if(obj.TryGetComponent<TriggerSourceAudio>(out var tsa))
            throwForce = tsa.throwForce;
        else if(obj.TryGetComponent<MopScript>(out var mop))
            throwForce = mop.throwForce;
        trajectoryPredictor.SetTrajectoryVisible(true);
    }

    void Update()
    {
        if(objectToThrow != null)
            Predict();
        else
            trajectoryPredictor.SetTrajectoryVisible(false);
    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        properties.direction = PlayerCameraScript.Instance.GetViewVector();
        properties.initialPosition = StartPosition.position + 0.2f * properties.direction;
        properties.initialSpeed = throwForce/50;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }
}

public struct ProjectileProperties
{
    public Vector3 direction;
    public Vector3 initialPosition;
    public float initialSpeed;
    public float mass;
    public float drag;
}
