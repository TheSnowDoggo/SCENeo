namespace SCENeo;

public class Image : Grid2D<Pixel>
{
    public Image(Pixel[,] data) 
        : base(data)
    {
    }

    public Image(int width, int height)
        : base(width, height)
    {
    }

    public void MapString(string str, ColorInfo colorInfo, int startX, int startY)
    {
        int x = startX;
        int y = startY;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] is '\n' or '\v' or '\f')
            {
                x = startX;
                y++;
                continue;
            }

            this[x, y] = new Pixel(str[i], colorInfo);
            x++;
        }
    }
}
