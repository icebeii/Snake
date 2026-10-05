//Snake
//Alena Koniukhova, 1. ročnik
//Letní semestr 2022-2023
//Programování 2 NPRG031


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gSnake
{
    public partial class Form1 : Form
    {
        enum State { START, GAME, END };
        State state;

        private int block = 40;                        //Základní proměnné a datové struktury, 
        private int width = 800;                       //které budeme potřebovat
        private int height = 800;
        private int score = 0;
        private int snakeSize = 5;
        private int snakeLength = 5;
        private int topOffset = 80;
        private int leftOffset = 560;
        private int fruitChance = 17;
        private int fruitNumber = 0;
        public PictureBox apple;
        private Label visibleScore = new Label();
        private Direction direction = new Direction(-1, 0);
        private int appleX; private int appleY;
        private PictureBox[] snake = new PictureBox[400];
        public PictureBox[] fruits = new PictureBox[1];                     //Odkaz na jablko, které je vždy jedno
        public Position[,] Positions = new Position[21, 21];
        public List<Position> allPos = new List<Position>();
        private List<Position> freePos = new List<Position>();

        public Form1()
        {
            //Inicializace počátečního stavu hry
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            scoreSign();
            createField();
            generatePos();
            for (int j = 0; j < snakeSize; j++)
            {
                generateSnake(j);
            }
            direction.Row = 1;
            ChangeState(State.START);   
        }

        void ChangeState(State new_state)
        {
            //Funkce, která zobrazí nebo skryje tlačítka a text
            //v závislosti na stavu
            switch (new_state)
            {
                case State.START:
                    bPlay.Visible = true;
                    break;

                case State.GAME:
                    bPlay.Visible = false;
                    KeyPreview = true;
                    break;

                case State.END:
                    bPlay.Visible = true;
                    break;

                default:
                    break;
            }

            state = new_state;
        }

        private void bPlay_Click(object sender, EventArgs e)
        {
            //Stisk tlačítka Play
            generateApple();
            startGame();
            ChangeState(State.GAME);
            this.KeyDown += new KeyEventHandler(click);
        }

        private void scoreSign()
        {
            //Vytváření textu score
            visibleScore = new Label();
            visibleScore.Text = "Score: 0";
            visibleScore.BackColor = Color.White;
            visibleScore.Location = new Point(350, 150);
            visibleScore.Visible = true;
            this.Controls.Add(visibleScore);
        }

        private void generatePos()
        {
            //generování pozic, odpovídajícich bunkam pole
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Position position = new Position(leftOffset + i * block, topOffset + j * block, true);
                    allPos.Add(position);
                    Positions[i, j] = position;
                }
            }
        }

        private Position FindPos(int x, int y)
        { 
            //Hledání pozic podle souřadnic
            int px = (x - leftOffset)/block;
            int py = (y - topOffset)/block;
            Position pos = Positions[px, py];
            return pos;
        }

        private void freePositions()
        {
            //Přidává všechny volné bunky do seznamu
            for (int i = 0; i < allPos.Count; i++)
            {
                if (allPos[i].Free == true)
                {
                    freePos.Add(allPos[i]);
                }
            }
        }

        private void createField()
        {
            //Vytváření bunek
            for (int i = topOffset/block; i <= (topOffset + width) / block; i++)
            {
                PictureBox line = new PictureBox();
                line.BackColor = Color.Gray;
                line.Size = new Size(width, 1);
                line.Location = new Point(leftOffset, block * i);
                this.Controls.Add(line);
            }

            for (int j = leftOffset/block; j <= (leftOffset + height) / block; j++)
            {
                PictureBox line = new PictureBox();
                line.BackColor = Color.Gray;
                line.Size = new Size(1, height);
                line.Location = new Point(block * j, topOffset);
                this.Controls.Add(line);
            }
        
        }

        private void startTimer()
        {
            timer.Tick += new EventHandler(action);
            timer.Interval = 180;
            timer.Start();
        }

        private void generateSnake(int i)
        {
            //Vytváření bloků hada
            snake[i] = new PictureBox();
            snake[i].Size = new Size(block, block);
            if (i == 0)
            {
                snake[i].BackColor = Color.DarkCyan;
                snake[i].Location = new Point(200+leftOffset, 200+topOffset);

            }
            else
            {
                snake[i].BackColor = Color.Aquamarine;
                int x = snake[i - 1].Location.X + block * direction.Row;
                int y = snake[i - 1].Location.Y + block * direction.Column;
                snake[i].Location = new Point(x, y);

            }
            this.Controls.Add(snake[i]);
        }

        private void move()
        {
            //Pohyb hada (1 interval časovače)
            for (int i = snakeLength - 1; i > 0 ; i--)
            {
                snake[i].Location = snake[i - 1].Location;
                if (i == snakeLength - 1)
                {
                    int x = snake[i].Location.X;
                    int y = snake[i].Location.Y;
                    Position pos = FindPos(x, y);
                    pos.Free = true;
                }
            }
            int nx = snake[0].Location.X + direction.Row * block;
            int ny = snake[0].Location.Y + direction.Column * block;
            snake[0].Location = new Point(nx, ny);
            Position npos = FindPos(nx, ny);
            npos.Free = false;
        }

        private void appleType()
        {
            //Generace čísla pro typ jablka
            Random num = new Random();
            fruitNumber = num.Next(1, 100);
        }

        private void generateApple()
        {
            //Vytváření jablka
            PictureBox apple = new PictureBox();
            appleType();
            freePositions();
            if (fruitNumber <= fruitChance)
            {
                apple.BackColor = Color.DarkGoldenrod;
            }
            else
            {
                apple.BackColor = Color.DarkRed;
            }
            apple.Size = new Size(40, 40);
            this.Controls.Add(apple);
            Random r = new Random();
            if (freePos.Count != 0)
            {
                int p = r.Next(0, freePos.Count);
                Position np = freePos[p];
                appleX = np.X;
                appleY = np.Y;
                apple.Location = new Point(appleX, appleY);
                this.Controls.Add(apple);
                fruits[0] = apple;
                freePos.Clear();
            }
            else
            {
                endGame();
            }
            
        }

        private void eatApple()
        {
            //Had jí jablko
            if (snake[0].Location.X == appleX && snake[0].Location.Y == appleY )
            {
                if (fruits[0].BackColor == Color.DarkRed)
                {
                    score += 1;
                }
                else
                {
                    score += 5;
                }
                visibleScore.Text = "Score: " + score;
                generateSnake(snakeLength);
                snakeLength += 1;
                fruits[0].Dispose();
                generateApple();
            }

        }

        private void checkWalls()
        {
            //Kontrola, zda je had uvnitř pole
            if (snake[0].Location.X < leftOffset + block && (direction.Row == -1 && direction.Column == 0))
            {
                endGame();
            }
            if (snake[0].Location.X > width + leftOffset - 2*block && (direction.Row == 1 && direction.Column == 0))
            {
                endGame();
            }
            if (snake[0].Location.Y < topOffset + block && (direction.Row == 0 && direction.Column == -1))
            {
                endGame();
            }
            if (snake[0].Location.Y > height + topOffset - 2*block && (direction.Row == 0 && direction.Column == 1))
            {
                endGame();
            }
        }

        private void eatTail()
        {
            //Kontrola, zda had nesežral ocas
            for (int i = 1; i < snakeLength - 1; i++)
            {
                if (snake[0].Location == snake[i].Location)
                {
                    endGame();
                    
                }
            }
        }

        private void startGame()
        {
            //Začatek hry
            if (state != State.START)
            {
                visibleScore.Text = "Score: " + score;
                removeSnake();
                direction.Row = -1;
                direction.Column = 0;
                for (int j = 0; j < snakeSize; j++)
                {
                    generateSnake(j);
                }
                snakeLength = 5;
                direction.Row = 1;
                timer.Start();
            }
            else
            {
                startTimer();
            }
        }

        private void endGame()
        {
            //Konec hry
            ChangeState(State.END);
            if (fruits[0] != null) 
            { 
                fruits[0].Dispose(); 
            }
            timer.Stop();
            visibleScore.Text = "Score: " + score;
            score = 0;
        }

        private void removeSnake()
        {
            //Odstranění hada po hře
            for (int i = 0; i < snakeLength; i++)
            {
                this.Controls.Remove(snake[i]);
                snake[i] = null;
            }
        }

        private void action(object myObject, EventArgs e)
        {
            //Funkce, která probíhají v jednom intervalu časovače 
            checkWalls();
            if (state != State.END)
            {
                eatTail();
            }          
            if (state != State.END)
            {
                eatApple();
                move();
            }
        }

        private void click(object sender, KeyEventArgs e)
        {
            //Stisk tlačítek se šipkami
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    direction = Direction.ChangeDirection(direction, "Right");
                    break;

                case "Left":
                    direction = Direction.ChangeDirection(direction, "Left");
                    break;

                case "Up":
                    direction = Direction.ChangeDirection(direction, "Up");
                    break;

                case "Down":
                    direction = Direction.ChangeDirection(direction, "Down");
                    break;
            }
        }

        public bool CheckBody(Direction d)
        {
            bool opp = false;
            if ((snake[1].Location.X == snake[0].Location.X + d.Row) && (snake[1].Location.Y == snake[0].Location.Y + d.Column))
            {
                opp = true;
            }
            return opp;
        }



    }


    //Class Direction, abychom vždy veděli, ve kterém směru se had pohybuje
    public class Direction
    {
        public int Row;
        public int Column;
        public Direction(int row, int column)
        {
            Row = row;
            Column = column;
        }

        private static bool checkOppositeDirection(Direction d, Direction nd)
        {
            bool dir = false;
            if (d.Row * -1 == nd.Row && d.Column * -1 == nd.Column)
            {
                dir = true;
            }
            return dir;
        }

        public static Direction ChangeDirection(Direction direction, string s)
        {
            //Pomocí předchozí funkce kontroluje, zda je nový směr opačný,
            //a pokud ano, zakazuje změnu směru
            int x = direction.Row; int y = direction.Column;
            Direction new_direction = new Direction(x, y);

            switch (s)
            {
                case "Right":
                    new_direction.Row = 1; new_direction.Column = 0;
                    if (checkOppositeDirection(direction, new_direction) == false)
                    {
                        direction.Row = 1; direction.Column = 0;
                    }
                    break;

                case "Left":
                    new_direction.Row = -1; new_direction.Column = 0;
                    if (checkOppositeDirection(direction, new_direction) == false)
                    {
                        direction.Row = -1; direction.Column = 0;
                    }
                    break;

                case "Up":
                    new_direction.Row = 0; new_direction.Column = -1;
                    if (checkOppositeDirection(direction, new_direction) == false)
                    {
                        direction.Row = 0; direction.Column = -1;
                    }
                    break;

                case "Down":
                    new_direction.Row = 0; new_direction.Column = 1;
                    if (checkOppositeDirection(direction, new_direction) == false)
                    {
                        direction.Row = 0; direction.Column = 1;
                    }
                    break;
            }
            return direction;
        }

    }

    //Class Position, abychom mohli vědět, které bunky jsou volné
    public class Position
    {
        public int X;
        public int Y;
        public bool Free;
        public Position(int x, int y, bool free)
        {
            this.X = x;
            this.Y = y;
            this.Free = free;
        }
    }
   
}