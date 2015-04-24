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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

       
        private Vector3 position;
        private float angle;
        private KeyboardState oldState;

        Camera camera = new Camera();

        Matrix shipWorld;
        Model shipModel;

        Matrix astroWorld;
        Model astroModel;

        Matrix astro2World;
        Model astro2Model;

        Matrix astro3World;
        Model astro3Model;

        private Vector3 astroFly;
        private Vector3 astroFly2;
        private Vector3 astroFly3;
              
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

   

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
            shipWorld = Matrix.Identity;
            astroWorld = Matrix.Identity;
            
            astro2World = Matrix.Identity;
            
            astro3World = Matrix.Identity;
            

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
                       
            // TODO: use this.Content to load your game content here

            shipModel = Content.Load<Model>(@"Ship\Ship");
            astroModel = Content.Load<Model>("LargeAsteroid");
            astro2Model = Content.Load<Model>("LargeAsteroid");
            astro3Model = Content.Load<Model>("LargeAsteroid");

            astroFly = new Vector3(0, 10, 0);
            astroFly2 = new Vector3(-1, 15, -2);
            astroFly3 = new Vector3(2, 20, -1);
            position = new Vector3(0, 0, 0);
            angle = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            camera.Update();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            KeyboardState newState = Keyboard.GetState();  // get the newest state
                  
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            // quit game
            
            if (state.IsKeyDown(Keys.A))
            {
                position += new Vector3(-0.1f, 0, 0);
                shipWorld = Matrix.CreateTranslation(position);
            }
            if (state.IsKeyDown(Keys.D))
            {
                position += new Vector3(0.1f, 0, 0);
                shipWorld = Matrix.CreateTranslation(position);
            }
            if (state.IsKeyDown(Keys.Space))
            {
                position += new Vector3(0, 0, 0.1f);
                shipWorld = Matrix.CreateTranslation(position);
            }
            if (state.IsKeyDown(Keys.LeftShift))
            {
                position += new Vector3(0, 0, -0.1f);
                shipWorld = Matrix.CreateTranslation(position);
            }
            if (oldState.IsKeyUp(Keys.Q) && newState.IsKeyDown(Keys.Q))
            {
                position += new Vector3(-1f, 0, 0);
                angle += -1f;
                shipWorld = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(position);                
            }
            oldState = newState;  // set the new state as the old state for next time
            if (oldState.IsKeyUp(Keys.E) && newState.IsKeyDown(Keys.E))
            {
                position += new Vector3(1f, 0, 0);
                angle += 1f;
                shipWorld = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(position);                
            }
            oldState = newState;  // set the new state as the old state for next time

            astroFly += new Vector3(0, -0.05f, 0);
            astroFly2 += new Vector3(0, -0.05f, 0);
            astroFly3 += new Vector3(0, -0.05f, 0);
            astroWorld = Matrix.CreateTranslation(astroFly);
            astro2World = Matrix.CreateTranslation(astroFly2);
            astro3World = Matrix.CreateTranslation(astroFly3);

            if (IsCollision(shipModel, shipWorld, astroModel, astroWorld))
            {
                this.Exit();
            }
            if (IsCollision(shipModel, shipWorld, astro2Model, astro2World))
            {
                this.Exit();
            }
            if (IsCollision(shipModel, shipWorld, astro3Model, astro3World))
            {
                this.Exit();
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

            DrawModel(shipModel, shipWorld);
            DrawModel(astroModel, astroWorld);
            DrawModel(astro2Model, astro2World);
            DrawModel(astro3Model, astro3World);

            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix worldMatrix)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
        }

        private bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
    }


}
