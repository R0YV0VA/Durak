using System;
using System.Windows.Forms;

namespace DurakWinForms
{
  public partial class CreateServer : Form
  {
    public CreateServer()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (serverNameTb.Text == String.Empty)
      {
        MessageBox.Show(@"Введіть ім'я сервера!");
        serverNameTb.Focus();
      }
      else if (NickTb.Text == String.Empty)
      {
        MessageBox.Show(@"Введіть нік!");
        NickTb.Focus();
      }
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}
