namespace Othello;

public sealed partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        othelloBoard1.Othello = new OthelloGame();
    }
}
