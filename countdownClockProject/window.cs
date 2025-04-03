using System.Timers;
using Timer = System.Timers.Timer;
using System.Drawing.Drawing2D;

/*
Title:
    Countdown Clock
Description:
    This is a simple application which creates a GUI count down clock.
    It takes in an amount of time in seconds, which it will tick down
    until it reaches zero, at which point it will play 3 beeps to let
    the user know the time has passed.
*/

class window : Form
{
    Timer timer, drawTimer, beepTimer;
    int amount = 0;
    int time = 0;
    Button inputButton, stopButton;
    TextBox input;
    Label label;
    Graphics graphics;
    PointF[] points;
    RectangleF clockPointer;
    float myAngle = 180;

    public window()
    {
        //Initialize the window
        this.Text = "countdown";
        this.BackColor = Color.LightBlue;
        this.ClientSize = new Size(500, 500);

        //Initialize the GUI
        inputButton = new Button();
        stopButton = new Button();
        input = new TextBox();
        label = new Label();

        //Initialize the locations of GUI components
        inputButton.Location = new Point(145, 9);
        stopButton.Location = new Point(200, 55);
        input.Location = new Point(275, 10);
        label.Location = new Point(200, 40);

        //Initialize the sizes of GUI components
        inputButton.Size = new Size(120, 25);
        stopButton.Size = new Size(10, 10);
        input.Size = new Size(50, 20);

        //Initialize the two buttons with their functions
        inputButton.Text = "Number of seconds:";
        inputButton.Click += parseInput;

        stopButton.Text = "Stop timer";
        stopButton.Click += stopTheClock;


        //Adding the GUI to the screen
        this.Controls.Add(inputButton);
        this.Controls.Add(input);
        this.Controls.Add(label);

        this.Paint += drawClock;

        //We initalize the 3 points used to draw the triangle of the clock
        points = new PointF[3];
        points[0] = new Point(0, 300);
        points[1] = new Point(150, 150);
        points[2] = new Point(300, 300);

        clockPointer = new RectangleF(-5 / 2, 0, 5, 100);
    }

    void parseInput(object o, EventArgs e)
    {
        //Here, if the bool b is false then the parsing failed, but true if it didn't
        bool b = int.TryParse(input.Text, out int x);
        if (b)
        {
            //If it parsed succesfully, we start the clock
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

    //This function is called upon when the timer elapses/when the time is over
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
        graphics = pea.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        graphics.TranslateTransform(100, 100);

        graphics.FillPolygon(Brushes.Gray, points);
        graphics.FillEllipse(Brushes.Gray, 25, 25, 250, 250);
        graphics.FillEllipse(Brushes.White, 30, 30, 240, 240);

        graphics.TranslateTransform(150, 150);
        graphics.RotateTransform(myAngle);
        graphics.FillRectangle(Brushes.Black, clockPointer);
        graphics.ResetTransform();
    }

    void clockTicking(object o, ElapsedEventArgs e)
    {
        time++;
        if (time > 60)
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

    void stopTheClock(object o, EventArgs e)
    {
        whenElapsed(null, null);
    }

    //This function is called upon when the clock is done
    void playBeep()
    {
        Beep(null, null);
        beepTimer = new Timer(1000);
        beepTimer.Elapsed += Beep;
        beepTimer.AutoReset = true;
        beepTimer.Enabled = true;
    }

    //This function plays the beeps
    void Beep(object o, ElapsedEventArgs e)
    {
        if (amount == 3)
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

