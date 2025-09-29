namespace Sammy.Interfaces
{
    public interface IUIView
    {
        void UpdatePoints(int current, int max);
        void UpdateWaspCount(int count);
        void UpdateCrystal(CrystalColor color);
    }
}