﻿/*Copyright(c) 2025, LastBattle https://github.com/lastbattle/Harepacker-resurrected

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using HaCreator.MapEditor;
using System;
using System.Windows.Forms;

namespace HaCreator.GUI.EditorPanels
{
    public partial class BlackBorderPanel : UserControl
    {
        private HaCreatorStateManager hcsm;

        public BlackBorderPanel()
        {
            InitializeComponent();
        }

        public void Initialize(HaCreatorStateManager hcsm)
        {
            this.hcsm = hcsm;
            this.hcsm.SetBlackBorderPanel(this);
        }

        public void UpdateBoardData()
        {
            var selectedBoard = hcsm.MultiBoard.SelectedBoard;
            if (selectedBoard == null)
                return; // No board selected 

            // Bottom
            if (selectedBoard.MapInfo.LBBottom != null && selectedBoard.MapInfo.LBBottom.Value != 0)
            {
                checkBox_bottom.Checked = true;
                numericUpDown_bottom.Value = selectedBoard.MapInfo.LBBottom.Value;
            }
            else
            {
                checkBox_bottom.Checked = false;
            }
            numericUpDown_bottom.Enabled = checkBox_bottom.Checked;

            // Top
            if (selectedBoard.MapInfo.LBTop != null && selectedBoard.MapInfo.LBTop.Value != 0)
            {
                checkBox_top.Checked = true;
                numericUpDown_top.Value = selectedBoard.MapInfo.LBTop.Value;
            }
            else
            {
                checkBox_top.Checked = false;
            }
            numericUpDown_top.Enabled = checkBox_top.Checked;

            // Side
            if (selectedBoard.MapInfo.LBSide != null && selectedBoard.MapInfo.LBSide.Value != 0)
            {
                checkBox_side.Checked = true;
                numericUpDown_side.Value = selectedBoard.MapInfo.LBSide.Value;
            }
            else
            {
                checkBox_side.Checked = false;
            }
            numericUpDown_side.Enabled = checkBox_side.Checked;
        }

        private void checkBox_top_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_top.Enabled = checkBox_top.Checked;

            if (!checkBox_top.Checked)
            {
                numericUpDown_top.Value = 0;

                var selectedBoard = hcsm.MultiBoard.SelectedBoard;
                if (selectedBoard == null)
                    return; // No board selected 
                selectedBoard.MapInfo.LBTop = 0;
            }
        }

        private void checkBox_bottom_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_bottom.Enabled = checkBox_bottom.Checked;

            if (!checkBox_bottom.Checked)
            {
                numericUpDown_bottom.Value = 0;

                var selectedBoard = hcsm.MultiBoard.SelectedBoard;
                if (selectedBoard == null)
                    return; // No board selected 
                selectedBoard.MapInfo.LBBottom = 0;
            }
        }

        private void checkBox_side_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_side.Enabled = checkBox_side.Checked;

            if (!checkBox_side.Checked)
            {
                numericUpDown_side.Value = 0;

                var selectedBoard = hcsm.MultiBoard.SelectedBoard;
                if (selectedBoard == null)
                    return; // No board selected 
                selectedBoard.MapInfo.LBSide = 0;
            }
        }

        /// <summary>
        /// On numericUpDown_bottom key up, update the selected board's bottom black border value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown_bottom_KeyUp(object sender, KeyEventArgs e)
        {
            var selectedBoard = hcsm.MultiBoard.SelectedBoard;
            if (selectedBoard == null)
                return; // No board selected 

            if (int.TryParse(numericUpDown_bottom.Text, out int newValue))
            {
                selectedBoard.MapInfo.LBBottom = checkBox_bottom.Checked ? newValue : null;
            }
        }

        /// <summary>
        /// On numericUpDown_top key up, update the selected board's top black border value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown_top_KeyUp(object sender, KeyEventArgs e)
        {
            var selectedBoard = hcsm.MultiBoard.SelectedBoard;
            if (selectedBoard == null)
                return; // No board selected 

            if (int.TryParse(numericUpDown_top.Text, out int newValue))
            {
                selectedBoard.MapInfo.LBTop = checkBox_top.Checked ? newValue : null;
            }
        }

        /// <summary>
        /// On numericUpDown_side key up, update the selected board's side black border value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown_side_KeyUp(object sender, KeyEventArgs e)
        {
            var selectedBoard = hcsm.MultiBoard.SelectedBoard;
            if (selectedBoard == null)
                return; // No board selected 

            if (int.TryParse(numericUpDown_side.Text, out int newValue))
            {
                selectedBoard.MapInfo.LBSide = checkBox_side.Checked ? newValue : null;
            }
        }
    }
}
