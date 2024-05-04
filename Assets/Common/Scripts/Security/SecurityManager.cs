using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityManager : SingletonClass<SecurityManager>
{
    public List<SecurityController> securities = new List<SecurityController>();

    public int RaysForPlayer = 5;
    public int MinimumRaysToHitForPlayer = 3;
    public int RaysForItem = 5;
    public int MinimumRaysToHitForItem = 3;
}
