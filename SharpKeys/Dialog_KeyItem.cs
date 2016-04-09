using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SharpKeys
{
	/// <summary>
	/// Summary description for Dialog_KeyItem.
	/// </summary>
	public partial class Dialog_KeyItem : System.Windows.Forms.Form
	{
    // passed into here so it can be pushed through to type key
    internal Dictionary<string,string> m_hashKeys = null;

	    public Dialog_KeyItem(Dictionary<string,string> hashKeys) : this()
	    {
	        m_hashKeys = hashKeys;
	    }

		public Dialog_KeyItem()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

    private void btnFrom_Click(object sender, System.EventArgs e) {
      // Pop open the "typing" form to collect keyboard input to get a valid code
      Dialog_KeyPress dlg = new Dialog_KeyPress(m_hashKeys);
      if (dlg.ShowDialog() == DialogResult.OK) {
        if (lbFrom.Items.Contains(dlg.m_strSelected))
          lbFrom.SelectedItem = dlg.m_strSelected;
        else {
          // probably an international keyboard code
          MessageBox.Show("You've entered a key that SharpKeys doesn't know about.\n\nPlease check the SharpKeys website for an updated release", "SharpKeys");
        }
      }
    }

    private void btnTo_Click(object sender, System.EventArgs e) {
      // Pop open the "typing" form to collect keyboard input to get a valid code
      Dialog_KeyPress dlg = new Dialog_KeyPress(m_hashKeys);
      if (dlg.ShowDialog() == DialogResult.OK) {
        if (lbTo.Items.Contains(dlg.m_strSelected))
          lbTo.SelectedItem = dlg.m_strSelected;
        else {
          // probably an international keyboard code
          MessageBox.Show("You've entered a key that SharpKeys doesn't know about.\n\nPlease check the SharpKeys website for an updated release", "SharpKeys");
        }
      }
    }

    private void Dialog_KeyItem_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle,
                     Color.FromArgb(188, 188, 188), Color.FromArgb(225, 225, 225),
                     LinearGradientMode.ForwardDiagonal);

      graphics.FillRectangle(linearGradientBrush, rectangle);
    }

    private void Dialog_KeyItem_Resize(object sender, EventArgs e) {
      this.Invalidate();
    }

    private void mainPanel_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle rectangle = new Rectangle(0, 0, mainPanel.Width, mainPanel.Height);
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle,
                     Color.FromArgb(209, 221, 228), Color.FromArgb(237, 239, 247), //Color.FromArgb(236, 241, 243), 
                     LinearGradientMode.Vertical);

      graphics.FillRectangle(linearGradientBrush, rectangle);

    }
	}
}
