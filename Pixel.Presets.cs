namespace SCENeo;

public readonly partial struct Pixel
{
    /// <summary>
    /// Gets a pixel that will be fully transparent when merged.
    /// </summary>
    public static Pixel Null => new Pixel(ElementTransparent, SCEColor.Transparent, SCEColor.Transparent);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Transparent"/>.
    /// </summary>
    public static Pixel Transparent => new Pixel(SCEColor.Transparent);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Black"/>.
    /// </summary>
    public static Pixel Black => new Pixel(SCEColor.Black);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkBlue"/>.
    /// </summary>
    public static Pixel DarkBlue => new Pixel(SCEColor.DarkBlue);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkGreen"/>.
    /// </summary>
    public static Pixel DarkGreen => new Pixel(SCEColor.DarkGreen);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkCyan"/>.
    /// </summary>
    public static Pixel DarkCyan => new Pixel(SCEColor.DarkCyan);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkRed"/>.
    /// </summary>
    public static Pixel DarkRed => new Pixel(SCEColor.DarkRed);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkMagenta"/>.
    /// </summary>
    public static Pixel DarkMagenta => new Pixel(SCEColor.DarkMagenta);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkYellow"/>.
    /// </summary>
    public static Pixel DarkYellow => new Pixel(SCEColor.DarkYellow);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Gray"/>.
    /// </summary>
    public static Pixel Gray => new Pixel(SCEColor.Gray);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.DarkGray"/>.
    /// </summary>
    public static Pixel DarkGray => new Pixel(SCEColor.DarkGray);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Blue"/>.
    /// </summary>
    public static Pixel Blue => new Pixel(SCEColor.Blue);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Green"/>.
    /// </summary>
    public static Pixel Green => new Pixel(SCEColor.Green);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Cyan"/>.
    /// </summary>
    public static Pixel Cyan => new Pixel(SCEColor.Cyan);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Red"/>.
    /// </summary>
    public static Pixel Red => new Pixel(SCEColor.Red);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Magenta"/>.
    /// </summary>
    public static Pixel Magenta => new Pixel(SCEColor.Magenta);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.Yellow"/>.
    /// </summary>
    public static Pixel Yellow => new Pixel(SCEColor.Yellow);

    /// <summary>
    /// Gets an opaque pixel who's background color is <see cref="SCEColor.White"/>.
    /// </summary>
    public static Pixel White => new Pixel(SCEColor.White);
}