

public interface IDamageable
{
    int health {get; set;}
    int maxHealth {get; set;}
    void Damage(int amount);
}
