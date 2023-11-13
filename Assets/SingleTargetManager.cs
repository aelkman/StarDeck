using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetManager : MonoBehaviour
{
    public BaseCharacterInfo currentTarget;
    public bool targetLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearTarget() {
        if(currentTarget != null) {
            ((BattleEnemyContainer)currentTarget).selectorAnimator.gameObject.SetActive(false);
        }
        currentTarget = null;
    }

    public void SetTarget(BaseCharacterInfo target) {
        currentTarget = target;
        ((BattleEnemyContainer)target).selectorAnimator.gameObject.SetActive(true);
    }

    public BaseCharacterInfo GetTarget() {
        return currentTarget;
    }
}
