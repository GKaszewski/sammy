namespace Sammy
{
    public class CrystalManager
    {
        public CrystalColor Mix(CrystalColor currentColor, CrystalColor newColor)
        {
            switch (currentColor)
            {
                case CrystalColor.RED:
                    if (newColor == CrystalColor.BLUE) return CrystalColor.PURPLE;
                    if (newColor == CrystalColor.YELLOW) return CrystalColor.ORANGE;
                    break;
                case CrystalColor.BLUE:
                    if (newColor == CrystalColor.RED) return CrystalColor.PURPLE;
                    if (newColor == CrystalColor.YELLOW) return CrystalColor.GREEN;
                    break;
                case CrystalColor.YELLOW:
                    if (newColor == CrystalColor.RED) return CrystalColor.ORANGE;
                    if (newColor == CrystalColor.BLUE) return CrystalColor.GREEN;
                    break;
                case CrystalColor.MULTI:
                    return CrystalColor.MULTI;
            }
            
            return CrystalColor.NONE;
        }
    }
}