using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Behave
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      this.InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      var a = BehaviourRepository.Instance.GetMatching(this);
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      var f = new Form2();
      f.Show(this);
    }
  }
}
