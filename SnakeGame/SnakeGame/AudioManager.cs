using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace SnakeGame
{
    public class AudioManager : GameComponent
    {
        /// <summary>
        /// The singleton for this type.
        /// </summary>
        private static AudioManager audioManager = null;

        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private WaveBank waveBank;

        private AudioManager(Game game, string settingsFile, string waveBankFile, string soundBankFile) : base(game)
        {
            try
            {
                audioEngine = new AudioEngine(settingsFile);
                waveBank = new WaveBank(audioEngine, waveBankFile);
                soundBank = new SoundBank(audioEngine, soundBankFile);
            }
            catch (NoAudioHardwareException)
            {
                // silently fall back to silence
                audioEngine = null;
                waveBank = null;
                soundBank = null;
            }
        }

        public static void Initialize(Game game, string settingsFile, string waveBankFile, string soundBankFile)
        {
            audioManager = new AudioManager(game, settingsFile, waveBankFile,
                soundBankFile);
            if (game != null)
            {
                game.Components.Add(audioManager);
            }
        }

        public static Cue GetCue(string cueName)
        {
            if (String.IsNullOrEmpty(cueName) ||
                (audioManager == null) || (audioManager.audioEngine == null) ||
                (audioManager.soundBank == null) || (audioManager.waveBank == null))
            {
                return null;
            }
            return audioManager.soundBank.GetCue(cueName);
        }

        public static void PlayCue(string cueName)
        {
            if ((audioManager != null) && (audioManager.audioEngine != null) &&
                (audioManager.soundBank != null) && (audioManager.waveBank != null))
            {
                audioManager.soundBank.PlayCue(cueName);
            }
        }

        private Cue musicCue;
        private Stack<string> musicCueNameStack = new Stack<string>();

        public static void PlayMusic(string cueName)
        {
            // start the new music cue
            if (audioManager != null)
            {
                audioManager.musicCueNameStack.Clear();
                PushMusic(cueName);
            }
        }

        public static void PushMusic(string cueName)
        {
            // start the new music cue
            if ((audioManager != null) && (audioManager.audioEngine != null) &&
                (audioManager.soundBank != null) && (audioManager.waveBank != null))
            {
                audioManager.musicCueNameStack.Push(cueName);
                if ((audioManager.musicCue == null) ||
                    (audioManager.musicCue.Name != cueName))
                {
                    if (audioManager.musicCue != null)
                    {
                        audioManager.musicCue.Stop(AudioStopOptions.AsAuthored);
                        audioManager.musicCue.Dispose();
                        audioManager.musicCue = null;
                    }
                    audioManager.musicCue = GetCue(cueName);
                    if (audioManager.musicCue != null)
                    {
                        audioManager.musicCue.Play();
                    }
                }
            }
        }

        public static void PopMusic()
        {
            // start the new music cue
            if ((audioManager != null) && (audioManager.audioEngine != null) && (audioManager.soundBank != null) && (audioManager.waveBank != null))
            {
                string cueName = null;
                if (audioManager.musicCueNameStack.Count > 0)
                {
                    audioManager.musicCueNameStack.Pop();
                    if (audioManager.musicCueNameStack.Count > 0)
                    {
                        cueName = audioManager.musicCueNameStack.Peek();
                    }
                }
                if ((audioManager.musicCue == null) ||
                    (audioManager.musicCue.Name != cueName))
                {
                    if (audioManager.musicCue != null)
                    {
                        audioManager.musicCue.Stop(AudioStopOptions.AsAuthored);
                        audioManager.musicCue.Dispose();
                        audioManager.musicCue = null;
                    }
                    if (!String.IsNullOrEmpty(cueName))
                    {
                        audioManager.musicCue = GetCue(cueName);
                        if (audioManager.musicCue != null)
                        {
                            audioManager.musicCue.Play();
                        }
                    }
                }
            }
        }

        public static void StopMusic()
        {
            if (audioManager != null)
            {
                audioManager.musicCueNameStack.Clear();
                if (audioManager.musicCue != null)
                {
                    audioManager.musicCue.Stop(AudioStopOptions.AsAuthored);
                    audioManager.musicCue.Dispose();
                    audioManager.musicCue = null;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update the audio engine
            if (audioEngine != null)
            {
                audioEngine.Update();
            }

            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    StopMusic();
                    if (soundBank != null)
                    {
                        soundBank.Dispose();
                        soundBank = null;
                    }
                    if (waveBank != null)
                    {
                        waveBank.Dispose();
                        waveBank = null;
                    }
                    if (audioEngine != null)
                    {
                        audioEngine.Dispose();
                        audioEngine = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

    }
}
