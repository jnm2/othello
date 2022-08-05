using System.ComponentModel;

namespace Othello;

public sealed class OthelloBoard : GraphicsControl
{
    public OthelloBoard()
    {
        base.BackColor = Color.FromArgb(16, 64, 32);
    }

    [DefaultValue(typeof(Color), "16, 64, 32")]
    public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

    [DefaultValue(typeof(Color), "24, 96, 48"), Category("Appearance")]
    public Color BoardColor { get; set; } = Color.FromArgb(24, 96, 48);

    private OthelloGame othello;
    public OthelloGame Othello
    {
        get => othello;
        set
        {
            othello = value;
            ViewSize = new SizeF(othello.State.Board.GetLength(0) + 1, othello.State.Board.GetLength(1) + 1);
        }
    }

    private Point mouseSquare;

    private static float NearestDistanceWhere(int xLen, int yLen, PointF nearTo, Func<int, int, bool> pred)
    {
        var r = float.MaxValue;
        for (var x = 0; x < xLen; x++)
            for (var y = 0; y < yLen; y++)
                if (pred(x, y))
                {
                    var dist = Sqr(nearTo.X - x) + Sqr(nearTo.Y - y);
                    if (r > dist) r = dist;
                }
        return MathF.Sqrt(r);
    }

    public byte MinHint { get; set; } = 32;
    public byte MaxHint { get; set; } = 172;
    public bool ShowHint { get; set; }
    public bool ShowConsequence { get; set; }

