using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Dalamud.Logging;

namespace SWMGPlugin.Helpers.AudioHelper
{
    public class AudioHelper
    {
        private static AudioHelper _instance;
        public static AudioHelper Instance = _instance ??= new();

        private static WaveOutEvent Output;
        private static MixingSampleProvider Mixer;

        AudioHelper()
        {
            Output = new WaveOutEvent();
            Mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
            {
                ReadFully = true
            };
            Output.Init(Mixer);
            Output.Play();
        }

        private ISampleProvider FixChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == Mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && Mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }

            throw new NotImplementedException($"Channel count mismatch. Soundcard is {Mixer.WaveFormat.Channels}ch but audio is {input.WaveFormat.Channels}ch.");
        }

        public static void Teardown()
        {
            Output.Dispose();
        }

        public AudioFileReader LoadFromData(string filename)
        {
            var path = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, filename);
            return new AudioFileReader(path);
        }

        public CachedSound CacheFromData(string filename)
        {
            using var afr = LoadFromData(filename);
            var cached = new CachedSound(afr);
            return cached;
        }

        public void PlayOneshot(ISampleProvider audio)
        {
            var config = Plugin.PluginInterface.GetPluginConfig() as Configuration;

            var adjustAudio = new VolumeSampleProvider(FixChannelCount(audio));
            adjustAudio.Volume = config == null ? 0.1f : config.Volume;

            Mixer.AddMixerInput(adjustAudio);
        }

        public void PlayOneshot(CachedSound audio)
        {
            var prov = new CachedSoundSampleProvider(audio);
            PlayOneshot(prov);
        }

        public void PlayWavOneshot(AudioFileReader audio)
        {
            audio.Seek(0, SeekOrigin.Begin);
            PlayOneshot(audio);
        }
    }
}
