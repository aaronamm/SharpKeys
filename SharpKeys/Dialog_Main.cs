using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;

namespace SharpKeys
{
	/// <summary>
	/// Summary description for Dialog_Main.
	/// </summary>
	public partial class Dialog_Main : System.Windows.Forms.Form
	{
    // Field for saving window position
    private Rectangle m_rcWindow;

    // Field for registy storage
    private string m_strRegKey = "Software\\RandyRants\\SharpKeys";

    // Hashtable for tracking text to scan codes
    private Hashtable m_hashKeys = null;

    // Dirty flag (to see track if mappings have been saved)
    private bool m_bDirty = false;

		public Dialog_Main()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
    }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
      Application.EnableVisualStyles();
			Application.Run(new Dialog_Main());
		}

    private void LoadRegistrySettings() {
      // First load the window positions from registry
      RegistryKey regKey = Registry.CurrentUser.OpenSubKey(m_strRegKey);
      Rectangle rc = new Rectangle(10, 10, 750, 550);
      int nWinState = 0, nWarning=0;

      if (regKey != null) {
        // Load Window Pos
        nWinState = (int)regKey.GetValue("MainWinState", 0);
        rc.X = (int)regKey.GetValue("MainX", 10);
        rc.Y = (int)regKey.GetValue("MainY", 10);
        rc.Width = (int)regKey.GetValue("MainCX", 750);
        rc.Height = (int)regKey.GetValue("MainCY", 550);

        nWarning = (int)regKey.GetValue("ShowWarning", 0);
        regKey.Close();
      }

      if (nWarning == 0) {
        MessageBox.Show("Welcome to SharpKeys!\n\nThis application will add one key to your registry that allows you\nto change how certain keys on your keyboard will work.\n\nYou must be running Windows 2000, XP, 2003, Vista, 2008, or 7 for this to be supported and\nyou'll be using SharpKeys at your own risk!\n\nEnjoy!\nRandyRants.com", "SharpKeys");
      }

      // Set the WinPos
      m_rcWindow = rc;
      DesktopBounds = m_rcWindow;
      WindowState = (FormWindowState)nWinState;

      // now load the scan code map
      RegistryKey regScanMapKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Keyboard Layout");
      if (regScanMapKey != null) {
        byte[] bytes = (byte[])regScanMapKey.GetValue("Scancode Map");
        if (bytes == null) {
          regScanMapKey.Close();
          return;
        }
        // can skip the first 8 bytes as they are ALWAYS 0x00
        // the 9th byte is ALWAYS the total number of mappings (including the trailing null pointer)
        if (bytes.Length > 8) {
          int nTotal = Int32.Parse(bytes[8].ToString());
          for (int i=0; i < nTotal-1; i++) {
            // scan codes are stored in ToHi ToLo FromHi FromLo
            String strFrom, strFromCode, strTo, strToCode;
            strFromCode = string.Format("{0,2:X}_{1,2:X}", bytes[(i*4)+12+3], bytes[(i*4)+12+2]);
            strFromCode = strFromCode.Replace(" ", "0");
            strFrom = string.Format("{0} ({1})", (string)m_hashKeys[strFromCode], strFromCode);
            
            strToCode = string.Format("{0,2:X}_{1,2:X}", bytes[(i*4)+12+1], bytes[(i*4)+12+0]);
            strToCode = strToCode.Replace(" ", "0");
            strTo = string.Format("{0} ({1})", (string)m_hashKeys[strToCode], strToCode);
            
            ListViewItem lvI = lvKeys.Items.Add(strFrom);
            lvI.SubItems.Add(strTo);
          }
        }

        regScanMapKey.Close();
      }
    }

    private void SaveRegistrySettings() {
      // Only save the window position info on close; user is prompted to save mappings elsewhere
      RegistryKey regKey = Registry.CurrentUser.CreateSubKey(m_strRegKey);

      // Save Window Pos
      regKey.SetValue("MainWinState", (int) WindowState);
      regKey.SetValue("MainX", m_rcWindow.X);
      regKey.SetValue("MainY", m_rcWindow.Y);
      regKey.SetValue("MainCX", m_rcWindow.Width);
      regKey.SetValue("MainCY", m_rcWindow.Height);
      regKey.SetValue("ShowWarning", 1);
    }

    private void SaveMappingsToRegistry() {
      Cursor = Cursors.WaitCursor;

      // Open the key to save the scancodes
      RegistryKey regScanMapKey = Registry.LocalMachine.CreateSubKey("System\\CurrentControlSet\\Control\\Keyboard Layout");
      if (regScanMapKey != null) {
        int nCount = lvKeys.Items.Count;
        if (nCount <= 0) {
          // the second param is required; this will throw an exception if the value isn't found,
          // and it might not always be there (which is valid), so it's ok to ignore it
          regScanMapKey.DeleteValue("Scancode Map", false);
        }
        else {
          // create a new byte array that is:
          //   8 bytes that are always 00 00 00 00 00 00 00 00 (as is required)
          // + 4 bytes that are used for the count nn 00 00 00 (as is required)
          // + 4 bytes per mapping
          // + 4 bytes for the last mapping (required)
          byte[] bytes = new byte[8 + 4 + (4 * nCount) + 4];
          
          // skip first 8 (0-7)

          // set 8 to the count, plus the trailing null
          bytes[8] = Convert.ToByte(nCount + 1);

          // skip 9, 10, 11

          // add up the list
          for (int i=0; i < nCount; i++) {
            String str = lvKeys.Items[i].SubItems[1].Text;
            bytes[(i*4)+12+0] = Convert.ToByte(str.Substring(str.LastIndexOf("_")+1, 2), 16);
            bytes[(i*4)+12+1] = Convert.ToByte(str.Substring(str.LastIndexOf("(")+1, 2), 16);

            str = lvKeys.Items[i].Text;
            bytes[(i*4)+12+2] = Convert.ToByte(str.Substring(str.LastIndexOf("_")+1, 2), 16);
            bytes[(i*4)+12+3] = Convert.ToByte(str.Substring(str.LastIndexOf("(")+1, 2), 16);
          }

          // last 4 are 0's

          // dump to the registry
          regScanMapKey.SetValue("Scancode Map", bytes);
        }
        regScanMapKey.Close();
      }
      m_bDirty = false;
      Cursor = Cursors.Default;

      MessageBox.Show("Key Mappings have been successfully stored to the registry.\n\nPlease logout or reboot for these changes to take effect!", "SharpKeys");
    }

    private void AddMapping() {
      // max out the mapping at 104
      if (lvKeys.Items.Count >= 104) {
        MessageBox.Show("The maximum number of mappings SharpKeys supports is 16.\n\nPlease delete an existing mapping before adding a new one!", "SharpKeys");
        return;
      }

      // adding a new mapping, so prep the add dialog with all of the scancodes
      Dialog_KeyItem dlg = new Dialog_KeyItem();
      dlg.m_hashKeys = m_hashKeys; // passed into this dialog so it can go out to the next
      IDictionaryEnumerator iDic = m_hashKeys.GetEnumerator();
      while (iDic.MoveNext() == true) {
        string str = string.Format("{0} ({1})", iDic.Value, iDic.Key);
        dlg.lbFrom.Items.Add(str);
        dlg.lbTo.Items.Add(str);
      }

      // remove the null setting for "From" since you can never have a null key to map
      int nPos = 0;
      nPos = dlg.lbFrom.FindString("-- Turn Key Off (00_00)");
      if (nPos > -1)
        dlg.lbFrom.Items.RemoveAt(nPos);

      // Now remove any of the keys that have already been mapped in the list (can't double up on from's)
      for (int i=0; i<lvKeys.Items.Count; i++) {
        nPos = dlg.lbFrom.FindString(lvKeys.Items[i].Text);
        if (nPos > -1)
          dlg.lbFrom.Items.RemoveAt(nPos);
      }

      // let C# sort the lists
      dlg.lbFrom.Sorted = true;
      dlg.lbTo.Sorted = true;

      // UI stuff
      dlg.Text = "SharpKeys: Add New Key Mapping";
      dlg.lbFrom.SelectedIndex = 0;
      dlg.lbTo.SelectedIndex = 0;
      if (dlg.ShowDialog() == DialogResult.OK) {
        m_bDirty = true;

        // Add the list, as it's past inspection.
        ListViewItem lvI = lvKeys.Items.Add(dlg.lbFrom.Text);
        lvI.SubItems.Add(dlg.lbTo.Text);
        lvI.Selected = true;
      }
      lvKeys.Focus();
    }
    private void EditMapping() {
      // make sure something was selecting
      if (lvKeys.SelectedItems.Count <= 0) {
        MessageBox.Show("Please select a mapping to edit!", "SharpKeys");
        return;
      }

      // built the drop down lists no matter what
      Dialog_KeyItem dlg = new Dialog_KeyItem();
      dlg.m_hashKeys = m_hashKeys; // passed into this dialog so it can go out to the next
      IDictionaryEnumerator iDic = m_hashKeys.GetEnumerator();
      while (iDic.MoveNext() == true) {
        string str = string.Format("{0} ({1})", iDic.Value, iDic.Key);
        dlg.lbFrom.Items.Add(str);
        dlg.lbTo.Items.Add(str);
      }

      // remove the null setting for "From" since you can never have a null key to map
      int nPos = 0;
      nPos = dlg.lbFrom.FindString("-- Turn Key Off (00_00)");
      if (nPos > -1)
        dlg.lbFrom.Items.RemoveAt(nPos);

      // remove any of the existing from key mappings however, leave in the one that has currently
      // been selected!
      for (int i=0; i<lvKeys.Items.Count; i++) {
        nPos = dlg.lbFrom.FindString(lvKeys.Items[i].Text);
        if ((nPos > -1) && (lvKeys.Items[i].Text != lvKeys.SelectedItems[0].Text)) {
          dlg.lbFrom.Items.RemoveAt(nPos);
        }
      }

      // Let C# sort the lists
      dlg.lbFrom.Sorted = true;
      dlg.lbTo.Sorted = true;

      // as it's an edit, set the drop down lists to the current From value
      nPos = dlg.lbFrom.FindString(lvKeys.SelectedItems[0].Text);
      if (nPos > -1)
        dlg.lbFrom.SelectedIndex = nPos;
      else
        dlg.lbFrom.SelectedIndex = 0;

      // as it's an edit, set the drop down lists to the current To value
      nPos = dlg.lbTo.FindString(lvKeys.SelectedItems[0].SubItems[1].Text);
      if (nPos > -1)
        dlg.lbTo.SelectedIndex = nPos;
      else
        dlg.lbTo.SelectedIndex = 0;

      dlg.Text = "SharpKeys: Edit Key Mapping";
      if (dlg.ShowDialog() == DialogResult.OK) {
        m_bDirty = true;

        // update the select mapping item in the list view
        lvKeys.SelectedItems[0].Text = dlg.lbFrom.Text;
        lvKeys.SelectedItems[0].SubItems[1].Text = dlg.lbTo.Text;
      }
      lvKeys.Focus();
    }

    private void DeleteMapping() {
      // Pop a mapping out of the list view
      if (lvKeys.SelectedItems.Count <= 0) {
        MessageBox.Show("Please select a mapping to remove!", "SharpKeys");
        return;
      }

      lvKeys.Items.Remove(lvKeys.SelectedItems[0]);
      
      m_bDirty = true;
    }

    private void DeleteAllMapping() {
      // Since removing all is a big step, get a confirmation
      DialogResult dlgRes = MessageBox.Show("Deleting all will clear this list of key mapping but your registry will not be updated until you click \"Write to Registry\".\n\nDo you want to clear this list of key mappings?", "SharpKeys", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);
      if (dlgRes == DialogResult.No) {
        return;
      }

      // ...and then clean out the list
      m_bDirty = true;
      btnEdit.Enabled = true;
      btnDelete.Enabled = false;
      lvKeys.Items.Clear();
    }

    private void BuildParseTables() {
      if (m_hashKeys != null)
        return;

      // the hash table uses a string in the form of Hi_Lo scan code (in Hex values) 
      // that most sources say are scan codes.  The 00_00 will disable a key - everything else 
      // is pretty obvious.  There is a bit of a reverse lookup however, so labels changed here
      // need to be changed in a couple of other places
      m_hashKeys = new Hashtable();
      m_hashKeys.Add("00_00", "-- Turn Key Off");
      m_hashKeys.Add("00_01", "Special: Escape");
      m_hashKeys.Add("00_02", "Key: 1 !");
      m_hashKeys.Add("00_03", "Key: 2 @");
      m_hashKeys.Add("00_04", "Key: 3 #");
      m_hashKeys.Add("00_05", "Key: 4 $");
      m_hashKeys.Add("00_06", "Key: 5 %");
      m_hashKeys.Add("00_07", "Key: 6 ^");
      m_hashKeys.Add("00_08", "Key: 7 &");
      m_hashKeys.Add("00_09", "Key: 8 *");
      m_hashKeys.Add("00_0A", "Key: 9 (");
      m_hashKeys.Add("00_0B", "Key: 0 )");
      m_hashKeys.Add("00_0C", "Key: - _");
      m_hashKeys.Add("00_0D", "Key: = +");
      m_hashKeys.Add("00_0E", "Special: Backspace");
      m_hashKeys.Add("00_0F", "Special: Tab");

      m_hashKeys.Add("00_10", "Key: Q");
      m_hashKeys.Add("00_11", "Key: W");
      m_hashKeys.Add("00_12", "Key: E");
      m_hashKeys.Add("00_13", "Key: R");
      m_hashKeys.Add("00_14", "Key: T");
      m_hashKeys.Add("00_15", "Key: Y");
      m_hashKeys.Add("00_16", "Key: U");
      m_hashKeys.Add("00_17", "Key: I");
      m_hashKeys.Add("00_18", "Key: O");
      m_hashKeys.Add("00_19", "Key: P");
      m_hashKeys.Add("00_1A", "Key: [ {");
      m_hashKeys.Add("00_1B", "Key: ] }");
      m_hashKeys.Add("00_1C", "Special: Enter");
      m_hashKeys.Add("00_1D", "Special: Left Ctrl");
      m_hashKeys.Add("00_1E", "Key: A");
      m_hashKeys.Add("00_1F", "Key: S");

      m_hashKeys.Add("00_20", "Key: D");
      m_hashKeys.Add("00_21", "Key: F");
      m_hashKeys.Add("00_22", "Key: G");
      m_hashKeys.Add("00_23", "Key: H");
      m_hashKeys.Add("00_24", "Key: J");
      m_hashKeys.Add("00_25", "Key: K");
      m_hashKeys.Add("00_26", "Key: L");
      m_hashKeys.Add("00_27", "Key: ; :");
      m_hashKeys.Add("00_28", "Key: ' \"");
      m_hashKeys.Add("00_29", "Key: ` ~");
      m_hashKeys.Add("00_2A", "Special: Left Shift");
      m_hashKeys.Add("00_2B", "Key: \\ |");
      m_hashKeys.Add("00_2C", "Key: Z");
      m_hashKeys.Add("00_2D", "Key: X");
      m_hashKeys.Add("00_2E", "Key: C");
      m_hashKeys.Add("00_2F", "Key: V");

      m_hashKeys.Add("00_30", "Key: B");
      m_hashKeys.Add("00_31", "Key: N");
      m_hashKeys.Add("00_32", "Key: M");
      m_hashKeys.Add("00_33", "Key: , <");
      m_hashKeys.Add("00_34", "Key: . >");
      m_hashKeys.Add("00_35", "Key: / ?");
      m_hashKeys.Add("00_36", "Special: Right Shift");
      m_hashKeys.Add("00_37", "Num: *");
      m_hashKeys.Add("00_38", "Special: Left Alt");
      m_hashKeys.Add("00_39", "Special: Space");
      m_hashKeys.Add("00_3A", "Special: Caps Lock");
      m_hashKeys.Add("00_3B", "Function: F1");
      m_hashKeys.Add("00_3C", "Function: F2");
      m_hashKeys.Add("00_3D", "Function: F3");
      m_hashKeys.Add("00_3E", "Function: F4");
      m_hashKeys.Add("00_3F", "Function: F5");

      m_hashKeys.Add("00_40", "Function: F6");
      m_hashKeys.Add("00_41", "Function: F7");
      m_hashKeys.Add("00_42", "Function: F8");
      m_hashKeys.Add("00_43", "Function: F9");
      m_hashKeys.Add("00_44", "Function: F10");
      m_hashKeys.Add("00_45", "Special: Num Lock");
      m_hashKeys.Add("00_46", "Special: Scroll Lock");
      m_hashKeys.Add("00_47", "Num: 7");
      m_hashKeys.Add("00_48", "Num: 8");
      m_hashKeys.Add("00_49", "Num: 9");
      m_hashKeys.Add("00_4A", "Num: -");
      m_hashKeys.Add("00_4B", "Num: 4");
      m_hashKeys.Add("00_4C", "Num: 5");
      m_hashKeys.Add("00_4D", "Num: 6");
      m_hashKeys.Add("00_4E", "Num: +");
      m_hashKeys.Add("00_4F", "Num: 1");

      m_hashKeys.Add("00_50", "Num: 2");
      m_hashKeys.Add("00_51", "Num: 3");
      m_hashKeys.Add("00_52", "Num: 0");
      m_hashKeys.Add("00_53", "Num: .");
      m_hashKeys.Add("00_54", "Unknown: 0x0054");
      m_hashKeys.Add("00_55", "Unknown: 0x0055");
      m_hashKeys.Add("00_56", "Unknown: 0x0056");
      m_hashKeys.Add("00_57", "Function: F11");
      m_hashKeys.Add("00_58", "Function: F12");
      m_hashKeys.Add("00_59", "Unknown: 0x0059");
      m_hashKeys.Add("00_5A", "Unknown: 0x005A");
      m_hashKeys.Add("00_5B", "Unknown: 0x005B");
      m_hashKeys.Add("00_5C", "Unknown: 0x005C");
      m_hashKeys.Add("00_5D", "Unknown: 0x005D");
      m_hashKeys.Add("00_5E", "Unknown: 0x005E");
      m_hashKeys.Add("00_5F", "Unknown: 0x005F");

      m_hashKeys.Add("00_60", "Unknown: 0x0060");
      m_hashKeys.Add("00_61", "Unknown: 0x0061");
      m_hashKeys.Add("00_62", "Unknown: 0x0062");
      m_hashKeys.Add("00_63", "Unknown: 0x0063");
      m_hashKeys.Add("00_64", "Function: F13");
      m_hashKeys.Add("00_65", "Function: F14");
      m_hashKeys.Add("00_66", "Function: F15");
      m_hashKeys.Add("00_67", "Unknown: 0x0067");
      m_hashKeys.Add("00_68", "Unknown: 0x0068");
      m_hashKeys.Add("00_69", "Unknown: 0x0069");
      m_hashKeys.Add("00_6A", "Unknown: 0x006A");
      m_hashKeys.Add("00_6B", "Unknown: 0x006B");
      m_hashKeys.Add("00_6C", "Unknown: 0x006C");
      m_hashKeys.Add("00_6D", "Unknown: 0x006D");
      m_hashKeys.Add("00_6E", "Unknown: 0x006E");
      m_hashKeys.Add("00_6F", "Unknown: 0x006F");

      m_hashKeys.Add("00_70", "Unknown: 0x0070");
      m_hashKeys.Add("00_71", "Unknown: 0x0071");
      m_hashKeys.Add("00_72", "Unknown: 0x0072");
      m_hashKeys.Add("00_73", "Unknown: 0x0073");
      m_hashKeys.Add("00_74", "Unknown: 0x0074");
      m_hashKeys.Add("00_75", "Unknown: 0x0075");
      m_hashKeys.Add("00_76", "Unknown: 0x0076");
      m_hashKeys.Add("00_77", "Unknown: 0x0077");
      m_hashKeys.Add("00_78", "Unknown: 0x0078");
      m_hashKeys.Add("00_79", "Unknown: 0x0079");
      m_hashKeys.Add("00_7A", "Unknown: 0x007A");
      m_hashKeys.Add("00_7B", "Unknown: 0x007B");
      m_hashKeys.Add("00_7C", "Unknown: 0x007C");
      m_hashKeys.Add("00_7D", "Special: ¥ -");
      m_hashKeys.Add("00_7E", "Unknown: 0x007E");
      m_hashKeys.Add("00_7F", "Unknown: 0x007F");

      m_hashKeys.Add("E0_01", "Unknown: 0xE001");
      m_hashKeys.Add("E0_02", "Unknown: 0xE002");
      m_hashKeys.Add("E0_03", "Unknown: 0xE003");
      m_hashKeys.Add("E0_04", "Unknown: 0xE004");
      m_hashKeys.Add("E0_05", "Unknown: 0xE005");
      m_hashKeys.Add("E0_06", "Unknown: 0xE006");
      m_hashKeys.Add("E0_07", "F-Lock: Redo");        //   F3 - Redo
      m_hashKeys.Add("E0_08", "F-Lock: Undo"); //   F2 - Undo
      m_hashKeys.Add("E0_09", "Unknown: 0xE009");
      m_hashKeys.Add("E0_0A", "Unknown: 0xE00A");
      m_hashKeys.Add("E0_0B", "Unknown: 0xE00B");
      m_hashKeys.Add("E0_0C", "Unknown: 0xE00C");
      m_hashKeys.Add("E0_0D", "Unknown: 0xE00D");
      m_hashKeys.Add("E0_0E", "Unknown: 0xE00E");
      m_hashKeys.Add("E0_0F", "Unknown: 0xE00F");

      m_hashKeys.Add("E0_10", "Media: Prev Track");
      m_hashKeys.Add("E0_11", "App: Messenger");
      m_hashKeys.Add("E0_12", "Logitech: Webcam");
      m_hashKeys.Add("E0_13", "Logitech: iTouch");
      m_hashKeys.Add("E0_14", "Logitech: Shopping");
      m_hashKeys.Add("E0_15", "Unknown: 0xE015");
      m_hashKeys.Add("E0_16", "Unknown: 0xE016");
      m_hashKeys.Add("E0_17", "Unknown: 0xE017");
      m_hashKeys.Add("E0_18", "Unknown: 0xE018");
      m_hashKeys.Add("E0_19", "Media: Next Track");
      m_hashKeys.Add("E0_1A", "Unknown: 0xE01A");
      m_hashKeys.Add("E0_1B", "Unknown: 0xE01B");
      m_hashKeys.Add("E0_1C", "Num: Enter");
      m_hashKeys.Add("E0_1D", "Special: Right Ctrl");
      m_hashKeys.Add("E0_1E", "Unknown: 0xE01E");
      m_hashKeys.Add("E0_1F", "Unknown: 0xE01F");

      m_hashKeys.Add("E0_20", "Media: Mute");
      m_hashKeys.Add("E0_21", "App: Calculator");
      m_hashKeys.Add("E0_22", "Media: Play/Pause");
      m_hashKeys.Add("E0_23", "F-Lock: Spell");       //   F10
      m_hashKeys.Add("E0_24", "Media: Stop");
      m_hashKeys.Add("E0_25", "Unknown: 0xE025");
      m_hashKeys.Add("E0_26", "Unknown: 0xE026");
      m_hashKeys.Add("E0_27", "Unknown: 0xE027");
      m_hashKeys.Add("E0_28", "Unknown: 0xE028");
      m_hashKeys.Add("E0_29", "Unknown: 0xE029");
      // m_hashKeys.Add("E0_2A", "Special: PrtSc");   // removed due to conflict
      m_hashKeys.Add("E0_2B", "Unknown: 0xE02B");
      m_hashKeys.Add("E0_2C", "Unknown: 0xE02C");
      m_hashKeys.Add("E0_2D", "Unknown: 0xE02D");
      m_hashKeys.Add("E0_2E", "Media: Volume Down");
      m_hashKeys.Add("E0_2F", "Unknown: 0xE02F");

      m_hashKeys.Add("E0_30", "Media: Volume Up");
      m_hashKeys.Add("E0_31", "Unknown: 0xE031");
      m_hashKeys.Add("E0_32", "Web: Home");
      m_hashKeys.Add("E0_33", "Unknown: 0xE033");
      m_hashKeys.Add("E0_34", "Unknown: 0xE034");
      m_hashKeys.Add("E0_35", "Num: /");
      m_hashKeys.Add("E0_36", "Unknown: 0xE036");
      m_hashKeys.Add("E0_37", "Special: PrtSc"); 
      m_hashKeys.Add("E0_38", "Special: Right Alt");
      m_hashKeys.Add("E0_39", "Unknown: 0xE039");
      m_hashKeys.Add("E0_3A", "Unknown: 0xE03A");
      m_hashKeys.Add("E0_3B", "F-Lock: Help");        //   F1
      m_hashKeys.Add("E0_3C", "F-Lock: Office Home"); //   F2 - Office Home
      m_hashKeys.Add("E0_3D", "F-Lock: Task Pane");   //   F3 - Task pane
      m_hashKeys.Add("E0_3E", "F-Lock: New");         //   F4
      m_hashKeys.Add("E0_3F", "F-Lock: Open");        //   F5
      
      m_hashKeys.Add("E0_40", "F-Lock: Close");       //   F6
      m_hashKeys.Add("E0_41", "F-Lock: Reply");       //   F7
      m_hashKeys.Add("E0_42", "F-Lock: Fwd");         //   F8
      m_hashKeys.Add("E0_43", "F-Lock: Send");        //   F9
      m_hashKeys.Add("E0_44", "Unknown: 0xE044");
      m_hashKeys.Add("E0_45", "Special: €");        //   Euro
      m_hashKeys.Add("E0_46", "Unknown: 0xE046");
      m_hashKeys.Add("E0_47", "Special: Home");
      m_hashKeys.Add("E0_48", "Arrow: Up");
      m_hashKeys.Add("E0_49", "Special: Page Up");
      m_hashKeys.Add("E0_4A", "Unknown: 0xE04A");
      m_hashKeys.Add("E0_4B", "Arrow: Left");
      m_hashKeys.Add("E0_4C", "Unknown: 0xE04C");
      m_hashKeys.Add("E0_4D", "Arrow: Right");
      m_hashKeys.Add("E0_4E", "Unknown: 0xE04E");
      m_hashKeys.Add("E0_4F", "Special: End");

      m_hashKeys.Add("E0_50", "Arrow: Down");
      m_hashKeys.Add("E0_51", "Special: Page Down");
      m_hashKeys.Add("E0_52", "Special: Insert");
      m_hashKeys.Add("E0_53", "Special: Delete");
      m_hashKeys.Add("E0_54", "Unknown: 0xE054");
      m_hashKeys.Add("E0_55", "Unknown: 0xE055");
      m_hashKeys.Add("E0_56", "Unknown: 0xE056");
      m_hashKeys.Add("E0_57", "F-Lock: Save");        //   F11
      m_hashKeys.Add("E0_58", "F-Lock: Print");       //   F12
      m_hashKeys.Add("E0_59", "Unknown: 0xE059");
      m_hashKeys.Add("E0_5A", "Unknown: 0xE05A");
      m_hashKeys.Add("E0_5B", "Special: Left Windows");
      m_hashKeys.Add("E0_5C", "Special: Right Windows");
      m_hashKeys.Add("E0_5D", "Special: Application");
      m_hashKeys.Add("E0_5E", "Special: Power");
      m_hashKeys.Add("E0_5F", "Special: Sleep");
      
      m_hashKeys.Add("E0_61", "Unknown: 0xE061");
      m_hashKeys.Add("E0_62", "Unknown: 0xE062");
      m_hashKeys.Add("E0_63", "Special: Wake (or Fn)");
      m_hashKeys.Add("E0_64", "Unknown: 0xE064");
      m_hashKeys.Add("E0_65", "Web: Search");
      m_hashKeys.Add("E0_66", "Web: Favorites");
      m_hashKeys.Add("E0_67", "Web: Refresh");
      m_hashKeys.Add("E0_68", "Web: Stop");
      m_hashKeys.Add("E0_69", "Web: Forward");
      m_hashKeys.Add("E0_6A", "Web: Back");
      m_hashKeys.Add("E0_6B", "App: My Computer");
      m_hashKeys.Add("E0_6C", "App: E-Mail");
      m_hashKeys.Add("E0_6D", "App: Media Select");
      m_hashKeys.Add("E0_6E", "Unknown: 0xE06E");
      m_hashKeys.Add("E0_6F", "Unknown: 0xE06F");
    }

    
    // Dialog related events and overrides
    private void Dialog_Main_Load(object sender, System.EventArgs e) {
      Cursor = Cursors.WaitCursor;

      // Set up the hashtable and load the registy settings
      BuildParseTables();
      LoadRegistrySettings();

      // UI tweaking
      if (lvKeys.Items.Count > 0) {
        lvKeys.Items[0].Selected = true;
      }
      Cursor = Cursors.Default;
    }

    private void Dialog_Main_Closing(object sender, CancelEventArgs e) {
      // if anything has been added, edit'd or delete'd, ask if a save to the registry should be performed
      if (m_bDirty) {
        DialogResult dlgRes = MessageBox.Show("You have made changes to the list of key mappings.\n\nDo you want to update the registry now?", "SharpKeys", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button3);
        if (dlgRes == DialogResult.Cancel) {
          e.Cancel = true;
          return;
        }
        else if (dlgRes == DialogResult.Yes) {
          // update the registry
          SaveMappingsToRegistry();
        }
      }
      SaveRegistrySettings();
    }

    protected override void OnMove(EventArgs e) {
      base.OnMove(e);

      // save the current window position/size whenever moved
      if (WindowState == FormWindowState.Normal) {
        m_rcWindow = DesktopBounds;
      }
    }

    protected override void OnResize(EventArgs e) {
      base.OnResize(e);

      // resize the listview columns whenever sizeds
      lvcFrom.Width = lvKeys.Width/2 - 2;
      lvcTo.Width = lvcFrom.Width - 2;

      // save the current window position/size whenever moved
      if (WindowState == FormWindowState.Normal) {
        m_rcWindow = DesktopBounds;
      }
    }

    
    // Other Events
    private void lvKeys_SelectedIndexChanged(object sender, System.EventArgs e) {
      // UI stuff (to prevent editing or deleting a non-item
      if (lvKeys.SelectedItems.Count <= 0) {
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;
      }
      else {
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;
      }
    }

    private void mnuPop_Popup(object sender, System.EventArgs e) {
      // UI stuff (to prevent editing or deleting a non-item
      if (lvKeys.SelectedItems.Count <= 0) {
        mniEdit.Enabled = false;
        mniDelete.Enabled = false;
      }
      else {
        mniEdit.Enabled = true;
        mniDelete.Enabled = true;
      }
    }

    private void btnClose_Click(object sender, System.EventArgs e) {
      this.Close();
    }
    private void btnAdd_Click(object sender, System.EventArgs e) {
      AddMapping();
    }

    private void mniAdd_Click(object sender, System.EventArgs e) {
      AddMapping();
    }

    private void btnEdit_Click(object sender, System.EventArgs e) {
      EditMapping();
    }

    private void mniEdit_Click(object sender, System.EventArgs e) {
      EditMapping();
    }

    private void lvKeys_DoubleClick(object sender, System.EventArgs e) {
      EditMapping();
    }

    private void btnDelete_Click(object sender, System.EventArgs e) {
      DeleteMapping();
    }

    private void mniDelete_Click(object sender, System.EventArgs e) {
      DeleteMapping();    
    }

    private void btnDeleteAll_Click(object sender, System.EventArgs e) {
      DeleteAllMapping();
    }

    private void mniDeleteAll_Click(object sender, System.EventArgs e) {
      DeleteAllMapping();
    }

    private void btnSave_Click(object sender, System.EventArgs e) {
      SaveMappingsToRegistry();
    }

    private void urlMain_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {
      // open the home page
      System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
    }

    private void Dialog_Main_Resize(object sender, EventArgs e) {
      this.Invalidate();
    }

    private void Dialog_Main_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle,
                     Color.FromArgb(188, 188, 188), Color.FromArgb(225, 225, 225),
                     LinearGradientMode.ForwardDiagonal);

      graphics.FillRectangle(linearGradientBrush, rectangle);
    }

    private void mainPanel_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle rectangle = new Rectangle(0, 0, mainPanel.Width, mainPanel.Height);
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle,
                     Color.FromArgb(209, 221, 228), Color.FromArgb(237, 239, 247), //Color.FromArgb(236, 241, 243), 
                     LinearGradientMode.Vertical);

      graphics.FillRectangle(linearGradientBrush, rectangle);
    }

    private void headerPanel_Paint(object sender, PaintEventArgs e) {
      Graphics graphics = e.Graphics;

      Rectangle topRectangle = new Rectangle(0, 0, headerPanel.Width, headerPanel.Height / 2);
      Rectangle bottomRectangle = new Rectangle(0, topRectangle.Height, headerPanel.Width, headerPanel.Height - topRectangle.Height);
      LinearGradientBrush topGradientBrush = new LinearGradientBrush(topRectangle,
                     Color.FromArgb(165, 182, 206), Color.FromArgb(37, 81, 142),
                     LinearGradientMode.Vertical);

      LinearGradientBrush bottomGradientBrush = new LinearGradientBrush(bottomRectangle,
                     Color.FromArgb(13, 37, 90), Color.FromArgb(39, 37, 160),
                     LinearGradientMode.Vertical);

      graphics.FillRectangle(topGradientBrush, topRectangle);
      graphics.FillRectangle(bottomGradientBrush, bottomRectangle);

    }
  }
}
