using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public int team
   {
      get;
      set;
   }

    public void Hit(Vector2 dir, float force = 0, int team = 0);
}
