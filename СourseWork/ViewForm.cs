using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace СourseWork
{
    public partial class ViewForm : Form
    {
        RadioButton[] radioButtons;

        MachineLevel machine;

        bool machineEnded;

        MicroProgLevel microProg;
        public ViewForm()
        {
            InitializeComponent();
            radioButtons = new[] { radioButton0, radioButton1, radioButton2, radioButton3, radioButton4, radioButton5, radioButton6, radioButton7, radioButton8, radioButton9, radioButton10 };
            buttonGSA.AutoSize = false;
            buttonGSA.Paint += ButtonPaint;
            buttonYA.Paint += ButtonPaint;
            this.Text = string.Empty;
            this.ControlBox = false;
            dataGridView_A.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridView_B.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            registr_A_DGV.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            registr_B_DGV.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            registr_C_DGV.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            registr_CH_DGV.Rows.Add(0, 0, 0, 0);
            dataGridView_Q.Rows.Add(0, 0, 0, 0);
            dataGridView_D.Rows.Add(0, 0, 0, 0);
            dataGridView_decoder.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridView_Y.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridView_X.Rows.Add(0, 0, 0, 0, 0, 0);
            registr_PP_DGV.Rows.Add("0");
            dataGridView_A.Tag = label_A;
            dataGridView_B.Tag = label_B;
            registr_C_DGV.Tag = label_C;
        }

        #region Обработчики событий нажатий на кнопки

        // Закрытие приложения при клике по кнопке "Закрыть"
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Отображение ГСА при клике по кнопке "Микропрограмма"
        private void ButtonGSA_Click(object sender, EventArgs e)
        {
            panelGSA.Visible = true;
            panelYA.Visible = false;
            buttonGSA.BackColor = Color.FromArgb(0, 104, 124);
            buttonYA.BackColor = Color.FromArgb(0, 167, 199);
        }

        // Отображение ГСА при клике по кнопке "Взаимодействие УА с ОА"
        private void ButtonYA_Click(object sender, EventArgs e)
        {
            panelGSA.Visible = false;
            panelYA.Visible = true;
            buttonGSA.BackColor = Color.FromArgb(0, 167, 199);
            buttonYA.BackColor = Color.FromArgb(0, 104, 124);
        }

        // Запуск моделирования при клике по кнопке "Подтвердить"
        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            panelControl.Enabled = true;
            panelDataEnter.Enabled = false;
            if (radioButtonMP.Checked)
            {
                microProg = new(this);
            }
            else
            {
                machineEnded = false;
                machine = new(this);

            }
        }

        // Выполняет один шаг работу в зависимости от уровня моделирования
        private void StepButton_Click(object sender, EventArgs e)
        {
            if (radioButtonMP.Checked)
            {
                microProg.Step();
            }
            else
            {
                if (machineEnded) machine.Reset();
                else machine.Step();
            }
        }

        // Запуск таймера при клике по кнопке "Старт"
        private void StartButton_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        // Остановка таймера при клике по кнопке "Стоп"
        private void StopButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        // Сброс
        private void ResetButton_Click(object sender, EventArgs e)
        {
            End();
        }

        #endregion

        #region Методы работы с регистрами

        // Метод получения целочисленного значения А
        public uint GetA()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                stringBuilder.Append(dataGridView_A[i, 0].Value);
            }
            var result = Convert.ToUInt32(stringBuilder.ToString(), 2);
            return result;
        }

        // Метод получения целочисленного значения B
        public uint GetB()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(dataGridView_B[0, 0].Value);
            stringBuilder.Append('0');
            for (int i = 1; i < 16; i++)
            {
                stringBuilder.Append(dataGridView_B[i, 0].Value);
            }
            var result = Convert.ToUInt32(stringBuilder.ToString(), 2);
            return result;
        }

        // Метод обновления регистров
        public void UpdateRegisters(uint A, uint B, uint C, byte CH, bool PP)
        {
            UpdateRegistr(registr_A_DGV, StaticMethods.GetSubstring(A, 15, 0));
            UpdateRegistr(registr_B_DGV, StaticMethods.GetSubstring(B, 16, 0));
            UpdateRegistr(registr_C_DGV, StaticMethods.GetSubstring(C, 16, 0));
            UpdateRegistr(registr_CH_DGV, StaticMethods.GetSubstring(CH, 3, 0));
            UpdateRegistr(registr_PP_DGV, PP ? "1" : "0");
            SetNumber(registr_C_DGV);
        }

        // Метод обновления регистра
        void UpdateRegistr(DataGridView dataGridView, string values)
        {
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView[i, 0].Value = values[i];
            }
        }

        // Метод обновления метки состояния на ГСА
        public void UpdateGSA(int currestState)
        {
            for (int i = 0; i < radioButtons.Length; i++)
            {
                radioButtons[i].Checked = i == currestState;
            }
        }

        // Обновление структурной схемы автомата
        public void UpdateMachine(bool[] Q, bool[] D, bool[] a, bool[] y, X_Array x)
        {
            Array.Reverse(Q);
            Array.Reverse(D);
            UpdateElement(dataGridView_Q, Q);
            UpdateElement(dataGridView_decoder, a);
            UpdateElement(dataGridView_Y, y);
            UpdateElement(dataGridView_D, D);
            UpdateElement(dataGridView_X, new[] { x.X0, x.X1, x.X2, x.X3, x.X4, x.X5 });
        }

        // Обновление отдельной таблицы на схеме
        void UpdateElement(DataGridView dataGridView, bool[] array)
        {
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView[i, 0].Value = array[i] ? 1 : 0;
            }
        }

        #endregion

        #region Управление вычислением

        // Изменение режима работы
        private void StepRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            timer.Stop();
            stepButton.Enabled = StepRadioButton.Checked;
            startButton.Enabled = stopButton.Enabled = !StepRadioButton.Checked;
        }

        // Изменение интервала таймера по перемещении ползунка
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            timer.Interval = trackBar.Value * 100;
            delayLabel.Text = "Интервал: " + (double)trackBar.Value / 10 + " с.";
        }

        // Изменение значение в ячейке таблицы при клики по ней
        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (e.RowIndex == 0 && dataGridView is not null)
            {
                if (dataGridView[e.ColumnIndex, 0].Value.ToString() == "1")
                {
                    dataGridView[e.ColumnIndex, 0].Value = 0;
                }
                else
                {
                    dataGridView[e.ColumnIndex, 0].Value = 1;
                }
                SetNumber(dataGridView);
            }
        }

        // Изменение десятичного значения переменных
        private void SetNumber(DataGridView dataGridView)
        {
            var textBox = dataGridView.Tag as Label;
            if (textBox is not null)
            {
                var stringBuilder = new StringBuilder();
                for (int i = 1; i < 16; i++)
                {
                    stringBuilder.Append(dataGridView[i, 0].Value);
                }
                var number = Convert.ToInt16(stringBuilder.ToString(), 2);
                if (dataGridView[0, 0].Value.ToString() == "1")
                {
                    number *= -1;
                }
                textBox.Text = Math.Round(number / 32768d, 5).ToString();
            }
        }

        // Сброс выделения ячеек
        private void SelectionChanged(object sender, EventArgs e)
        {
            (sender as DataGridView)?.ClearSelection();
        }

        // Изменение цвета ячейки при смене значения в ней
        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (e.RowIndex == 0 && dataGridView is not null)
            {
                if (dataGridView[e.ColumnIndex, 0].Value.ToString() == "1")
                {
                    dataGridView[e.ColumnIndex, 0].Style.BackColor = Color.FromArgb(155, 207, 224);
                }
                else
                {
                    dataGridView[e.ColumnIndex, 0].Style.BackColor = Color.White;
                }
            }
        }

        // Переключение панелей при смене уровня моделирования
        private void LevelRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMP.Checked)
            {
                ButtonGSA_Click(null, null);
            }
            else
            {
                ButtonYA_Click(null, null);
            }
        }

        #endregion

        // Поворачивает текст на кнопках на 90 градусов 
        private void ButtonPaint(object sender, PaintEventArgs e)
        {
            var button = sender as Button;
            e.Graphics.Clear(button.BackColor);
            e.Graphics.RotateTransform(-90);
            SizeF textSize = e.Graphics.MeasureString(button.Text, button.Font);
            e.Graphics.TranslateTransform(-button.Height / 2, button.Width / 2);
            e.Graphics.DrawString(button.Text, button.Font, new SolidBrush(button.ForeColor), -(textSize.Width / 2), -(textSize.Height / 2));
        }

        // Синхронизация меток состояний на ГСА
        private void RadioButton0_CheckedChanged(object sender, EventArgs e)
        {
            radioButton11.Checked = radioButton0.Checked;
        }

        // Окончание работы
        public void End()
        {
            timer.Stop();
            panelDataEnter.Enabled = true;
            panelControl.Enabled = false;
        }

        // Окончение работы автомата
        public void MachineEnded()
        {
            machineEnded = true;
        }

    }
}