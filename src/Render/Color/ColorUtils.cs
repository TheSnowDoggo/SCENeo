namespace SCENeo;

/// <summary>
/// A static class containing color utility functions.
/// </summary>
public static class ColorUtils
{
    /// <summary>
    /// Converts a <see cref="SCEColor"/> to a <see cref="ConsoleColor"/>.
    /// </summary>
    /// <remarks>
    /// Returns <see cref="ConsoleColor.Black"/> when <paramref name="color"/> is <see cref="SCEColor.Transparent"/>.
    /// </remarks>
    /// <param name="color">The color to convert.</param>
    /// <returns>The converted color.</returns>
    public static ConsoleColor ToConsoleColor(this SCEColor color)
    {
        return (ConsoleColor)((int)color & 0xF);
    }

    /// <summary>
    /// Merges the <paramref name="top"/> color onto the <paramref name="bottom"/> color.
    /// </summary>
    /// <param name="bottom">The bottom color.</param>
    /// <param name="top">The top color.</param>
    /// <returns>The merged color.</returns>
    public static SCEColor Merge(this SCEColor bottom, SCEColor top)
    {
        return top == SCEColor.Transparent ? bottom : top;
    }

    /// <summary>
    /// Merges the <paramref name="top"/> element onto the <paramref name="bottom"/> element.
    /// </summary>
    /// <param name="bottom">The bottom element.</param>
    /// <param name="top">The top element.</param>
    /// <returns>The merged element.</returns>
    public static char Merge(this char bottom, char top)
    {
        return top == Pixel.ElementTransparent ? bottom : top;
    }

    /// <summary>
    /// Determines whether the given color is light.
    /// </summary>
    /// <remarks>
    /// <see cref="SCEColor.White"/>, <see cref="SCEColor.Gray"/>, <see cref="SCEColor.Yellow"/> and <see cref="SCEColor.Cyan"/> are counted as light.
    /// </remarks>
    /// <param name="color">The color to check.</param>
    /// <returns><see langword="true"/> if the given <paramref name="color"/> is light; otherwise, <see langword="false"/>.</returns>
    public static bool IsLight(this SCEColor color)
    {
        return color is SCEColor.White or SCEColor.Gray or SCEColor.Yellow or SCEColor.Cyan;
    }

    /// <summary>
    /// Returns <see cref="SCEColor.White"/> or <see cref="SCEColor.Black"/> depending on if the given color is light.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The contrasting color.</returns>
    public static SCEColor Contrast(this SCEColor color)
    {
        return IsLight(color) ? SCEColor.Black : SCEColor.White;
    }

    /// <summary>
    /// Returns the next random color.
    /// </summary>
    /// <param name="random">The random.</param>
    /// <param name="includeTransparent">Whether or not transparent colors can be chosen.</param>
    /// <returns>The next random color.</returns>
    public static SCEColor NextColor(this Random random, bool includeTransparent = false)
    {
        return includeTransparent ? (SCEColor)random.Next(16) : (SCEColor)random.Next(17);
    }
}
