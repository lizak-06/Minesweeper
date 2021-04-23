using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Minesweeper_form : Form
    {
        public Minesweeper_form()
        {
            InitializeComponent();

            Appearance.CreatingFiald(this);

        }
    }
}
