using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisDotnet;

namespace Tetris
{
    static class StaticVars
    {
        static readonly PieceType[,] emptyArray = new PieceType[4, 4]
        {
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] iArray = new PieceType[4, 4]
        {
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.I, PieceType.I, PieceType.I, PieceType.I},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] jArray = new PieceType[3, 3]
        {
            {PieceType.J, PieceType.Empty, PieceType.Empty},
            {PieceType.J, PieceType.J, PieceType.J},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] lArray = new PieceType[3, 3]
        {
            {PieceType.Empty, PieceType.Empty, PieceType.L},
            {PieceType.L, PieceType.L, PieceType.L},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] oArray = new PieceType[4, 4]
        {
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty},
            {PieceType.Empty, PieceType.O, PieceType.O, PieceType.Empty},
            {PieceType.Empty, PieceType.O, PieceType.O, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] sArray = new PieceType[3, 3]
        {
            {PieceType.Empty, PieceType.S, PieceType.S},
            {PieceType.S, PieceType.S, PieceType.Empty},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] tArray = new PieceType[3, 3]
        {
            {PieceType.Empty, PieceType.T, PieceType.Empty},
            {PieceType.T, PieceType.T, PieceType.T},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        readonly static PieceType[,] zArray = new PieceType[3, 3]
        {
            {PieceType.Z, PieceType.Z, PieceType.Empty},
            {PieceType.Empty, PieceType.Z, PieceType.Z},
            {PieceType.Empty, PieceType.Empty, PieceType.Empty}
        };

        public static PieceType[,] GetPieceArray(PieceType type)
        {
            //Based on
            //http://vignette1.wikia.nocookie.net/tetrisconcept/images/3/3d/SRS-pieces.png/revision/latest?cb=20060626173148

            switch (type)
            {
                case PieceType.Dead:
                    return emptyArray;

                case PieceType.Empty:
                    return emptyArray;

                case PieceType.I:
                    return iArray;

                case PieceType.J:
                    return jArray;

                case PieceType.L:
                    return lArray;

                case PieceType.O:
                    return oArray;

                case PieceType.S:
                    return sArray;

                case PieceType.T:
                    return tArray;

                case PieceType.Z:
                    return zArray;

                default:
                    return emptyArray;
            }
        }

        static StaticVars()
        {
            blockTextures = new[]
            {
                new Texture("Art/blocks/placeholder_I.png"),
                new Texture("Art/blocks/placeholder_O.png"),
                new Texture("Art/blocks/placeholder_T.png"),
                new Texture("Art/blocks/placeholder_S.png"),
                new Texture("Art/blocks/placeholder_Z.png"),
                new Texture("Art/blocks/placeholder_J.png"),
                new Texture("Art/blocks/placeholder_L.png"),
                new Texture("Art/blocks/placeholder_empty.png"),
                new Texture("Art/blocks/placeholder_ghost.png")
            };

            //Create the VISIBLE grid of sprites for the game
            drawGrid = new Sprite[20, 10];

            //Basicly take the resolution of the block sprite and uses it for calculations
            imageSize = new Vector2i((int) StaticVars.blockTextures[0].Size.X,
                (int) StaticVars.blockTextures[0].Size.Y);

            gridXPos = Application.WINDOW_WIDTH / 2 -
                       drawGrid.GetLength(1) * imageSize.X / 2; //(int)drawGrid[0,0].Size.X / 2;
            gridYPos = Application.WINDOW_HEIGHT / 2 -
                       drawGrid.GetLength(0) * imageSize.Y / 2; //* (int)drawGrid[0, 0].Size.Y / 2;

            backDropTexture = new Texture("Art/background_img.png");
            holdTexture = new Texture("Art/holdbox_Frame.png");
            queueTexture = new Texture("Art/queuebox_Frame.png");
            drawGridBackgroundTexture = new Texture("Art/gridbox_Frame.png");
            statsTexture = new Texture("Art/stats.png");

            backDrop = new Sprite(backDropTexture);
            queueSprite = new Sprite(queueTexture);
            holdSprite = new Sprite(holdTexture);
            drawGridSprite = new Sprite(drawGridBackgroundTexture);
            statsSprite = new Sprite(statsTexture);

            flat = new Vector2i(0, 0);
            down = new Vector2i(0, 1);
            left = new Vector2i(-1, 0);
            right = new Vector2i(1, 0);

            font = new Font("consola.ttf");

            pauseText = new Text("Paused", font, 40);

            controlsText =
                new Text(
                    "←/→ - Move piece\n↑ - Rotate piece\n↓ - Soft Drop\nSpace - Hard Drop\nC - Hold piece\nP - Pause\nEsc/M - Menu",
                    font, 20);
            controlsText.Color = Color.Green;
        }


        public static int gridXPos { get; set; }

        public static int gridYPos { get; set; }

        public static Texture[] blockTextures { get; set; }

        public static Texture backDropTexture { get; set; }

        public static Texture holdTexture { get; set; }

        public static Texture queueTexture { get; set; }

        public static Texture drawGridBackgroundTexture { get; set; }

        public static Sprite backDrop { get; set; }

        public static Sprite queueSprite { get; set; }

        public static Sprite holdSprite { get; set; }

        public static Sprite drawGridSprite { get; set; }

        public static Vector2i flat { get; set; }

        public static Vector2i down { get; set; }

        public static Vector2i left { get; set; }

        public static Vector2i right { get; set; }

        public static Font font { get; set; }

        public static Text pauseText { get; set; }

        public static Sprite[,] drawGrid { get; set; }

        public static Vector2i imageSize { get; set; }

        public static Texture statsTexture { get; set; }

        public static Sprite statsSprite { get; set; }

        public static Text controlsText { get; private set; }
    }
}