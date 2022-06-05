using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Code shamelessly taken from noclip.website
 * and converted to C#
 * https://github.com/magcius/noclip.website
 * 
 */
namespace N3DSCmbViewer.Csab
{
    class CsabChunk : BaseCTRChunk
    {
        // "CTR Skeletal Animation Binary"?
        public override string ChunkTag { get { return "csab"; } }


        public uint duration { get; private set; }
        public LoopMode loopMode { get; private set; }
        public Int16[] boneToAnimationTable { get; private set; }
        public AnimNode[] animationNodes { get; private set; }



        public CsabChunk(byte[] data, int offset, BaseCTRChunk parent)
            : base(data, offset, parent)
        {

            if (BaseCTRChunk.IsMajora3D)
            {
                //Majora's Mask animations are currently broken.
                //TODO: Fix maybe?
                /*
                NumberOfAnimations = BitConverter.ToUInt32(ChunkData, 0x3C);
                Version version = Version.Majora;

                AnimationOffsets = new uint[NumberOfAnimations];
                for (int i = 0; i < AnimationOffsets.Length; i++) AnimationOffsets[i] = BitConverter.ToUInt32(ChunkData, 0x58 + (i * sizeof(uint)));

                Animations = new Animation[NumberOfAnimations];
                for (int i = 0; i < Animations.Length; i++) Animations[i] = new Animation(this, (int)AnimationOffsets[i] + 0x28);*/
            }
            else
            {

                uint size = BitConverter.ToUInt32(ChunkData, 0x04);

                uint subversion = BitConverter.ToUInt32(ChunkData, 0x08);
                GameVersion version = GameVersion.Ocarina;



                if (subversion != 0x03) return;
                if (BitConverter.ToUInt32(ChunkData, 0x0C) != 0x00) return;
                if (BitConverter.ToUInt32(ChunkData, 0x10) != 0x01) return;  // num animations?
                if (BitConverter.ToUInt32(ChunkData, 0x14) != 0x18) return; // location?
                if (BitConverter.ToUInt32(ChunkData, 0x18) != 0x00) return;
                if (BitConverter.ToUInt32(ChunkData, 0x1C) != 0x00) return;
                if (BitConverter.ToUInt32(ChunkData, 0x20) != 0x00) return;
                if (BitConverter.ToUInt32(ChunkData, 0x24) != 0x00) return;



                duration = BitConverter.ToUInt32(ChunkData, 0x28) + 1;
                // loop mode?
                // if (BitConverter.ToUInt32(ChunkData, 0x2C) != 0x00) return;

                loopMode = LoopMode.REPEAT;
                uint anodCount = BitConverter.ToUInt32(ChunkData, 0x30);
                uint boneCount = BitConverter.ToUInt32(ChunkData, 0x34);
                if (anodCount > boneCount) return;

                // This appears to be an inverse of the bone index in each array, probably for fast binding?
                //const boneToAnimationTable = new Int16Array(boneCount);
                boneToAnimationTable = new Int16[boneCount];

                int boneTableIdx = 0x38;
                for (int i = 0; i < boneCount; i++)
                {
                    boneToAnimationTable[i] = BitConverter.ToInt16(ChunkData,boneTableIdx + 0x00);
                    boneTableIdx += 0x02;
                }

                // TODO(jstpierre): This doesn't seem like a Grezzo thing to do.
                int anodTableIdx = AlignInt(boneTableIdx, 0x04);


                animationNodes = new AnimNode[anodCount];
                for (int i = 0; i < anodCount; i++)
                {
                    uint offs = BitConverter.ToUInt32(ChunkData, anodTableIdx + 0x00);
                    animationNodes[i] = new AnimNode(version,this, Convert.ToInt32(offs) + 0x18);// .push(parseAnod());
                    anodTableIdx += 0x04;
                }


            }
            
        }

        private int AlignInt(int n, int multiple)
        {
            int mask = (multiple - 1);
            return (n + mask) & ~mask;
        }
    }
}
public enum LoopMode
{
    ONCE, REPEAT,
}
public enum GameVersion
{
    Ocarina, Majora
}
public enum AnimationType
{
    Linear, Hermite
}