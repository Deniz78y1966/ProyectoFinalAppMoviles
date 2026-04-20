namespace ProyectoFinal.Drawables;

public class StatisticsDrawable : IDrawable
{
    // Your original properties
    public int BooksRead { get; set; }
    public int BooksPending { get; set; }

    // New properties from teacher's model
    public int TotalBooks { get; set; }
    public int UnreadBooks { get; set; }
    public Dictionary<string, int> BooksByGenre { get; set; } = new();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(dirtyRect);

        // 1. Pie chart: Leídos vs Pendientes
        DrawPieChart(canvas, dirtyRect);

        // 2. Bar chart: Libros por género (your original logic)
        DrawBarChart(canvas, dirtyRect);

        // 3. Big stat numbers
        DrawStatNumbers(canvas, dirtyRect);
    }

    private void DrawPieChart(ICanvas canvas, RectF dirtyRect)
    {
        float centerX = dirtyRect.Width / 4;
        float centerY = 120;
        float radius = 80;

        float total = Math.Max(TotalBooks, 1);
        float readAngle = (BooksRead / total) * 360f;
        float unreadAngle = 360f - readAngle;

        // Draw "Leídos" slice
        canvas.FillColor = Color.FromArgb("#43A047");
        canvas.FillArc(centerX - radius, centerY - radius,
                       radius * 2, radius * 2,
                       0, readAngle, true);

        // Draw "Pendientes" slice
        canvas.FillColor = Color.FromArgb("#FB8C00");
        canvas.FillArc(centerX - radius, centerY - radius,
                       radius * 2, radius * 2,
                       readAngle, unreadAngle, true);

        // Draw donut hole
        canvas.FillColor = Colors.White;
        canvas.FillCircle(centerX, centerY, radius * 0.5f);

        // Center text
        canvas.FontColor = Color.FromArgb("#8B4513");
        canvas.FontSize = 14;
        float pct = TotalBooks > 0 ? (float)BooksRead / TotalBooks * 100 : 0;
        canvas.DrawString($"{Math.Round(pct)}%",
                          centerX - 20, centerY - 10, 40, 20,
                          HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.FontSize = 10;
        canvas.DrawString("leído",
                          centerX - 20, centerY + 5, 40, 20,
                          HorizontalAlignment.Center, VerticalAlignment.Center);

        // Legend
        canvas.FillColor = Color.FromArgb("#43A047");
        canvas.FillRectangle(centerX - radius, centerY + radius + 10, 12, 12);
        canvas.FontColor = Colors.Black;
        canvas.FontSize = 11;
        canvas.DrawString($"Leídos ({BooksRead})",
                          centerX - radius + 16, centerY + radius + 10, 80, 15,
                          HorizontalAlignment.Left, VerticalAlignment.Center);

        canvas.FillColor = Color.FromArgb("#FB8C00");
        canvas.FillRectangle(centerX - radius, centerY + radius + 28, 12, 12);
        canvas.DrawString($"Pendientes ({BooksPending})",
                          centerX - radius + 16, centerY + radius + 28, 90, 15,
                          HorizontalAlignment.Left, VerticalAlignment.Center);
    }

    private void DrawBarChart(ICanvas canvas, RectF dirtyRect)
    {
        float width = dirtyRect.Width;
        float height = dirtyRect.Height;

        // If we have genre data, use it
        if (BooksByGenre != null && BooksByGenre.Count > 0)
        {
            float startX = width / 2 + 10;
            float startY = 30;
            float chartHeight = 180;
            float chartWidth = width / 2 - 20;

            canvas.FontColor = Color.FromArgb("#8B4513");
            canvas.FontSize = 12;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            canvas.DrawString("Por género", startX, startY, chartWidth, 20,
                              HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;

            int maxValue = Math.Max(BooksByGenre.Values.Max(), 1);
            float barWidth = chartWidth / (BooksByGenre.Count * 2f);
            float x = startX + 10;

            var colors = new[]
            {
                Color.FromArgb("#8B4513"),
                Color.FromArgb("#D2691E"),
                Color.FromArgb("#CD853F"),
                Color.FromArgb("#DEB887"),
                Color.FromArgb("#A0522D")
            };

            int colorIndex = 0;
            foreach (var genre in BooksByGenre)
            {
                float barHeight = (genre.Value / (float)maxValue) * chartHeight;
                float barY = startY + 25 + chartHeight - barHeight;

                canvas.FillColor = colors[colorIndex % colors.Length];
                canvas.FillRectangle(x, barY, barWidth, barHeight);

                canvas.FontColor = Colors.Black;
                canvas.FontSize = 10;
                canvas.DrawString(genre.Value.ToString(), x, barY - 15, barWidth, 15,
                                  HorizontalAlignment.Center, VerticalAlignment.Center);

                string shortGenre = genre.Key.Length > 6
                    ? genre.Key.Substring(0, 6) + "."
                    : genre.Key;
                canvas.DrawString(shortGenre, x - 5, startY + 25 + chartHeight + 5,
                                  barWidth + 10, 20,
                                  HorizontalAlignment.Center, VerticalAlignment.Center);

                x += barWidth * 2;
                colorIndex++;
            }
        }
        else
        {
            // Your original bar chart as fallback
            float barWidth = 60;
            float maxValue = Math.Max(Math.Max(BooksRead, BooksPending), 1);
            float scale = (height - 60) / maxValue;

            float readHeight = BooksRead * scale;
            canvas.FillColor = Color.FromArgb("#43A047");
            canvas.FillRectangle(width / 4 - barWidth / 2, height - 40 - readHeight, barWidth, readHeight);
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 13;
            canvas.DrawString("Leídos", width / 4 - barWidth / 2, height - 35, barWidth, 30,
                              HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawString(BooksRead.ToString(), width / 4 - barWidth / 2, height - 45 - readHeight, barWidth, 20,
                              HorizontalAlignment.Center, VerticalAlignment.Center);

            float pendingHeight = BooksPending * scale;
            canvas.FillColor = Color.FromArgb("#FB8C00");
            canvas.FillRectangle(3 * width / 4 - barWidth / 2, height - 40 - pendingHeight, barWidth, pendingHeight);
            canvas.DrawString("Pendientes", 3 * width / 4 - barWidth / 2, height - 35, barWidth, 30,
                              HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawString(BooksPending.ToString(), 3 * width / 4 - barWidth / 2, height - 45 - pendingHeight, barWidth, 20,
                              HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }

    private void DrawStatNumbers(ICanvas canvas, RectF dirtyRect)
    {
        float y = dirtyRect.Height - 60;
        float sectionWidth = dirtyRect.Width / 3;

        // Total books
        canvas.FontColor = Color.FromArgb("#8B4513");
        canvas.FontSize = 28;
        canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
        canvas.DrawString(TotalBooks.ToString(), 0, y, sectionWidth, 35,
                          HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.FontSize = 11;
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        canvas.FontColor = Colors.Gray;
        canvas.DrawString("Total", 0, y + 32, sectionWidth, 20,
                          HorizontalAlignment.Center, VerticalAlignment.Center);

        // Read books
        canvas.FontColor = Color.FromArgb("#43A047");
        canvas.FontSize = 28;
        canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
        canvas.DrawString(BooksRead.ToString(), sectionWidth, y, sectionWidth, 35,
                          HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.FontSize = 11;
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        canvas.FontColor = Colors.Gray;
        canvas.DrawString("Leídos", sectionWidth, y + 32, sectionWidth, 20,
                          HorizontalAlignment.Center, VerticalAlignment.Center);

        // Pending books
        canvas.FontColor = Color.FromArgb("#FB8C00");
        canvas.FontSize = 28;
        canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
        canvas.DrawString(BooksPending.ToString(), sectionWidth * 2, y, sectionWidth, 35,
                          HorizontalAlignment.Center, VerticalAlignment.Center);
        canvas.FontSize = 11;
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        canvas.FontColor = Colors.Gray;
        canvas.DrawString("Pendientes", sectionWidth * 2, y + 32, sectionWidth, 20,
                          HorizontalAlignment.Center, VerticalAlignment.Center);
    }
}