using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N3DSCmbViewer.Csab
{
    class AnimTrack
    {

        public AnimationType type { get; private set; }
        public AnimFrame[] frames { get; private set; }
        public int timeEnd { get; private set; }


        public AnimTrack(GameVersion version, CsabChunk parentCsab, int offset)
        {
            switch (version)
            {
                case GameVersion.Ocarina:
                    parseTrackOcarina(parentCsab, offset);
                    break;
                case GameVersion.Majora:
                    //parseTrackMajora(parentCsab, offset);
                    break;
            }
        }


        public AnimationType UIntToAnimationType(uint i)
        {
            switch (i)
            {
                case 0x01:
                    return AnimationType.Linear;
                    break;
                case 0x02:
                    return AnimationType.Hermite;
                    break;
            }


            return AnimationType.Linear;
        }

        public void parseTrackOcarina(CsabChunk parentCsab, int offset)
        {

            type = UIntToAnimationType(BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x00));
            uint numKeyframes = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x04);
            uint unk1 = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x08);
            timeEnd = Convert.ToInt32(BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x0C) + 1);

            int keyframeTableIdx = 0x10;

            if (type == AnimationType.Linear)
            {

                frames = new AnimFrame[numKeyframes];
                for (int i = 0; i < numKeyframes; i++)
                {
                    frames[i] = new AnimFrame(GameVersion.Ocarina, type, parentCsab, offset + keyframeTableIdx);

                    keyframeTableIdx += 0x08;
                }
            }
            else if (type == AnimationType.Hermite)
            {
                frames = new AnimFrame[numKeyframes];
                for (int i = 0; i < numKeyframes; i++)
                {
                    frames[i] = new AnimFrame(GameVersion.Ocarina, type, parentCsab, offset + keyframeTableIdx);

                    keyframeTableIdx += 0x10;
                }
            }
            else
            {
                throw new Exception("Somehow, the animation wasn't a valid type.");

            }

        }

    }
}
