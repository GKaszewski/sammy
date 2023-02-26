using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    DEATH,
    HIT,
    RUN,
    JUMP,
    POOF
}

public class EffectsManager : MonoBehaviour {
    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject runEffect;
    public GameObject jumpEffect;
    public GameObject poofEffect;
    
    public void SpawnEffect(EffectType effectType, Vector3 position, float time = 5f) {
        GameObject effect = null;
        switch (effectType) {
            case EffectType.DEATH:
                effect = Instantiate(deathEffect, position, Quaternion.identity);
                break;
            case EffectType.HIT:
                effect = Instantiate(hitEffect, position, Quaternion.identity);
                break;
            case EffectType.RUN:
                effect = Instantiate(runEffect, position, Quaternion.identity);
                break;
            case EffectType.JUMP:
                effect = Instantiate(jumpEffect, position, Quaternion.identity);
                break;
            case EffectType.POOF:
                effect = Instantiate(poofEffect, position, Quaternion.identity);
                break;
        }
        if (effect) Destroy(effect, time);
    }
}
