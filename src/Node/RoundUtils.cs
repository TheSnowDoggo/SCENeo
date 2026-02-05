namespace SCENeo.Node;

public static class RoundUtils
{
    public static Vec2I RoundToRenderCoords(this Vec2 position, PositionRounding rounding)
    {
        return rounding switch
        {
            PositionRounding.Wide => (Vec2I)position.Round() * new Vec2I(2, 1),
            PositionRounding.WideExact => (Vec2I)(position * new Vec2(2, 1)).Round(),
            PositionRounding.None => (Vec2I)position.Round(),
            _ => throw new ArgumentOutOfRangeException(nameof(rounding), rounding, "Invalid rounding mode.")
        };
    }

    public static Vec2 RoundAsRenderCoords(this Vec2 position, PositionRounding rounding)
    {
        return rounding switch
        {
            PositionRounding.Wide => position.Round(),
            PositionRounding.WideExact => (position * new Vec2(2, 1)).Round() / new Vec2(2, 1),
            PositionRounding.None => position,
            _ => throw new ArgumentOutOfRangeException(nameof(rounding), rounding, "Invalid rounding mode.")
        };
    }
}