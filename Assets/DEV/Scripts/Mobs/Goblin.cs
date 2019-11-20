using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Mob
{
  
    private void OnEnable()
    {
        SetRequireComponents();
    }
    public SkinnedMeshRenderer skinRenderer;
    public List<Material> materials;
    //Virtual voids
    
    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
    }
    public override IEnumerator TakeDamageOverTime(float dPerSec, float t, float interval)
    {
        return base.TakeDamageOverTime(dPerSec, t, interval);
    }
    protected override void SetRequireComponents()
    {
        base.SetRequireComponents();
    }
    public override void Attack()
    {
        base.Attack();
    }
    protected override void ActivateNavMeshAgent()
    {
        base.ActivateNavMeshAgent();
    }
    protected override void DeactivateNavMeshAgent()
    {
        base.DeactivateNavMeshAgent();
    }
    protected override void SetAgent(int state)
    {
        base.SetAgent(state);
    }
}
