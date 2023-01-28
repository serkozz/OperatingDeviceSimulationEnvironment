using System;

namespace СourseWork
{
    class Registers
    {
        public uint A;

        public uint B;

        public uint C = default;

        public byte CH = default;

        public bool PP = default;

        public Registers(uint a, uint b)
        {
            A = a;
            B = b;
        }

        #region Микрооперации

        // ПП:=1
        public void Y0()
        {
            PP = true;
        }

        // C:=A[14:0]
        public void Y1(uint A)
        {
            var subString = StaticMethods.GetSubstring(A, 14, 0);
            C = Convert.ToUInt32(subString, 2);
        }

        // A[14:0]:=B[14:0]
        public void Y2()
        {
            var subString = StaticMethods.GetBit(A, 15) + StaticMethods.GetSubstring(B, 14, 0);
            A = Convert.ToUInt32(subString, 2);
        }

        // ПП:=0
        public void Y3()
        {
            PP = false;
        }

        // C:=C+11.~A[14:0]+1
        public void Y4()
        {
            var subString = StaticMethods.GetSubstring(A, 14, 0);
            var invertString = "11" + StaticMethods.Invert(subString);
            C += Convert.ToUInt32(invertString, 2) + 1;
        }

        // C:=C+A[14:0]
        public void Y5()
        {
            var subString = StaticMethods.GetSubstring(A, 14, 0);
            C += Convert.ToUInt32(subString, 2);
        }

        // C:=L1(C.0)
        public void Y6()
        {
            C <<= 1;
        }

        // B[15:0]:=0
        public void Y7()
        {
            var subString = StaticMethods.GetBit(B, 16) + new string('0', 16);
            B = Convert.ToUInt32(subString, 2);
        }

        // СЧ:=0
        public void Y8()
        {
            CH = 0;
        }

        // B[15:0]:=L1(B[15:0].~С[16])
        public void Y9()
        {
            var CBit = StaticMethods.GetBit(C, 16).ToString();
            var invertCBit = StaticMethods.Invert(CBit);
            var bSubstring = StaticMethods.GetBit(B, 16) + StaticMethods.GetSubstring(B, 14, 0) + invertCBit;
            B = Convert.ToUInt32(bSubstring, 2);
        }

        // СЧ:=СЧ–1
        public void Y10()
        {
            if (CH == 0)
            {
                CH = 15;
            }
            else
            {
                CH--;
            }
        }

        // C:=B[15:0]
        public void Y11()
        {
            var subString = StaticMethods.GetSubstring(B, 15, 0);
            C = Convert.ToUInt32(subString, 2);
        }

        // C[16:1]:=C[16:1]+1
        public void Y12()
        {
            C += 2;
        }

        // C[16]:=1
        public void Y13()
        {
            var subString = "1" + StaticMethods.GetSubstring(C, 15, 0);
            C = Convert.ToUInt32(subString, 2);
        }

        #endregion

        #region ЛУ

        //A [14:0]=0
        public bool GetX0()
        {
            var subString = StaticMethods.GetSubstring(A, 14, 0);
            bool result = Convert.ToUInt32(subString, 2) == 0;
            return result;
        }

        // C=0
        public bool GetX1()
        {
            var subString = StaticMethods.GetSubstring(C, 16, 0);
            bool result = Convert.ToUInt32(subString, 2) == 0;
            return result;
        }

        // C[16]
        public bool GetX2()
        {
            var bit = StaticMethods.GetBit(C, 16);
            return bit == 1;
        }

        // СЧ=0
        public bool GetX3()
        {
            return CH == 0;
        }

        // B[0]
        public bool GetX4()
        {
            var bit = StaticMethods.GetBit(B, 0);
            return bit == 1;
        }

        // A[15]^B[16]
        public bool GetX5()
        {
            var A15 = StaticMethods.GetBit(A, 15) == 1;
            var B16 = StaticMethods.GetBit(B, 16) == 1;
            return A15 ^ B16;
        }

        // Возвращает массив логических условий
        public X_Array GetX()
        {
            return new X_Array ( GetX0(), GetX1(), GetX2(), GetX3(), GetX4(), GetX5() );
        }

        #endregion
    }
}
