using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISprintState
{
    public bool isSprinting { get; }
    public void startSprint();
    public void stopSprint();
}
