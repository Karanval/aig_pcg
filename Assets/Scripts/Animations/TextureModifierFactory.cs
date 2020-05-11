public class TextureModifierFactory
{
    public static ITextureModifier CreateTextureModifier(NoiseAnimator.NoiseAnimatorType type)
    {
        switch (type)
        {
            case NoiseAnimator.NoiseAnimatorType.All:
                return new TextureModifier();
            case NoiseAnimator.NoiseAnimatorType.Terrain:
                return new TerrainTextureModifier();
            case NoiseAnimator.NoiseAnimatorType.Ocean:
                return new OceanTextureModifier();
            default:
                break;
        }
        return null;
    }
}
