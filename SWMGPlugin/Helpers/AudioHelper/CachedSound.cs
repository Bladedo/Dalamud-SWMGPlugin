using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace SWMGPlugin.Helpers.AudioHelper
{
    public class CachedSound
    {
        public float[] Samples { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public CachedSound(AudioFileReader audio)
        {
            WaveFormat = audio.WaveFormat;
            var wholeSamples = new List<float>((int)(audio.Length / 4));
            var readBuf = new float[audio.WaveFormat.SampleRate * audio.WaveFormat.Channels];
            int sampsRead;
            while ((sampsRead = audio.Read(readBuf, 0, readBuf.Length)) > 0)
            {
                wholeSamples.AddRange(readBuf.Take(sampsRead));
            }

            Samples = wholeSamples.ToArray();
        }
    }
}
