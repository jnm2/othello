namespace Othello;

public abstract class GraphicsControl : Control
{
    protected SizeF ViewSize { get; set; }
    float viewScale;
    protected float ViewScale { get { return viewScale; } }
    private bool isLeftMouseDown;
    protected bool IsLeftMouseDown { get { return isLeftMouseDown; } }
    private PointF mouseView;
    protected PointF MouseView { get { return mouseView; } }

    public GraphicsControl()
    {
        this.ResizeRedraw = true;
        this.DoubleBuffered = true;
        this.ViewSize = new SizeF(1.0f, 1.0f);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        viewScale = ScaleGraphics(e.Graphics, ViewSize.Width, ViewSize.Height);
        ViewDraw(e.Graphics);
    }
    protected virtual void ViewDraw(Graphics g)
    {
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) isLeftMouseDown = true;
        var viewLocation = ClientToView(e.Location);
        ViewMouseDown(viewLocation.X, viewLocation.Y, e.Button);
	        base.OnMouseDown(e);
    }
    protected override void OnMouseMove(MouseEventArgs e)
    {
        mouseView = ClientToView(e.Location);
        ViewMouseMove(mouseView.X, mouseView.Y, e.Button);
        base.OnMouseMove(e);
    }
    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) isLeftMouseDown = false;
        var viewLocation = ClientToView(e.Location);
        ViewMouseUp(viewLocation.X, viewLocation.Y, e.Button);
        base.OnMouseUp(e);
    }

    protected virtual void ViewMouseDown(float x, float y, MouseButtons buttons)
    {
    }

    protected virtual void ViewMouseMove(float x, float y, MouseButtons buttons)
    {
    }

    protected virtual void ViewMouseUp(float x, float y, MouseButtons buttons)
    {
    }

    protected PointF ClientToView(Point client)
    {
        var physicalWidth = this.ClientSize.Width;
        var physicalHeight = this.ClientSize.Height;

        if (ViewSize.Width * physicalHeight > physicalWidth * ViewSize.Height)
        {
            viewScale = physicalWidth / ViewSize.Width;
            return new PointF(client.X / viewScale, (client.Y - physicalHeight / 2f) / viewScale + ViewSize.Height / 2f);
        }
        else
        {
            viewScale = physicalHeight / ViewSize.Height;
            return new PointF((client.X - physicalWidth / 2f) / viewScale + ViewSize.Width / 2f, client.Y / viewScale);
        }
    }

    protected PointF ViewToClient(PointF view)
    {
        var physicalWidth = this.ClientSize.Width;
        var physicalHeight = this.ClientSize.Height;

        if (ViewSize.Width * physicalHeight > physicalWidth * ViewSize.Height)
        {
            viewScale = physicalWidth / ViewSize.Width;
            return new PointF(view.X * viewScale, (view.Y - ViewSize.Height / 2f) * viewScale + physicalHeight / 2f);
        }
        else
        {
            viewScale = physicalHeight / ViewSize.Height;
            return new PointF((view.Y - ViewSize.Width / 2f) * viewScale + physicalWidth / 2f, view.Y * viewScale);
        }
    }

    private float ScaleGraphics(Graphics graphics, float width, float height)
    {
        float scale;

        var physicalWidth = this.ClientSize.Width;
        var physicalHeight = this.ClientSize.Height;


        if (width * physicalHeight > physicalWidth * height)
        {
            graphics.TranslateTransform(0, (physicalHeight - height * physicalWidth / width) / 2);
            scale = physicalWidth / width;
        }
        else
        {
            graphics.TranslateTransform((physicalWidth - width * physicalHeight / height) / 2, 0);
            scale = physicalHeight / height;
        }

        graphics.ScaleTransform(scale, scale);
        return scale;
    }
}
