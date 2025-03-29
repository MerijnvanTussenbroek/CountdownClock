using System.Timers;
using Timer = System.Timers.Timer;
using System.Drawing.Drawing2D;

class window : Form
{
    Timer timer, drawTimer, beepTimer;

    int amount = 0;

    public int time = 0;

    Button inputButton, stopButton;

    TextBox input;

    Label label;

    Graphics g;

    PointF[] points;

    RectangleF clockPointer;

    float myAngle = 180;

    public window()
    {
        this.Text = "Countdown";
        this.BackColor = Color.LightBlue;
        this.ClientSize = new Size(500, 500);

        inputButton = new Button();
        stopButton = new Button();

        input = new TextBox();

        label = new Label();

        inputButton.Location = new Point(145, 9);
        input.Location = new Point(275, 10);
        label.Location = new Point(200, 40);

        inputButton.Size = new Size(120, 25);
        input.Size = new Size(50, 20);

        inputButton.Text = "Aantal seconden:";

        inputButton.Click += parseInput;

        this.Controls.Add(inputButton);
        this.Controls.Add(input);

        this.Controls.Add(label);

        this.Paint += drawClock;

        points = new PointF[3];
        points[0] = new Point(0, 300);
        points[1] = new Point(150, 150);
        points[2] = new Point(300, 300);

        clockPointer = new RectangleF(-5/2, 0, 5, 100);
    }

    void parseInput(object o, EventArgs e)
    {
        timer = new Timer();
        bool b = int.TryParse(input.Text, out int x);
        if (b)
        {
            label.Text = "";
            timer = new Timer(x * 1000);
            timer.Elapsed += whenElapsed;
            drawTimer = new Timer(1000);
            drawTimer.Elapsed += clockTicking;
            drawTimer.AutoReset = true;
            timer.Enabled = true;
            drawTimer.Enabled = true;
        }
        else
        {
            label.Text = "Input invalid.";
        }
    }

    void whenElapsed(object o, ElapsedEventArgs e)
    {
        timer.Stop();
        timer.Elapsed -= whenElapsed;
        timer.Enabled = false;
        timer.Dispose();

        drawTimer.Stop();
        drawTimer.Elapsed -= clockTicking;
        drawTimer.Enabled = false;
        drawTimer.Dispose();

        resetClock();


        playBeep();
    }

    void drawClock(object o, PaintEventArgs pea)
    {
        g = pea.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        g.TranslateTransform(100, 100);

        g.FillPolygon(Brushes.Gray, points);
        g.FillEllipse(Brushes.Gray, 25, 25, 250, 250);
        g.FillEllipse(Brushes.White, 30, 30, 240, 240);
        
        g.TranslateTransform(150, 150);
        g.RotateTransform(myAngle);
        g.FillRectangle(Brushes.Black, clockPointer);
        g.ResetTransform();
    }

    void clockTicking(object o, ElapsedEventArgs e)
    {
        time++;
        if(time > 60)
        {
            time = 1;
        }
        myAngle = 180 + (time * 6);
        this.Invalidate();
    }

    void resetClock()
    {
        time = 0;
        myAngle = 180;
        this.Invalidate();
    }

    void playBeep()
    {
        Beep(null, null);
        beepTimer = new Timer(1000);
        beepTimer.Elapsed += Beep;
        beepTimer.AutoReset = true;
        beepTimer.Enabled = true;
    }

    void Beep(object o, ElapsedEventArgs e)
    {
        if(amount == 3)
        {
            beepTimer.Stop();
            beepTimer.Elapsed -= Beep;
            beepTimer.AutoReset = false;
            beepTimer.Enabled = false;
            beepTimer.Dispose();
            amount = 0;
        }
        else
        {
            System.Media.SystemSounds.Beep.Play();
            amount++;
        }
    }
}
