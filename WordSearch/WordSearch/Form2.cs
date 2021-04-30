using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordSearch
{
    public partial class Form2 : Form
    {
        public Form2(int allWords, int wordsFounded)
        {
            InitializeComponent();
            label1.Text = wordsFounded + " founded out of " + allWords;
        }

        
    }
}
