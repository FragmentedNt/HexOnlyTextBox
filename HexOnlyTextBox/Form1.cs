using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HexOnlyTextBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;  // 通常KeyDown後に発生するKeyPressイベントを発生させない
            var caret = textBox1.SelectionStart;  // キャレットの位置を取得(入力フォームの ’I’)
            var str = textBox1.Text;

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                // 削除対象(Delならstr[caret], BSならstr[caret-1])がスペースの場合2文字分消去することに留意
                if (e.KeyCode == Keys.Delete && caret < str.Count())
                {
                    if (str[caret] == ' ')
                    {
                        if (caret + 1 < str.Count())
                        {
                            str = str.Remove(caret, 2);
                        }
                    }
                    else
                    {
                        str = str.Remove(caret, 1);
                    }
                }
                else if (e.KeyCode == Keys.Back && caret > 0)
                {
                    if (str[caret - 1] == ' ')
                    {
                        if (caret - 1 > 0)
                        {
                            str = str.Remove(caret - 2, 2);
                            caret -= 2;
                        }
                    }
                    else
                    {
                        str = str.Remove(caret - 1, 1);
                        caret -= 1;
                    }
                }
            }
            else if ((Keys.D0 <= e.KeyCode && e.KeyCode <= Keys.D9)
                   || (Keys.A <= e.KeyCode && e.KeyCode <= Keys.F)
                   || (Keys.NumPad0 <= e.KeyCode && e.KeyCode <= Keys.NumPad9))
            {
                char keyChar;
                if ((Keys.D0 <= e.KeyCode && e.KeyCode <= Keys.D9)
                 || (Keys.A <= e.KeyCode && e.KeyCode <= Keys.F))
                    keyChar = (char)e.KeyCode;  // 0～Zの場合はキーコードとASCIIコードが同じなのでそのままキャスト
                else
                    keyChar = (char)(e.KeyCode - 48);  // NumPadの場合はASCIIコードの数値と合わせるために計算
                str = str.Insert(caret, keyChar.ToString());
                if (caret + 1 < str.Count())
                {
                    if (str[caret + 1] == ' ')
                        caret += 2;
                    else
                        caret += 1;
                }
                else
                {
                    caret += 1;
                }
            }
            else if (e.KeyCode == Keys.Left  // キャレットの移動に使う文字はKeyPressイベントを許可
                  || e.KeyCode == Keys.Right
                  || e.KeyCode == Keys.Home
                  || e.KeyCode == Keys.End)
            {
                e.SuppressKeyPress = false;
            }

            // 一度空白を削除し，2文字ごとに空白を挿入
            str = str.Replace(" ", "");
            List<char> istr = new List<char>();
            for (int i = 0; i < str.Count(); i++)
            {
                istr.Add(str[i]);
                if (i % 2 == 1)
                {
                    istr.Add(' ');
                }
            }

            textBox1.Text = new string(istr.ToArray());
            textBox1.SelectionStart = caret;

            updateToolStripLabel(e);
        }

        private void updateToolStripLabel(KeyEventArgs e)
        {
            // 直前に押下されたキーの確認
            if ((Keys.D0 <= e.KeyCode && e.KeyCode <= Keys.D9)
            || (Keys.A <= e.KeyCode && e.KeyCode <= Keys.F)
            || (Keys.NumPad0 <= e.KeyCode && e.KeyCode <= Keys.NumPad9))
            {
                char keyChar;
                if ((Keys.D0 <= e.KeyCode && e.KeyCode <= Keys.D9)
                 || (Keys.A <= e.KeyCode && e.KeyCode <= Keys.F))
                    keyChar = (char)e.KeyCode;
                else
                    keyChar = (char)(e.KeyCode - 48);

                toolStripStatusLabel1.Text = $"Pressed {keyChar} key";
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        toolStripStatusLabel1.Text = "Pressed Delete key";
                        break;
                    case Keys.Back:
                        toolStripStatusLabel1.Text = "Pressed BackSpace key";
                        break;
                    case Keys.Left:
                        toolStripStatusLabel1.Text = "Pressed Left key";
                        break;
                    case Keys.Right:
                        toolStripStatusLabel1.Text = "Pressed Right key";
                        break;
                    case Keys.Home:
                        toolStripStatusLabel1.Text = "Pressed Home key";
                        break;
                    case Keys.End:
                        toolStripStatusLabel1.Text = "Pressed End key";
                        break;
                    default:
                        toolStripStatusLabel1.Text = "Invalaide key";
                        break;
                }
            }

            // 入力された文字列がすべて2桁であるかの確認
            var res = textBox1.Text.Split(' ');
            if (res.Count() == 1)
            {
                toolStripStatusLabel2.Text = "Hex text is empty";
            }
            else if (res.Last().Count() == 1)
            {
                toolStripStatusLabel2.Text = "Enter the hex with 2 digits";
            }
            else if (res.Last().Count() == 0)
            {
                toolStripStatusLabel2.Text = "Hex text is available";
            }
            textBox2.Text = "";
            foreach(var s in res)
            {
                textBox2.Text += $"\"{s}\" ";
            }
        }
    }
}


