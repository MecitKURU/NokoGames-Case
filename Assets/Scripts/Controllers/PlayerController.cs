using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Player
{
    public FloatingJoystick Joystick;

    public override void Update()
    {
        base.Update();
        Movement();
    }

    private void Movement()
    {
        Vector3 direction = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
        transform.position += direction * Time.deltaTime * MovementSpeed;
        transform.forward += direction * Time.deltaTime * RotationSpeed;

        float speed = Joystick.Direction.magnitude;
        currentSpeed = speed;
        Animator.SetFloat("Movement", speed);
    }
}
