﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace management_system
{
    //form used to create a new group
    public partial class F4_NewGroup : Form
    {
        //VARIABLES
        private Image icon = null;

        //CONSTRUCTORS
        public F4_NewGroup()
        {
            InitializeComponent();
        }

        //UTILITY FUNCTIONS
        private bool validGroupName(string groupName)
        {

            if (groupName.Length > Utility.maxGroupNameLength) //group name too long
            {
                this.F4_errorProvider_newGroupForm.SetError(this.F4_textBox_groupName, Utility.displayError("Groups_invalid_group_name_too_long") + Utility.maxGroupNameLength.ToString());
                return false;
            }
            else if (groupName.Length == 0 || groupName == null || groupName == "") //no name given
            {
                this.F4_errorProvider_newGroupForm.SetError(this.F4_textBox_groupName, Utility.displayError("Groups_invalid_group_name_empty"));
                return false;
            }

            foreach (char c in groupName)
            {
                if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c < '0' || c > '9') && c != '_') //invalid character
                {
                    this.F4_errorProvider_newGroupForm.SetError(this.F4_textBox_groupName, Utility.displayError("Groups_invalid_character_in_group_name"));
                    return false;
                }
            }


            //no errors
            return true;


        }

        //EVENT HANDLERS

        //load form
        private void F4_NewGroup_Load(object sender, EventArgs e)
        {
            //form settings
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.StartPosition = FormStartPosition.CenterScreen;

            //load preferences
            Utility.setLanguage(this);
            Utility.setTheme(this);



        }

        //create the group
        private void F4_button_createNewGroup_Click(object sender, EventArgs e)
        {
            //check group name
            if (this.validGroupName(this.F4_textBox_groupName.Text) == true) //valid group name
            {
                try
                {
                    //if no icon has been chosen => set a default icon from a predefined location in the SYSTEM folder
                    if (this.icon == null)
                    {
                        this.icon = Image.FromFile(Utility.IMG_defaultIconFilePath);
                    }
                }
                catch (Exception exception)
                {
                    Utility.DisplayError("Groups_cannot_load_default_group_icon", exception, "Group: Failed to load the default group icon from the SYSTEM folder: " + exception.ToString(), false);
                }

                //create the group and add it into the database index table
                int result = Utility.createNewGroup(this.F4_textBox_groupName.Text, Utility.username, DateTime.Now, Utility.admin != null, this.icon);

                
                if(result==0) //group created
                {
                    MessageBox.Show(Utility.displayMessage("F4_newGroupCreated"), Utility.displayMessage("F4_newGroup_title"), MessageBoxButtons.OK,MessageBoxIcon.Information);

                    //send SYSTEM notification about the new group
                    Utility.sendSystemNotification(Utility.displayMessage("Notification_system_new_group")+this.F4_textBox_groupName.Text.ToString());
                
                }
                else if(result==-1) //group not created because there already exists another group with the same name
                {
                    MessageBox.Show(Utility.displayMessage("F4_newGroupNotCreated_duplicateName"), Utility.displayMessage("F4_newGroup_title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else //error encountered (value = -2)
                {
                    MessageBox.Show(Utility.displayMessage("F4_newGroupNotCreated_error"), Utility.displayMessage("F4_newGroup_title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            //else -> invalid group name, do not create group and set an error on the textbox control; errors set in the validGroupName() function

        }

        //select an icon for the new group
        private void F4_button_newGroupIcon_Click(object sender, EventArgs e)
        {
            //open the file dialog
            try
            {
                //dialog settings
                this.F4_openFileDialog_chooseNewGroupIcon.Filter = Utility.groupIconFileFilterString; //apply a file filter to the shown results
                this.F4_openFileDialog_chooseNewGroupIcon.Title = "Icon";

                //select file
                DialogResult fileDialogResult = this.F4_openFileDialog_chooseNewGroupIcon.ShowDialog(this);

                //store file filePath and name
                if (fileDialogResult == DialogResult.OK)
                {
                    string iconFilePath = this.F4_openFileDialog_chooseNewGroupIcon.FileName;

                    //load the icon into memory
                    this.icon = Image.FromFile(iconFilePath);




                    //store the image in a byte buffer
                    /*
                    Stream iconStream = File.Open(iconFilePath, FileMode.Open);
                    int input = 0, i = 0;
                    byte[] byteBuffer = new byte[4096];

                    //store the bytes into a byte array
                    do
                    {
                        input = iconStream.ReadByte();
                        byteBuffer[i] = (byte)input;
                        i++;
                    } while (input != -1);
                    */
                    //load the image into the database
                    //DEV
                }

            }
            catch (Exception exception)
            {

                MessageBox.Show(Utility.displayError("System_failed_to_select_new_group_icon") + exception.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Utility.logDiagnosticEntry("Warning: failed to open the new group icon file dialog: " + exception.ToString());
                Utility.WARNING = true;


            }
        }

    }
}
