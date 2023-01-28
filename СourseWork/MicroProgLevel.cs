namespace СourseWork
{
    class MicroProgLevel
    {
        int currentState = 0;

        readonly ViewForm form;

        readonly Registers registers;

        public MicroProgLevel(ViewForm form)
        {
            this.form = form;
            registers = new(form.GetA(), form.GetB());
        }

        // Выполняет один шаг работы микропрограммы
        public void Step()
        {
            switch (currentState)
            {
                case 0:
                    registers.Y1(registers.A);
                    registers.Y2();
                    registers.Y3();
                    currentState = 1;
                    break;

                case 1:
                    if (registers.GetX0())
                    {
                        registers.Y0();
                        currentState = 0;
                    }
                    else if (registers.GetX1())
                    {
                        currentState = 0;
                    }
                    else
                    {
                        registers.Y4();
                        currentState = 2;
                    }
                    break;

                case 2:
                    if (registers.GetX2())
                    {
                        registers.Y5();
                        currentState = 3;
                    }
                    else
                    {
                        registers.Y0();
                        currentState = 0;
                    }
                    break;

                case 3:
                    registers.Y6();
                    registers.Y7();
                    registers.Y8();
                    currentState = 4;
                    break;

                case 4:
                    registers.Y4();
                    currentState = 5;
                    break;

                case 5:
                    registers.Y9();
                    currentState = 6;
                    break;

                case 6:
                    if (registers.GetX2())
                    {
                        registers.Y5();
                        currentState = 7;
                    }
                    else
                    {
                        goto case 7;
                    }
                    break;

                case 7:
                    registers.Y6();
                    registers.Y10();
                    currentState = 8;
                    break;

                case 8:
                    if (registers.GetX3())
                    {
                        registers.Y11();

                        currentState = 9;
                    }
                    else
                    {
                        registers.Y4();
                        currentState = 5;
                    }
                    break;

                case 9:
                    if (registers.GetX4())
                    {
                        registers.Y12();
                        currentState = 10;
                    }
                    else
                    {
                        goto case 10;
                    }
                    break;

                case 10:
                    if (registers.GetX5())
                    {
                        registers.Y13();
                    }
                    currentState = 0;
                    break;
            }

            form.UpdateRegisters(registers.A, registers.B, registers.C, registers.CH, registers.PP);
            form.UpdateGSA(currentState);
            if (currentState == 0) form.End();
        }
    }
}