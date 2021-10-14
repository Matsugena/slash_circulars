using UnityEngine;

public class AttackData {
    public int Damage;
    public int objectId { get; }
    public AttackData(int damage, GameObject obj) {
        this.Damage = damage;
        this.objectId = obj.GetInstanceID();
    }
}