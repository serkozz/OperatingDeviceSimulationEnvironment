namespace СourseWork
{
    public record X_Array(bool X0, bool X1, bool X2, bool X3, bool X4, bool X5);

    class MachineLevel
    {
        // Память состояний 
        bool[] Q;

        // Вектор D сигналов управления состояниями разрядов ПС УА
        bool[] D;

        // Вектор выходных сигналов дешифратора
        bool[] a;

        // Вектор выходных сигналов Y
        bool[] y;

        // Номер текущего состояния автомата
        int currentState;

        ViewForm form;

        Registers registers;

        public MachineLevel(ViewForm form)
        {
            this.form = form;
            registers = new(form.GetA(), form.GetB());
            Q = new bool[4];
            D = new bool[4];
            a = new bool[11];
            y = new bool[14];
            Step();
        }

        // Один такт работы автомата
        public void Step()
        {
            Q = (bool[])D.Clone();
            bool x1 = registers.GetX1();
            bool x2 = registers.GetX2();
            DS();
            KSY(registers.GetX() with { X1 = x1, X2 = x2 });
            OA();
            KSD(registers.GetX() with { X1 = x1, X2 = x2 });
            form.UpdateRegisters(registers.A, registers.B, registers.C, registers.CH, registers.PP);
            form.UpdateGSA(currentState);
            form.UpdateMachine((bool[])Q.Clone(), (bool[])D.Clone(), a, y, registers.GetX());
            if (!(D[0] | D[1] | D[2] | D[3])) form.MachineEnded();
        }

        // Дешифратор сотояний
        void DS()
        {
            a = new bool[11];
            int current = 0;
            for (int i = 0, j = 1; i < D.Length; i++, j *= 2)
            {
                if (Q[i]) current += j;
            }
            a[current] = true;
            currentState = current;
        }

        // Комбинационная схема у
        void KSY(X_Array x_Array)
        {
            (bool x0, bool x1, bool x2, bool x3, bool x4, bool x5) = x_Array;
            y[0] = a[1] & x0 | a[2] & !x2;
            y[1] = a[0];
            y[2] = a[0];
            y[3] = a[0];
            y[4] = a[1] & !x0 & !x1 | a[4] | a[8] & !x3;
            y[5] = (a[2] | a[6]) & x2;
            y[6] = a[3] | a[6] & !x2 | a[7];
            y[7] = a[3];
            y[8] = a[3];
            y[9] = a[5];
            y[10] = a[6] & !x2 | a[7];
            y[11] = a[8] & x3;
            y[12] = a[9] & x4;
            y[13] = a[9] & !x4 & x5 | a[10] & x5;
        }

        // Операционный автомат
        void OA()
        {
            var a_temp = registers.A;
            if (y[0]) registers.Y0();
            if (y[1]) registers.Y1(a_temp);
            if (y[2]) registers.Y2();
            if (y[3]) registers.Y3();
            if (y[4]) registers.Y4();
            if (y[5]) registers.Y5();
            if (y[6]) registers.Y6();
            if (y[7]) registers.Y7();
            if (y[8]) registers.Y8();
            if (y[9]) registers.Y9();
            if (y[10]) registers.Y10();
            if (y[11]) registers.Y11();
            if (y[12]) registers.Y12();
            if (y[13]) registers.Y13();
        }

        // Комбинационная схема D
        void KSD(X_Array x_Array)
        {
            (bool x0, bool x1, bool x2, bool x3, bool x4, _) = x_Array;
            D[0] = a[0] | (a[2] | a[6]) & x2 | a[4] | a[8];
            D[1] = a[1] & !x0 & !x1 | (a[2] | a[6]) & x2 | a[5] | a[9] & x4;
            D[2] = a[3] | a[4] | a[5] | a[6] & x2 | a[8] & !x3;
            D[3] = a[6] & !x2 | a[7] | a[8] & x3 | a[9] & x4;
        }

        // Сброс
        public void Reset()
        {
            Q = new bool[4];
            D = new bool[4];
            a = new bool[11];
            a[0] = true;
            y = new bool[14];
            form.UpdateMachine((bool[])Q.Clone(), (bool[])D.Clone(), a, y, registers.GetX());
            form.UpdateGSA(0);
            form.End();
        }
    }
}