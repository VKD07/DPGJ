namespace Code
{
    public interface IBurnable
    {
        public bool IsBurning { get; set; }
        public void Ignite();
        public void Extinguish(float depleteVa);
    }
}