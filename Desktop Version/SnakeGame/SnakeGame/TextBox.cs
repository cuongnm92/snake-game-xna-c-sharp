using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
{
    class TextBox
    {
        const int maxLength = 10;
        private Dictionary<Keys, char> characterByKey;
        private string name;
        private int len;

        public TextBox()
        {
            characterByKey = new Dictionary<Keys, char>()
            {
                {Keys.A, 'a'},
                {Keys.B, 'b'},
                {Keys.C, 'c'},
                {Keys.D, 'd'},
                {Keys.E, 'e'},
                {Keys.F, 'f'},
                {Keys.G, 'g'},
                {Keys.H, 'h'},
                {Keys.I, 'i'},
                {Keys.J, 'j'},
                {Keys.K, 'k'},
                {Keys.L, 'l'},
                {Keys.M, 'm'},
                {Keys.N, 'n'},
                {Keys.O, 'o'},
                {Keys.P, 'p'},
                {Keys.Q, 'q'},
                {Keys.R, 'r'},
                {Keys.S, 's'},
                {Keys.T, 't'},
                {Keys.U, 'u'},
                {Keys.V, 'v'},
                {Keys.W, 'w'},
                {Keys.X, 'x'},
                {Keys.Y, 'y'},
                {Keys.Z, 'z'},
                {Keys.D0, '0'},
                {Keys.D1, '1'},
                {Keys.D2, '2'},
                {Keys.D3, '3'},
                {Keys.D4, '4'},
                {Keys.D5, '5'},
                {Keys.D6, '6'},
                {Keys.D7, '7'},
                {Keys.D8, '8'},
                {Keys.D9, '9'},
                {Keys.NumPad0, '0'},
                {Keys.NumPad1, '1'},
                {Keys.NumPad2, '2'},
                {Keys.NumPad3, '3'},
                {Keys.NumPad4, '4'},
                {Keys.NumPad5, '5'},
                {Keys.NumPad6, '6'},
                {Keys.NumPad7, '7'},
                {Keys.NumPad8, '8'},
                {Keys.NumPad9, '9'},
                {Keys.OemPeriod, '.'},
                {Keys.OemMinus, '-'},
                {Keys.Space, ' '}
            };

            ResetName();
        }

        public void ResetName()
        {
            this.len = 0;
            this.name = "";
        }

        public void AddKey(Keys k)
        {
            if (len >= maxLength) return;

            if (characterByKey.ContainsKey(k))
            {

                this.name = this.name + characterByKey[k];

                len++;
            }
        }

        public string getName()
        {
            return this.name;
        }
    }
}
