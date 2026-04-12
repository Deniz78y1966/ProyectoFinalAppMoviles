namespace ProyectoFinal.Drawables;

public class StatisticsDrawable : IDrawable
{
    public int BooksRead { get; set; }
    public int BooksPending { get; set; }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float width = dirtyRect.Width;
        float height = dirtyRect.Height;
        float barWidth = 60;
        float maxValue = Math.Max(Math.Max(BooksRead, BooksPending), 1);
        float scale = (height - 60) / maxValue;

        // Draw "Leídos" bar
        float readHeight = BooksRead * scale;
        canvas.FillColor = Color.FromArgb("#43A047");
        canvas.FillRectangle(width / 4 - barWidth / 2, height - 40 - readHeight, barWidth, readHeight);
        canvas.FontColor = Colors.Black;
        canvas.FontSize = 13;
        canvas.DrawString("Leídos", width / 4 - barWidth / 2, height - 35, barWidth, 30, HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.DrawString(BooksRead.ToString(), width / 4 - barWidth / 2, height - 45 - readHeight, barWidth, 20, HorizontalAlignment.Center, VerticalAlignment.Center);

        // Draw "Pendientes" bar
        float pendingHeight = BooksPending * scale;
        canvas.FillColor = Color.FromArgb("#FB8C00");
        canvas.FillRectangle(3 * width / 4 - barWidth / 2, height - 40 - pendingHeight, barWidth, pendingHeight);
        canvas.DrawString("Pendientes", 3 * width / 4 - barWidth / 2, height - 35, barWidth, 30, HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.DrawString(BooksPending.ToString(), 3 * width / 4 - barWidth / 2, height - 45 - pendingHeight, barWidth, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
    }
}