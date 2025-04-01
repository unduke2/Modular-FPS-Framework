using UnityEngine;
public class WeaponState
{
    //Magazine
    public int CurrentAmmo;
    public bool IsReloading;

    //Transform
    public Transform MuzzleTransform;
    public Transform RightHandIKTarget;
    public Transform LeftHandIKTarget;


    public CameraController HolderCamera;

    public bool IsADS;
    public float ADSFOV;

    public AudioSource GunshotAudioSource;


}
