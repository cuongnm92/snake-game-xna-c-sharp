using System;
using System.Collections.Generic;

using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SnakeGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpaceModeManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int scoreAdd = 5;

        // SpriteBatch for drawing
        SpriteBatch spriteBatch;

        // A sprite for the player and a list of automated sprites
        Player player;
        List<Sprite> spriteList = new List<Sprite>();

        // Variables for spawning new enemies
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;

        // Chance of spawning different enemies
        int likelihoodAutomated = 75;
        int likelihoodChasing = 20;
        int likelihoodEvading = 5;

        // Scoring
        int automatedSpritePointValue = 10;
        int chasingSpritePointValue = 20;
        int evadingSpritePointValue = 0;

        List<Rat> rats = new List<Rat>();

        // Variables for spawning new rats
        const int ratMaxSpawnTime = 100;
        int ratTimeCount = 0;
        int ratMinSpeed = 2;
        int ratMaxSpeed = 6;

        // Lives
        List<AutomatedSprite> livesList = new List<AutomatedSprite>();

        // Lives remaining
        int numberLivesRemaining = 3;

        //Spawn time variables
        int nextSpawnTimeChange = 5000;
        int timeSinceLastSpawnTimeChange = 0;

        // Powerup
        int powerUpExpiration = 0;

        //Texture For Game

        Texture2D playerTexture;
        Texture2D threeringsTexture;
        Texture2D threebladesTexture;
        Texture2D fourbladesTexture;
        Texture2D boltTexture;
        Texture2D plusTexture;
        Texture2D skullballTexture;
        Texture2D ratTexture;

        //Player Score
        int playerScore;
        SpriteFont scoreFont;

        //Map
        SpaceMap map;

        public SpaceModeManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Initialize spawn time
            ResetSpawnTime();
            this.ratTimeCount = 0;
            playerScore = 0;

            map = new SpaceMap(Game.Content, 0, Game.Window.ClientBounds);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            playerTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\snakee");
            threeringsTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\threerings");
            threebladesTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\threeblades");
            fourbladesTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\fourblades");
            boltTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\bolt");
            plusTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\plus");
            skullballTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\skullball");
            ratTexture = Game.Content.Load<Texture2D>(@"Images\Sprite\rat");

            scoreFont = Game.Content.Load<SpriteFont>(@"Fonts\Score");

            player = new Player(playerTexture,
                new Vector2(Game.Window.ClientBounds.Width / 2,
                    Game.Window.ClientBounds.Height / 2),
                new Point(96, 96), 10, new Point(0, 0),
                new Point(4, 4), new Vector2(6, 6));


            // Load player lives list
            for (int i = 0; i < this.numberLivesRemaining; ++i)
            {
                int offset = 10 + i * 40;
                livesList.Add(new AutomatedSprite(
                    threeringsTexture,
                    new Vector2(offset, 35), new Point(75, 75), 10,
                    new Point(0, 0), new Point(6, 8), Vector2.Zero,
                    null, 0, .5f));
            }

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Time to spawn enemy?
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();

                // Reset spawn timer
                ResetSpawnTime();
            }

            ratTimeCount++;
            if (ratTimeCount > ratMaxSpawnTime)
            {
                ratTimeCount = 0;
                SpawnRat();
            }

            UpdateSprites(gameTime);

            // Adjust sprite spawn times
            AdjustSpawnTimes(gameTime);

            // Expire Powerups?
            CheckPowerUpExpiration(gameTime);

            map.Update(gameTime);

            base.Update(gameTime);
        }

        protected void UpdateSprites(GameTime gameTime)
        {
            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            for (int i = 0; i < rats.Count; i++)
            {
                rats[i].Update(gameTime, Game.Window.ClientBounds);
            }

            for (int i = 0; i < rats.Count; i++)
            {
                Rat r = rats[i];

                // Check for collisions
                if (r.collisionRect.Intersects(player.collisionRect))
                {
                    //Updata score value

                    this.AddScore(scoreAdd);

                    // Remove collided sprite from the game
                    rats.RemoveAt(i);
                    --i;
                }

                // Remove object if it is out of bounds
                if (r.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    rats.RemoveAt(i);
                    --i;
                }
            }

            // Update all non-player sprites
            for (int i = 0; i < spriteList.Count; ++i)
            {
                Sprite s = spriteList[i];

                s.Update(gameTime, Game.Window.ClientBounds);

                if (s is AutomatedSprite)
                {
                    for (int j = 0; j < rats.Count; j++)
                    {
                        Rat r = rats[j];

                        // Check for collisions
                        if (r.collisionRect.Intersects(s.collisionRect))
                        {
                            // Remove collided sprite from the game
                            rats.RemoveAt(j);
                            --j;
                        }
                    }
                }

                // Check for collisions
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    // Play collision sound
                    if (s.collisionCueName != null)
                        ((SnakeGame)Game).PlayCue(s.collisionCueName);

                    // If collided with AutomatedSprite
                    // remove a life from the player
                    if (s is AutomatedSprite)
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --this.numberLivesRemaining;
                        }
                    }
                    else if (s.collisionCueName == "pluscollision")
                    {
                        // Collided with plus - start plus power-up
                        powerUpExpiration = 5000;
                        player.ModifyScale(2);
                    }
                    else if (s.collisionCueName == "skullcollision")
                    {
                        // Collided with skull - start skull power-up
                        powerUpExpiration = 5000;
                        player.ModifySpeed(.5f);
                    }
                    else if (s.collisionCueName == "boltcollision")
                    {
                        // Collided with bolt - start bolt power-up
                        powerUpExpiration = 5000;
                        player.ModifySpeed(2);
                    }

                    // Remove collided sprite from the game
                    spriteList.RemoveAt(i);
                    --i;
                }


                // Remove object if it is out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    // this.AddScore(spriteList[i].scoreValue);
                    spriteList.RemoveAt(i);
                    --i;
                }
            }

            // Update lives-list sprites
            foreach (Sprite sprite in livesList)
                sprite.Update(gameTime, Game.Window.ClientBounds);
        }

        public override void Draw(GameTime gameTime)
        {
            // spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Begin();

            map.Draw(gameTime, spriteBatch);

            // Draw score
            spriteBatch.DrawString(scoreFont,
                "Score: " + playerScore,
                new Vector2(10, 10), Color.DarkBlue,
                0, Vector2.Zero,
                1, SpriteEffects.None, 1);

            // Draw the player
            player.Draw(gameTime, spriteBatch);

            // Draw all rats

            foreach (Rat r in rats)
                r.Draw(gameTime, spriteBatch);

            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            // Draw player lives
            foreach (Sprite sprite in livesList)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetSpawnTime()
        {
            // Set the next spawn time for an enemy
            nextSpawnTime = ((SnakeGame)Game).rnd.Next(
                enemySpawnMinMilliseconds,
                enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default frame size
            Point frameSize = new Point(75, 75);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((SnakeGame)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                        -frameSize.X, ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));
                    speed = new Vector2(((SnakeGame)Game).rnd.Next(
                        enemyMinSpeed,
                        enemyMaxSpeed), 0);
                    break;
                case 1: // RIGHT to LEFT
                    position = new
                        Vector2(
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(-((SnakeGame)Game).rnd.Next(
                        enemyMinSpeed, enemyMaxSpeed), 0);
                    break;
                case 2: // BOTTOM to TOP
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X),
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

                    speed = new Vector2(0,
                        -((SnakeGame)Game).rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
                case 3: // TOP to BOTTOM
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);

                    speed = new Vector2(0,
                        ((SnakeGame)Game).rnd.Next(enemyMinSpeed,
                        enemyMaxSpeed));
                    break;
            }

            // Get random number between 0 and 99
            int random = ((SnakeGame)Game).rnd.Next(100);
            if (random < likelihoodAutomated)
            {
                // Create an AutomatedSprite.
                // Get new random number to determine whether to
                // create a three-blade or four-blade sprite.
                if (((SnakeGame)Game).rnd.Next(2) == 0)
                {
                    // Create a four-blade enemy
                    spriteList.Add(
                    new AutomatedSprite(
                        fourbladesTexture,
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "fourbladescollision",
                        automatedSpritePointValue));
                }
                else
                {
                    // Create a three-blade enemy
                    spriteList.Add(
                    new AutomatedSprite(
                        threebladesTexture,
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "threebladescollision",
                        automatedSpritePointValue));
                }
            }
            else if (random < likelihoodAutomated +
            likelihoodChasing)
            {
                // Create a ChasingSprite.
                // Get new random number to determine whether
                // to create a skull or a plus sprite.
                if (((SnakeGame)Game).rnd.Next(2) == 0)
                {
                    // Create a skull
                    spriteList.Add(
                    new ChasingSprite(
                        skullballTexture,
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 8), speed, "skullcollision", this,
                        chasingSpritePointValue));
                }
                else
                {
                    // Create a plus
                    spriteList.Add(
                    new ChasingSprite(
                        plusTexture,
                        position, new Point(75, 75), 10, new Point(0, 0),
                        new Point(6, 4), speed, "pluscollision", this,
                        chasingSpritePointValue));
                }
            }
            else
            {
                // Create an EvadingSprite
                spriteList.Add(
                new EvadingSprite(
                    boltTexture,
                    position, new Point(75, 75), 10, new Point(0, 0),
                    new Point(6, 8), speed, "boltcollision", this,
                    .75f, 150, evadingSpritePointValue));
            }
        }

        private void SpawnRat()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            // Default frame size
            Point frameSize = new Point(40, 40);
            Point currentFrame = new Point(0, 0);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((SnakeGame)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                        -frameSize.X, ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(((SnakeGame)Game).rnd.Next(
                        ratMinSpeed,
                        ratMaxSpeed), 0);

                    currentFrame.Y = 2;
                    break;

                case 1: // RIGHT to LEFT
                    position = new
                        Vector2(
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                        ((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                        - frameSize.Y));

                    speed = new Vector2(-((SnakeGame)Game).rnd.Next(
                       ratMinSpeed, ratMaxSpeed), 0);

                    currentFrame.Y = 1;
                    break;

                case 2: // BOTTOM to TOP
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X),
                        Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

                    speed = new Vector2(0,
                       -((SnakeGame)Game).rnd.Next(ratMinSpeed,
                       ratMaxSpeed));

                    currentFrame.Y = 3;
                    break;

                case 3: // TOP to BOTTOM
                    position = new Vector2(((SnakeGame)Game).rnd.Next(0,
                        Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                        - frameSize.X), -frameSize.Y);

                    speed = new Vector2(0,
                        ((SnakeGame)Game).rnd.Next(ratMinSpeed,
                        ratMaxSpeed));
                    break;
            }

            // Create new Rat
            rats.Add(
                     new Rat(ratTexture, position, new Point(40, 40), 10, currentFrame,
                new Point(3, 4), speed, "boltcollision", this,
                1f, 150, evadingSpritePointValue)); ;

        }



        // Return current position of the player sprite
        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }

        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            // If the spawn max time is > 500 milliseconds
            // decrease the spawn time if it is time to do
            // so based on the spawn-timer variables
            if (enemySpawnMaxMilliseconds > 500)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (enemySpawnMaxMilliseconds > 1000)
                    {
                        enemySpawnMaxMilliseconds -= 100;
                        enemySpawnMinMilliseconds -= 100;
                    }
                    else
                    {
                        enemySpawnMaxMilliseconds -= 10;
                        enemySpawnMinMilliseconds -= 10;
                    }
                }
            }
        }

        protected void CheckPowerUpExpiration(GameTime gameTime)
        {
            // Is a power-up active?
            if (powerUpExpiration > 0)
            {
                // Decrement power-up timer
                powerUpExpiration -= gameTime.ElapsedGameTime.Milliseconds;
                if (powerUpExpiration <= 0)
                {
                    // If power-up timer has expired, end all power-ups
                    powerUpExpiration = 0;
                    player.ResetScale();
                    player.ResetSpeed();
                }
            }
        }

        public void setNumberLivesRemaining(int value)
        {
            this.numberLivesRemaining = value;
        }

        public int getNumberLivesRemaining()
        {
            return this.numberLivesRemaining;
        }

        public bool isGameEnd()
        {
            return (this.numberLivesRemaining == 0);
        }

        public void AddScore(int score)
        {
            this.playerScore += score;
        }

        public int getCurrentScore()
        {
            return this.playerScore;
        }
    }
}
