

public interface IExplodeable
{
    int radius {get; set;}
    int damage {get; set;}
    
    void Explode();
    void DamageInArea(int _radius, int _damage);
}
