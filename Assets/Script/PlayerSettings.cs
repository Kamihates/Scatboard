using UnityEngine;

[System.Serializable]
public struct Movement
{
    public float maxSpeed;
    public float acceleration;
}

[System.Serializable]
public struct Jump
{
    public float jumpPower;
    public LayerMask groundLayer;
    public Transform groundCheck;
}

[System.Serializable]
public struct Fly
{
    public float duration;
}

[System.Serializable]
public struct RampSlide
{
    public LayerMask rampLayer;
    public Transform rampCheck;
}