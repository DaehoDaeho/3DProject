using UnityEngine;
using UnityEngine.Events;

/// <summary>공통 피해 인터페이스</summary>
public interface IDamageable
{
    void TakeDamage(int amount, Vector3 hitPoint);
}