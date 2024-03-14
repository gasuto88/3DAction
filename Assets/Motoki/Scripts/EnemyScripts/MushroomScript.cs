/*-------------------------------------------------
* MushroomScript.cs
* 
* 作成日　2024/03/14
* 更新日　2024/03/14
*
* 作成者　本木大地
-------------------------------------------------*/
using UnityEngine;

public class MushroomScript : EnemyControlScript
{

    protected override void EnemyControl()
    {
        FallInGravity();
    }
}