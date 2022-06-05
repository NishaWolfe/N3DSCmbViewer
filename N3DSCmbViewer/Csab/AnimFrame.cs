using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N3DSCmbViewer.Csab
{
    class AnimFrame
    {

        public AnimationType type { get; private set; }
        public uint time { get; private set; }
        public uint value { get; private set; }
        public uint tangentIn { get; private set; }
        public uint tangentOut { get; private set; }

        public AnimFrame(GameVersion version, AnimationType animType, CsabChunk parentCsab, int offset)
        {
            type = animType;
            switch (version)
            {
                case GameVersion.Ocarina:
                    parseFrameOcarina(animType, parentCsab, offset);
                    break;
                case GameVersion.Majora:
                    //parseTrackMajora(animType, parentCsab, offset);
                    break;
            }
        }

        private void parseFrameOcarina(AnimationType animType, CsabChunk parentCsab, int offset)
        {
            time = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x00);
            value = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x04);

            if (animType == AnimationType.Hermite)
            {
                tangentIn = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x08);
                tangentOut = BitConverter.ToUInt32(parentCsab.ChunkData, offset + 0x0C);
            }

        }



    }
}
