using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }
}
