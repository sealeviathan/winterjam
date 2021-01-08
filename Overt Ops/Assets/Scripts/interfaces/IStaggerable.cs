public interface IStaggerable
{
    //This is an interface that the weapon class uses to interact with the environment.
    //Specifically, IStaggerable allows a weapon to cause a stagger of some kind, generally
    //To entities.
    void Stagger(float timeAmount);
    void FallDown();
}
