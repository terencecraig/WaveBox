using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WaveBox.Core.Extensions;
using WaveBox.Core.Model;
using WaveBox.Service;
using WaveBox.Transcoding;

namespace WaveBox.Service.Services {
    public class TranscodeService : IService {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get { return "transcode"; } set { } }

        public bool Required { get { return true; } set { } }

        public bool Running { get; set; }

        private IList<ITranscoder> transcoders = new List<ITranscoder>();

        public TranscodeService() {
        }

        public bool Start() {
            // Ready to roll!
            this.Running = true;
            return true;
        }

        public bool Stop() {
            // Stop all transcodes
            this.CancelAllTranscodes();

            return true;
        }

        public ITranscoder TranscodeSong(IMediaItem song, TranscodeType type, uint quality, bool isDirect, uint offsetSeconds, uint lengthSeconds) {
            if (!this.Running) {
                logger.Error("TranscodeService is not running!");
                return null;
            }

            logger.IfInfo("Asked to transcode song: " + song.FileName);
            lock (transcoders) {
                ITranscoder transcoder = null;
                switch (type) {
                case TranscodeType.MP3:
                    transcoder = new FFMpegMP3Transcoder(song, quality, isDirect, offsetSeconds, lengthSeconds);
                    break;
                case TranscodeType.OGG:
                    transcoder = new FFMpegOGGTranscoder(song, quality, isDirect, offsetSeconds, lengthSeconds);
                    break;
                case TranscodeType.OPUS:
                    transcoder = new FFMpegOpusTranscoder(song, quality, isDirect, offsetSeconds, lengthSeconds);
                    break;
                case TranscodeType.AAC:
                    transcoder = new FFMpegAACTranscoder(song, quality, isDirect, offsetSeconds, lengthSeconds);
                    break;
                }

                transcoder = StartTranscoder(transcoder);

                return transcoder;
            }
        }

        public ITranscoder TranscodeVideo(IMediaItem video, TranscodeType type, uint quality, bool isDirect, uint? width, uint? height, bool maintainAspect, uint offsetSeconds, uint lengthSeconds) {
            if (!this.Running) {
                logger.Error("TranscodeService is not running!");
                return null;
            }

            logger.IfInfo("Asked to transcode video: " + video.FileName);
            lock (transcoders) {
                ITranscoder transcoder = null;;
                switch (type) {
                case TranscodeType.X264:
                    transcoder = new FFMpegX264Transcoder(video, quality, isDirect, width, height, maintainAspect, offsetSeconds, lengthSeconds);
                    break;
                case TranscodeType.MPEGTS:
                    transcoder = new FFMpegMpegtsTranscoder(video, quality, isDirect, width, height, maintainAspect, offsetSeconds, lengthSeconds);
                    break;
                }

                transcoder = StartTranscoder(transcoder);

                return transcoder;
            }
        }

        private ITranscoder StartTranscoder(ITranscoder inTranscoder) {
            ITranscoder transcoder = inTranscoder;
            if ((object)transcoder != null) {
                // Don't reuse direct transcoders
                if (!transcoder.IsDirect && transcoders.Contains(transcoder)) {
                    logger.IfInfo("Using existing transcoder");

                    // Get the existing transcoder
                    int index = transcoders.IndexOf(transcoder);
                    transcoder = transcoders[index];

                    // Increment the reference count
                    transcoder.ReferenceCount++;
                } else {
                    logger.IfInfo("Creating a new transcoder");

                    // Add the transcoder to the array
                    transcoders.Add(transcoder);

                    // Increment the reference count
                    transcoder.ReferenceCount++;

                    // Start the transcode process
                    transcoder.StartTranscode();
                }
            }

            return transcoder;
        }

        public void ConsumedTranscode(ITranscoder transcoder) {
            logger.IfInfo("Waiting on " + transcoder.Item.FileName + " for 30 more seconds... State: " + transcoder.State);

            for (int i = 30; i > 0; i--) {
                Thread.Sleep(1000);
            }
            // Do nothing if the transcoder is null or is a stdout transcoder
            if ((object)transcoder == null) {
                return;
            }

            if (transcoder.IsDirect && transcoder.State == TranscodeState.Active) {
                try {
                    // Kill the running transcode
                    transcoder.TranscodeProcess.Kill();
                } catch {}
            }

            lock (transcoders) {
                logger.IfInfo("Consumed transcoder for " + transcoder.Item.FileName);

                // Decrement the reference count
                transcoder.ReferenceCount--;

                if (transcoder.ReferenceCount == 0) {
                    // No other clients need this file, remove it
                    transcoders.Remove(transcoder);

                    if (!transcoder.IsDirect) {
                        // Remove the file
                        File.Delete(transcoder.OutputPath);
                    }
                }
            }
        }

        public void CancelTranscode(ITranscoder transcoder) {
            // Do nothing if the transcoder is null or is a stdout transcoder
            if ((object)transcoder == null) {
                return;
            }

            lock (transcoders) {
                logger.IfInfo("Cancelling transcoder for " + transcoder.Item.FileName);

                if (transcoder.ReferenceCount == 1) {
                    // No one else is using this transcoder, so cancel it
                    transcoder.CancelTranscode();
                }

                // Consume the transcoder
                ConsumedTranscode(transcoder);
            }
        }

        public void CancelAllTranscodes() {
            List<ITranscoder> tempTranscoders = new List<ITranscoder>();
            tempTranscoders.AddRange(transcoders);
            foreach (ITranscoder transcoder in tempTranscoders) {
                CancelTranscode(transcoder);
            }
        }

        /*
         * Transcoder delegate
         */

        public void TranscodeFinished(ITranscoder transcoder) {
            // Do something
            logger.IfInfo("Transcode finished for " + transcoder.Item.FileName);
        }

        public void TranscodeFailed(ITranscoder transcoder) {
            // Do something
            logger.IfInfo("Transcode failed for " + transcoder.Item.FileName);
        }
    }
}
