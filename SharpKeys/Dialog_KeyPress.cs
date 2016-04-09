using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SharpKeys
{
	/// <summary>
	/// Summary description for Dialog_KeyPress.
	/// </summary>
	public partial class Dialog_KeyPress : System.Windows.Forms.Form, IMessageFilter
	{
    // passed in from the main form
    internal Hashtable m_hashKeys = null;

    // data handlers
    internal string m_strSelected = "";
    const string DISABLED_KEY = "Key is disabled\n(00_00)";

		public Dialog_KeyPress()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

      // required to activate the message hook for this dialog
      Application.AddMessageFilter(this);
		}

    private void Dialog_KeyPress_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
      // required to remove the message hook for this dialog
      Application.RemoveMessageFilter(this);
    }

    private void ShowKeyCode(int nCode) {
      // set up UI label
      if (lblPressed.Text.Length == 0)
        lblPressed.Text = "You pressed: ";

      nCode = nCode >> 16;
      if (nCode == 0) {
        lblKey.Text = DISABLED_KEY;
        btnOK.Enabled = false;
        return;
      }

      // get the code from LPARAM
      // if it's more than 256 then it's an extended key and mapped to 0xE0nn
      string strCode = "";
      if (nCode > 0x0100) {
        strCode = string.Format("E0_{0,2:X}", (nCode - 0x0100));
      }
      else {
        strCode = string.Format("00_{0,2:X}", nCode);
      }
      strCode = strCode.Replace(" ", "0");

      // Look up the scan code in the hashtable
      string strShow = "";
      if (m_hashKeys != null) {
        strShow = string.Format("{0}\n({1})", m_hashKeys[strCode], strCode);
      }
      else {
        strShow = "Scan code: " + strCode;
      }
      lblKey.Text = strShow;

      // UI to collect only valid scancodes
      btnOK.Enabled = true;
    }

    public bool PreFilterMessage(ref Message m) {
      if (m.Msg == 0x100) //0x100 == WM_KEYDOWN
        ShowKeyCode((int)m.LParam);
      // always return false because we're just watching messages; not
      // trapping them - this message comes from IMessageFilter!
      return false;
    }

    // button handlers - don't have to worry about null b/c they can't get to it
    private void btnOK_Click(object sender, System.EventArgs e) {
      this.AcceptButton = btnOK;
      m_strSelected = lblKey.Text.Replace("\n", " ");
    }

    private void btnCancel_Click(object sender, System.EventArgs e) {
      this.CancelButton = btnCancel;
      m_strSelected = "";
    }

    private void Dialog_KeyPress_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle,
                     Color.FromArgb(188, 188, 188), Color.FromArgb(225, 225, 225),
                     LinearGradientMode.ForwardDiagonal);

      graphics.FillRectangle(linearGradientBrush, rectangle);
    }

    private void Dialog_KeyPress_Resize(object sender, EventArgs e) {
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
