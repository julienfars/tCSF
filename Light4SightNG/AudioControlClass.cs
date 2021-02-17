using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SlimDX;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinMM;
using System.IO;


namespace Light4SightNG
{
    public class AudioControlClass : IDisposable
    {
        long cbWaveSize;
        static readonly XAudio2 AudioDevice = new XAudio2();

        SlimDX.Multimedia.WaveFormat SignalFormat = null;
        readonly MasteringVoice WaveMasterVoice = new MasteringVoice(AudioDevice);
        AudioBuffer WaveBuffer = null;
        MemoryStream WaveMemStream = null;
        SourceVoice WaveSourceVoice = null;
        Thread m_soundThread = null;
        ThreadStart soundThreadStart = null;

        public static byte[] WaveDaten { get; set; }

        bool disposed;

        readonly static double _DeltaPhiSinus = (2 * Math.PI / 360);
        public static double DeltaPhiSinus { get { return _DeltaPhiSinus; } } 

        readonly static double _DeltaPhi = AbtastFrequenz / 360;
        public static double DeltaPhi { get { return _DeltaPhi; } }
        
        readonly static Int16 _AnzahlKanaele = 8;
        public static Int16 AnzahlKanaele { get { return _AnzahlKanaele; } } 

        readonly static int _AbtastFrequenz = 96000;
        public static int AbtastFrequenz { get { return _AbtastFrequenz; } }

        readonly static double _DeltaPI = (2 * Math.PI / AbtastFrequenz);
        public static double DeltaPI { get { return _DeltaPI; } } 

        readonly static int _BytesProSekunde = AnzahlKanaele * AbtastFrequenz * 2;
        public static int BytesProSekunde { get { return _BytesProSekunde; } }

        readonly static Int16 _SampleContainerGroesse = 16;
        public static Int16 SampleContainerGroesse { get { return _SampleContainerGroesse; } }

        readonly static Int16 _Blockausrichtung = (Int16)(AnzahlKanaele * SampleContainerGroesse / 8);
        public static Int16 Blockausrichtung { get { return _Blockausrichtung; } }

        readonly static int _TraegerFrequenz = 20000;
        public static int TraegerFrequenz { get { return _TraegerFrequenz; } }

        readonly static int _SampleLaenge = 1;
        public static int SampleLaenge { get { return _SampleLaenge; } }

        public static byte Highbyte(Int16 HiTemp)
        {
            byte hi = (byte)(HiTemp >> 8);
            return hi;
        }

        public static byte Lowbyte(Int16 LowTemp)
        {
            byte lo = (byte)(LowTemp & 255);
            return lo;
        }

        public AudioControlClass()
        {
            SignalFormat = new SlimDX.Multimedia.WaveFormat();
            SignalFormat.AverageBytesPerSecond = AudioControlClass.BytesProSekunde;
            SignalFormat.BlockAlignment = AudioControlClass.Blockausrichtung;
            SignalFormat.Channels = AudioControlClass.AnzahlKanaele;
            SignalFormat.SamplesPerSecond = AudioControlClass.AbtastFrequenz;
            SignalFormat.BitsPerSample = AudioControlClass.SampleContainerGroesse;
            SignalFormat.FormatTag = SlimDX.Multimedia.WaveFormatTag.Pcm;
            cbWaveSize = AudioControlClass.SampleLaenge * SignalFormat.Channels * SignalFormat.SamplesPerSecond * SignalFormat.BitsPerSample / 8;
        }

        public bool InitWaveContainer()
        {
            try
            {
                WaveDaten = new byte[cbWaveSize];
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool ClearWaveContainer()
        {
            try
            {
                WaveDaten = null;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PlaySignal()
        {

            if (Globals.bPlaySignal) return false;

            soundThreadStart = new ThreadStart(SoundPlayerThread);
            m_soundThread = new Thread(soundThreadStart);
            m_soundThread.Start();

            return true;
        }

        void SoundPlayerThread()
        {
            Globals.bPlaySignal = true;

            WaveMemStream = new MemoryStream(WaveDaten);

            WaveBuffer = new AudioBuffer();
            WaveBuffer.Flags = BufferFlags.EndOfStream;
            WaveBuffer.AudioData = WaveMemStream;
            // WaveBuffer.AudioBytes = clGlobals.BytesProSekunde;
            WaveBuffer.AudioBytes = (int)WaveMemStream.Length;
            WaveBuffer.LoopCount = XAudio2.LoopInfinite;

            WaveSourceVoice = new SourceVoice(AudioDevice, SignalFormat);
            WaveSourceVoice.SubmitSourceBuffer(WaveBuffer);

            WaveSourceVoice.Start();

            while (Globals.bPlaySignal)
            {
                Thread.Sleep(10);
            }

            WaveSourceVoice.Stop();
            Thread.Sleep(10);
            WaveMemStream.Close();
            WaveMemStream.Dispose();
            WaveMemStream = null;
            WaveBuffer.Dispose();
            WaveBuffer = null;
            WaveSourceVoice.Dispose();
            WaveSourceVoice = null;
            // Thread.Sleep(100);
            soundThreadStart = null;
            // this.ClearWaveContainer();
            // this.InitWaveContainer();
            this.m_soundThread.Abort();
        }

        public void StopSignal()
        {
            if (Globals.bPlaySignal == true)
            {
                Globals.bPlaySignal = false;
            }
        }

        public void SetSignal(double[] signal, int channel)
        {
            double[] signalCopy;

            signalCopy = new double[signal.Length];
            for (int i = 0; i < signal.Length; i++)
            {
                signalCopy[i] = signal[i];
            }

            double dWinkel = 0.0;

            for (int i = 0; i < signalCopy.Length; i++)
            {
                signalCopy[i] *= Math.Sin(dWinkel);
                dWinkel += 2 * Math.PI * AudioControlClass.TraegerFrequenz / AudioControlClass.AbtastFrequenz;
                if (dWinkel > 2 * Math.PI)
                    dWinkel -= 2 * Math.PI;
                WriteToWaveContainer(signalCopy[i], channel, i);
            }
        }

        public void WriteToWaveContainer(double dValue, int iChannel, int iPosition)
        {
            WaveDaten[(((iPosition * 8) + iChannel) * 2)] = AudioControlClass.Lowbyte((Int16)dValue);
            WaveDaten[(((iPosition * 8) + iChannel) * 2) + 1] = AudioControlClass.Highbyte((Int16)dValue);
        }

        ~AudioControlClass()
        {
            this.StopSignal();
            this.Dispose();
        }

        #region IDisposable Member

        public void Dispose()
        {
            this.StopSignal();
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (WaveMemStream != null) WaveMemStream.Dispose();
                    if (WaveBuffer != null) WaveBuffer.Dispose();
                    if (WaveSourceVoice != null) WaveSourceVoice.Dispose();
                    WaveMasterVoice.Dispose();
                    AudioDevice.Dispose();
                }
            }
            disposed = true;
        }

        #endregion
    }


}
