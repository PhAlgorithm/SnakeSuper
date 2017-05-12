using System;
using System.Windows.Forms;

namespace SnakeSuper
{
    public partial class FormStart : Form
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormStart());
        }
        
        public FormStart()
        {
            InitializeComponent();
        }

        private void buttonRules_Click(object sender, EventArgs e)
        {
         MessageBox.Show(
@"Правила такие: необходимо с помощью <стрелочек> управлять телом змеи.
Необходимо собирать красные яблока.С каждым съеденым яблоком увеличивается длина змейки.
После каждых собранных 5 яблок увеличивается скорость смейки. В игре предусмотренно 3
уровня игры: от новичка (уровень 1) до профи (уровень 3).Для уровня < 3 > необходимо
обходить желтые преграды, т.к.при прохождении змеи через преграду игра заканчивается проиграшем.
Для выхода из игры необхотимо нажать клавишу<Escape>", "Правила");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStart.Text == "Level 1")          //используется для выбора уровня в форме Игра
            {
                FormGame.AFFF = 1;
            }
            if (cbStart.Text == "Level 2")
            {
                FormGame.AFFF = 2;
            }
            if (cbStart.Text == "Level 3")
            {
                FormGame.AFFF = 3;
            }

        }
      
        private void buttonStart_Click(object sender, EventArgs e)
        {
            Hide();
            FormGame fg = new FormGame();
            fg.ShowDialog();
            
        }
               
    }
}
