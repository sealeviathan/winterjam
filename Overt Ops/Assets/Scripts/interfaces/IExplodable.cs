

public interface IExplodeable
{
    int radius {get; set;}
    int damage {get; set;}
    float force {get; set;}
    
    void Explode();
    void DamageInArea(int _radius, int _damage, float _force);
    void Kill();
}
