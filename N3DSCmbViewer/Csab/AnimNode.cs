using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N3DSCmbViewer.Csab
{
    class AnimNode
    {
        public const string AnimNodeTag = "anod";

        public string Tag { get; private set; }

        public AnimTrack boneIndex { get; private set; }
        public AnimTrack translationX { get; private set; }
        public AnimTrack translationY { get; private set; }
        public AnimTrack translationZ { get; private set; }
        public AnimTrack rotationX { get; private set; }
        public AnimTrack rotationY { get; private set; }
        public AnimTrack rotationZ { get; private set; }
        public AnimTrack scaleX { get; private set; }
        public AnimTrack scaleY { get; private set; }
        public AnimTrack scaleZ { get; private set; }




        public AnimNode(GameVersion version,CsabChunk parentCsab, int offset)
        {
            Tag = Encoding.ASCII.GetString(parentCsab.ChunkData, offset, 4).TrimEnd(' ');
            if (Tag != AnimNodeTag) throw new Exception(string.Format("Trying to read data with tag '{0}' as {1}, expected tag '{2}'", Tag, this.GetType().Name, AnimNodeTag));


            uint boneIndex = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x04);

            uint translationXOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x08);
            uint translationYOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x0A);
            uint translationZOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x0C);
            uint rotationXOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x0E);
            uint rotationYOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x10);
            uint rotationZOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x12);
            uint scaleXOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x14);
            uint scaleYOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x16);
            uint scaleZOffs = BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x18);


            if (BitConverter.ToUInt16(parentCsab.ChunkData, offset+ 0x1A) != 0x00) throw new Exception(string.Format("Expecting byte '{0}' to be {1}, instead got  '{2}'", offset+ 0x1A, 0x00, BitConverter.ToUInt16(parentCsab.ChunkData, offset + 0x1A)));


            translationX = translationXOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(translationXOffs)) : null;
            translationY = translationYOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(translationYOffs)) : null;
            translationZ = translationZOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(translationZOffs)) : null;
            rotationX = rotationXOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(rotationXOffs)) : null;
            rotationY = rotationYOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(rotationYOffs)) : null;
            rotationZ = rotationZOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(rotationZOffs)) : null;
            scaleX = scaleXOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(scaleXOffs)) : null;
            scaleY = scaleYOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(scaleYOffs)) : null;
            scaleZ = scaleZOffs != 0 ? new AnimTrack(version, parentCsab, Convert.ToInt32(scaleZOffs)) : null;

        }



    }
}
