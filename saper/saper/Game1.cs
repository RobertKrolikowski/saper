using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace saper
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D textField;
        Random rand = new Random();
        MineField minefield;
        MouseState oldMouse;
        SpriteFont font;
        bool dead = false;
        bool win = false;
        float time = 0;


        class Field
        {
            int size;
            int mineValue;
            bool isVisable;
            bool flag;
            Rectangle rectPosition;
            public Field(int _size, int _mineValue, int posX = 0, int posY = 0)
            {
                size = _size;
                mineValue = _mineValue;
                isVisable = false;
                rectPosition = new Rectangle(posX, posY, size, size);
                flag = false;
            }

            public void Draw(SpriteBatch spriteBatch, Texture2D texture)
            {
                spriteBatch.Begin();
                if(isVisable == true)
                    spriteBatch.Draw(texture, rectPosition, new Rectangle((mineValue+1)*40, 0, 40, 40), Color.White);
                else if(isVisable == false && flag == false)
                    spriteBatch.Draw(texture, rectPosition, new Rectangle(0, 0, 40, 40), Color.White);
                else
                    spriteBatch.Draw(texture, rectPosition, new Rectangle(440, 0, 40, 40), Color.White);
                spriteBatch.End();
            }

            public Rectangle GetRectPosition()
            {
                return rectPosition;
            }

            public bool GetisVisable()
            {
                return isVisable;
            }

            public int GetMineValue()
            {
                return mineValue;
            }

            public bool GetFlag()
            {
                return flag;
            }

            public void SetisVisable(bool _isVisable)
            {
                isVisable = _isVisable;
            }

            public void SetMineValue(int _mineValue)
            {
                mineValue = _mineValue;
            }

            public void SetFlag(bool _flag)
            {
                flag = _flag;
            }


        }

        class MineField
        {
            int sizeX;
            int sizeY;
            int fieldSize;
            int moveTop;
            int moveRight;
            int mines;
            Field[,] fields;

            public MineField(int _sizeX, int _sizeY, int _mines,int _fieldSize)
            {
                fieldSize = _fieldSize;
                sizeX = _sizeX;
                sizeY = _sizeY;
                fields = new Field[sizeX, sizeY];
                moveTop = 0;
                moveRight = 0;
                mines = _mines;
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        fields[j, i] = new Field(fieldSize, 0, j * fieldSize + moveRight, i * fieldSize + moveTop);
                    }
                }                
            }

            public MineField(int _sizeX, int _sizeY, int _mines,int _fieldSize, int _moveTop, int _moveRight)
            {
                fieldSize = _fieldSize;
                sizeX = _sizeX;
                sizeY = _sizeY;
                fields = new Field[sizeX, sizeY];
                moveTop = _moveTop;
                moveRight = _moveRight;
                mines = _mines;
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        fields[j, i] = new Field(fieldSize, 0, j * fieldSize + moveRight, i * fieldSize + moveTop);
                    }
                }
            }

            public void Draw(SpriteBatch spriteBatch, Texture2D texture)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        fields[j, i].Draw(spriteBatch, texture);
                    }
                }
            }

            public void GenerateMineField(Random rand)
            {
                int mines1 = mines;
                if (mines1 >= sizeX * sizeY)
                    mines1 = (sizeX * sizeY) - 1;
                do
                {
                    int x, y;
                    x = rand.Next(sizeX);
                    y = rand.Next(sizeY);

                    if (fields[x, y].GetMineValue() == 0)
                    {                 
                        fields[x, y].SetMineValue(9);
                        mines1--;
                    }

                } while (mines1 > 0);

                
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        int minesbuff = 0;
                        if (fields[j, i].GetMineValue() != 9)
                        {
                            //if ((j % sizeX != 0) && (i % sizeY != 0))
                            
                            if ((j-1) >= 0 && (i-1) >= 0)
                            {
                                if (fields[j-1,i-1].GetMineValue() == 9)
                                    minesbuff++;
                            }                           
                            if (i-1 >= 0)
                            {
                                if (fields[j, i - 1].GetMineValue() == 9)
                                    minesbuff++;
                            }
                            
                            if((i-1 >= 0) && (j+1 < sizeX))
                            {
                                if (fields[j+1, i-1].GetMineValue() == 9)
                                    minesbuff++;
                            }

                            if (j - 1 >= 0)
                            {
                                if (fields[j - 1,i].GetMineValue() == 9)
                                    minesbuff++;
                            }

                            if (j + 1 < sizeX)
                            {
                                if (fields[j + 1, i].GetMineValue() == 9)
                                    minesbuff++;
                            }

                            if ((j - 1 >= 0) && (i + 1 < sizeY))
                            {
                                if (fields[j - 1, i + 1].GetMineValue() == 9)
                                    minesbuff++;
                            }

                            if (i + 1 < sizeY)
                            {
                                if (fields[j, i + 1].GetMineValue() == 9)
                                    minesbuff++;
                            }

                            if ((j + 1 < sizeX) && (i + 1 < sizeY))
                            {
                                if (fields[j + 1, i + 1].GetMineValue() == 9)
                                    minesbuff++;
                            }

                        }
                        if (fields[j, i].GetMineValue() != 9)
                            fields[j, i].SetMineValue(minesbuff);                       
                    }
                }

            }

            public bool ShowField(Rectangle rectMouse)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        //fields[j, i].GetRectPosition();
                        if (rectMouse.Intersects(fields[j, i].GetRectPosition()) && fields[j, i].GetisVisable() == false
                            && fields[j,i].GetFlag() == false)
                        {
                            if (fields[j, i].GetMineValue() == 9)
                            {
                                ShowAllMines();
                                return true;
                            }                       
                            fields[j, i].SetisVisable(true);
                            ShowBlankField();
                        }
                    }
                }
                return false;
            }

            private void ShowBlankField()
            {
                bool end = false;
                do
                {
                    end = false;
                    for (int i = 0; i < sizeY; i++)
                    {
                        for (int j = 0; j < sizeX; j++)
                        { 
                            if (fields[j, i].GetisVisable() == true && fields[j, i].GetMineValue() == 0)
                            {
                                if ((j - 1) >= 0 && (i - 1) >= 0 && fields[j - 1, i - 1].GetisVisable() == false)
                                {
                                    fields[j - 1, i - 1].SetisVisable(true);
                                    fields[j - 1, i - 1].SetFlag(false);
                                    end = true;
                                }
                                if (i - 1 >= 0 && fields[j, i - 1].GetisVisable() == false)
                                {
                                    fields[j, i - 1].SetisVisable(true);
                                    fields[j, i - 1].SetFlag(false);
                                    end = true;
                                }

                                if ((i - 1 >= 0) && (j + 1 < sizeX) && fields[j + 1, i - 1].GetisVisable() == false)
                                {
                                    fields[j + 1, i - 1].SetisVisable(true);
                                    fields[j + 1, i - 1].SetFlag(false);
                                    end = true;
                                }

                                if (j - 1 >= 0 && fields[j - 1, i].GetisVisable() == false)
                                {
                                    fields[j - 1, i].SetisVisable(true);
                                    fields[j - 1, i].SetFlag(false);
                                    end = true;
                                }

                                if (j + 1 < sizeX && fields[j + 1, i].GetisVisable() == false)
                                {
                                    fields[j + 1, i].SetisVisable(true);
                                    fields[j + 1, i].SetFlag(false);
                                    end = true;
                                }

                                if ((j - 1 >= 0) && (i + 1 < sizeY) && fields[j - 1, i + 1].GetisVisable() == false)
                                {
                                    fields[j - 1, i + 1].SetisVisable(true);
                                    fields[j - 1, i + 1].SetFlag(false);
                                    end = true;
                                }

                                if (i + 1 < sizeY && fields[j, i + 1].GetisVisable() == false)
                                {
                                    fields[j, i + 1].SetisVisable(true);
                                    fields[j, i + 1].SetFlag(false);
                                    end = true;
                                }

                                if ((j + 1 < sizeX) && (i + 1 < sizeY) && fields[j + 1, i + 1].GetisVisable() == false)
                                {
                                    fields[j + 1, i + 1].SetisVisable(true);
                                    fields[j + 1, i + 1].SetFlag(false);
                                    end = true;
                                }

                            }
                        }
                    }
                } while (end == true);
            }

            public void ShowAllMines()
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if(fields[j,i].GetMineValue() == 9)
                            fields[j, i].SetisVisable(true);
                    }
                }
            }

            public void SaveMine(Rectangle rectMouse)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        //fields[j, i].GetRectPosition();
                        if (rectMouse.Intersects(fields[j, i].GetRectPosition()) && fields[j, i].GetisVisable() == false)
                        {
                            fields[j, i].SetFlag(!fields[j, i].GetFlag());
                        }
                    }
                }
            }

            public void ShowFields(Rectangle rectMouse)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if (rectMouse.Intersects(fields[j, i].GetRectPosition()) && fields[j, i].GetisVisable() == true)
                        {
                            int mines = fields[j, i].GetMineValue();
                            int flags = 0;
                            if (mines != 0)
                            {
                                //count number of flags
                                if ((j - 1) >= 0 && (i - 1) >= 0 && fields[j - 1, i - 1].GetFlag() == true)
                                {
                                    flags++;                                   
                                }
                                if (i - 1 >= 0 && fields[j, i - 1].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if ((i - 1 >= 0) && (j + 1 < sizeX) && fields[j + 1, i - 1].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if (j - 1 >= 0 && fields[j - 1, i].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if (j + 1 < sizeX && fields[j + 1, i].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if ((j - 1 >= 0) && (i + 1 < sizeY) && fields[j - 1, i + 1].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if (i + 1 < sizeY && fields[j, i + 1].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if ((j + 1 < sizeX) && (i + 1 < sizeY) && fields[j + 1, i + 1].GetFlag() == true)
                                {
                                    flags++;
                                }

                                if (mines == flags)
                                {
                                    if ((j - 1) >= 0 && (i - 1) >= 0 && fields[j - 1, i - 1].GetFlag() == false)
                                    {
                                        fields[j - 1, i - 1].SetisVisable(true);
                                    }
                                    if (i - 1 >= 0 && fields[j, i - 1].GetFlag() == false)
                                    {
                                        fields[j, i - 1].SetisVisable(true);
                                    }

                                    if ((i - 1 >= 0) && (j + 1 < sizeX) && fields[j + 1, i - 1].GetFlag() == false)
                                    {
                                        fields[j + 1, i - 1].SetisVisable(true);
                                    }

                                    if (j - 1 >= 0 && fields[j - 1, i].GetFlag() == false)
                                    {
                                        fields[j - 1, i].SetisVisable(true);
                                    }

                                    if (j + 1 < sizeX && fields[j + 1, i].GetFlag() == false)
                                    {
                                        fields[j + 1, i].SetisVisable(true);
                                    }

                                    if ((j - 1 >= 0) && (i + 1 < sizeY) && fields[j - 1, i + 1].GetFlag() == false)
                                    {
                                        fields[j - 1, i + 1].SetisVisable(true);
                                    }

                                    if (i + 1 < sizeY && fields[j, i + 1].GetFlag() == false)
                                    {
                                        fields[j, i + 1].SetisVisable(true);
                                    }

                                    if ((j + 1 < sizeX) && (i + 1 < sizeY) && fields[j + 1, i + 1].GetFlag() == false)
                                    {
                                        fields[j + 1, i + 1].SetisVisable(true);
                                    }
                                }
                                
                            }
                        }
                    }
                }
                ShowBlankField();
            }

            public bool Dead()
            {
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if (fields[j, i].GetisVisable() == true && fields[j, i].GetMineValue() == 9)
                            return true;
                    }
                }
                return false;
            }

            public bool YouWin()
            {
                int noVisableFields = 0;
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if (fields[j, i].GetisVisable() == false)
                            noVisableFields++;
                    }
                }
                if (noVisableFields == mines)
                    return true;
                return false;
            }

            public int GetSizeX()
            {
                return sizeX;
            }

            public int GetSizeY()
            {
                return sizeY;
            }

            public int GetNumberOfFlags()
            {
                int flags = 0;
                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        if (fields[j, i].GetFlag() == true)
                            flags++;
                    }
                }
                return flags;
            }

            public int GetMines()
            {
                return mines;
            }


        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 200;
            graphics.PreferredBackBufferHeight = 230;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textField = Content.Load<Texture2D>("saper");
            font = Content.Load<SpriteFont>("font");
            minefield = new MineField(10, 10, 10, 20, 30, 0);
            minefield.GenerateMineField(rand);


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            MouseState mouse = Mouse.GetState();

            if (dead == false)
            {
                if(win == false)
                    time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    Rectangle rectMouse = new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1);
                    minefield.ShowField(rectMouse);
                    dead = minefield.Dead();
                }
                if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released)
                {
                    Rectangle rectMouse = new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1);
                    minefield.SaveMine(rectMouse);
                    minefield.GetNumberOfFlags();
                }
                oldMouse = mouse;
                if (mouse.LeftButton == ButtonState.Pressed && mouse.RightButton == ButtonState.Pressed)
                {
                    Rectangle rectMouse = new Rectangle(mouse.Position.X, mouse.Position.Y, 1, 1);
                    minefield.ShowFields(rectMouse);
                    dead = minefield.Dead();
                }
                win = minefield.YouWin();
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            minefield.Draw(spriteBatch, textField);
            if (dead == true)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "YOU DIE", new Vector2(graphics.PreferredBackBufferWidth - 70,0), Color.Black);
                spriteBatch.End();
            }
            else if (win == true)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "YOU WIN", new Vector2(graphics.PreferredBackBufferWidth - 70, 0), Color.Black);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(font, ""+(int)time, Vector2.Zero, Color.Black);
            int m = minefield.GetMines() - minefield.GetNumberOfFlags();
            spriteBatch.DrawString(font, ""+ m, new Vector2(graphics.PreferredBackBufferWidth / 2,0), Color.Black);
            spriteBatch.Draw(textField,new Rectangle(graphics.PreferredBackBufferWidth / 2 - 20, 0, 20, 20) , new Rectangle(440, 0, 40, 40), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