    protected override void ViewDraw(Graphics g)
    {
        var xcount = othello.State.Board.GetLength(0);
        var ycount = othello.State.Board.GetLength(1);

        g.TranslateTransform(0.5f, 0.5f);

        using (var boardBrush = new SolidBrush(BoardColor))
            g.FillRectangle(boardBrush, 0, 0, xcount, ycount);

        g.FillEllipse(Brushes.Black, 1.925f, 1.925f, .15f, .15f);
        g.FillEllipse(Brushes.Black, 1.925f, 5.925f, .15f, .15f);
        g.FillEllipse(Brushes.Black, 5.925f, 1.925f, .15f, .15f);
        g.FillEllipse(Brushes.Black, 5.925f, 5.925f, .15f, .15f);

        if (mouseSquare.X >= 0 && mouseSquare.X < othello.State.Board.GetLength(0) && mouseSquare.Y >= 0 && mouseSquare.Y < othello.State.Board.GetLength(1))
            using (var highlightBrush = new SolidBrush(Color.FromArgb(48, IsLeftMouseDown ? Color.Black : Color.White)))
            using (var plusPen = new Pen(
                Color.FromArgb(othello.State.IsLegalMove(mouseSquare.X, mouseSquare.Y, othello.State.CurrentPlayer)
                    ? 255
                    : (int)(224 / (1 + Sqr(
                        NearestDistanceWhere(
                            othello.State.Board.GetLength(0),
                            othello.State.Board.GetLength(1),
                            new PointF(MouseView.X - 1f, MouseView.Y - 1f),
                            (x,y) => othello.State.IsLegalMove(x, y, othello.State.CurrentPlayer)
                        )
                    ))),
                    othello.CurrentPlayer == Player.Black ? Color.Black : Color.White
                ),
                .05f
            ))
            {
                //g.FillRectangle(highlightBrush, mouseSquare.X, mouseSquare.Y, 1.0f, 1.0f);
                g.DrawLines(plusPen, new PointF[] {
                    new PointF(mouseSquare.X + .3f, mouseSquare.Y + .5f),
                    new PointF(mouseSquare.X + .7f, mouseSquare.Y + .5f),
                    new PointF(mouseSquare.X + .5f, mouseSquare.Y + .5f),
                    new PointF(mouseSquare.X + .5f, mouseSquare.Y + .3f),
                    new PointF(mouseSquare.X + .5f, mouseSquare.Y + .7f)
                });
            }

        using (var pen = new Pen(Color.Black, 0.05f) { EndCap = System.Drawing.Drawing2D.LineCap.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round })
        {
            for (var x = 0; x < xcount + 1; x++)
                g.DrawLine(pen, x, 0, x, ycount);
            for (var y = 0; y < ycount + 1; y++)
                g.DrawLine(pen, 0, y, xcount, y);
        }

        using (var edgePen = new Pen(Color.Black, .05f))
            for (var x = 0; x < xcount; x++)
                for (var y = 0; y < ycount; y++)
                    if (othello.State.Board[x, y] is not null)
                    {
                        g.DrawEllipse(edgePen, x + .1f, y + .1f, .8f, .8f);
                        g.FillEllipse(othello.State.Board[x, y].Value == Player.Black ? Brushes.Black : Brushes.White, x + .1f, y + .1f, .8f, .8f);
                    }
                    else if (ShowHint && othello.State.IsLegalMove(x, y, othello.CurrentPlayer))
                    {
                        var fade = MinHint + (int)((MaxHint - MinHint) / Sqr(1.0f + Sqr(MouseView.X - x - 1) + Sqr(MouseView.Y - y - 1)));
                        if (fade < 0) fade = 0; else if (fade > 255) fade = 255;

                        using (var edgePenFade = new Pen(Color.FromArgb(fade, Color.Black), .05f))
                        using (var fillFade = new SolidBrush(Color.FromArgb(fade, othello.CurrentPlayer == Player.Black ? Color.Black : Color.White)))
                        {
                            g.DrawEllipse(edgePenFade, x + .1f, y + .1f, .8f, .8f);
                            g.FillEllipse(fillFade, x + .1f, y + .1f, .8f, .8f);
                        }
                    }

        if (ShowConsequence && mouseSquare.X >= 0 && mouseSquare.X < othello.State.Board.GetLength(0) && mouseSquare.Y >= 0 && mouseSquare.Y < othello.State.Board.GetLength(1))
        {
            var move = othello.State.Move(mouseSquare.X, mouseSquare.Y, othello.State.CurrentPlayer);
            if (move != null)
                for (var x = 0; x < othello.State.Board.GetLength(0); x++)
                    for (var y = 0; y < othello.State.Board.GetLength(1); y++)
                        if (!(x == mouseSquare.X && y == mouseSquare.Y) && move.Board[x, y] != othello.State.Board[x, y])
                            g.FillEllipse(Brushes.Red, x + .45f, y + .45f, .1f, .1f);
        }

        var blackTotal = 0;
        var whiteTotal = 0;

        for (var y = 0; y < othello.State.Board.GetLength(1); y++)
            for (var x = 0; x < othello.State.Board.GetLength(0); x++)
            {
                if (othello.State.Board[x, y] == Player.Black) blackTotal++;
                else if (othello.State.Board[x, y] == Player.White) whiteTotal++;
            }

        var blackCount = 0;
        var whiteCount = 0;

        if (othello.State.IsGameOver())
            for (var y = 0; y < othello.State.Board.GetLength(1); y++)
                for (var x = 0; x < othello.State.Board.GetLength(0); x++)
                {
                    if (othello.State.Board[x, y] == Player.Black) blackCount++;
                    else if (othello.State.Board[x, y] == Player.White) whiteCount++;
                    if (othello.State.Board[x, y] is not null)
                        using (var brush = new SolidBrush(othello.State.Board[x, y] == Player.Black
                            ? Color.FromArgb(32 + 223 / (1 + blackTotal - blackCount), Color.White)
                            : Color.FromArgb(32 + 223 / (1 + whiteTotal - whiteCount), Color.Black))
                        )
                            g.DrawString(
                                (othello.State.Board[x, y] == Player.Black ? blackCount : whiteCount).ToString(),
                                new Font(SystemFonts.MessageBoxFont.FontFamily, .3f),
                                brush,
                                new RectangleF(x, y, 1f, 1f),
                                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                            );
                }
    }

    private static float Sqr(float x)
    {
        return x * x;
    }

    protected override void ViewMouseMove(float x, float y, MouseButtons buttons)
    {
        mouseSquare = new Point((int)Math.Floor(x - .5), (int)Math.Floor(y - .5));
        Invalidate();
    }

    protected override void ViewMouseDown(float x, float y, MouseButtons buttons)
    {
        Invalidate();
    }

    protected override void ViewMouseUp(float x, float y, MouseButtons buttons)
    {
        if (mouseSquare.X >= 0 && mouseSquare.X < othello.State.Board.GetLength(0) && mouseSquare.Y >= 0 && mouseSquare.Y < othello.State.Board.GetLength(1))
            othello.Move(mouseSquare.X, mouseSquare.Y);
        Invalidate();
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.Z:
                othello.Undo();
                Invalidate();
                return true;

            case Keys.Control | Keys.Y:
                othello.Redo();
                Invalidate();
                return true;

            case Keys.Control | Keys.H:
                ShowHint = !ShowHint;
                Invalidate();
                return true;

            case Keys.Control | Keys.R:
                ShowConsequence = !ShowConsequence;
                Invalidate();
                return true;
        }

        return false;
    }
}
