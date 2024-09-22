﻿/*Copyright(c) 2024, LastBattle https://github.com/lastbattle/Harepacker-resurrected

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

using MapleLib.WzLib.WzStructure;
using MapleLib.WzLib;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HaCreator.MapEditor.Instance.Shapes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Threading;
using MapleLib.WzLib.WzProperties;
using MapleLib.WzLib.WzStructure.Data.ItemStructure;

namespace HaCreator.GUI.InstanceEditor
{
    public partial class LoadItemSelector : Form
    {
        private bool _isLoading = false;
        private bool _bItemsLoaded = false;

        private int _filterItemId = 0; // 0 = no filter, 243 = _itemId / 10000

        // dictionary
        private readonly List<string> itemNames = new List<string>(); // cache


        private int _selectedItemId = 0;
        private bool _bNotUserClosing = false;

        /// <summary>
        /// The selected itemId in the listbox
        /// </summary>
        public int SelectedItemId
        {
            get { return _selectedItemId; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filterItemId"></param>
        public LoadItemSelector(int filterItemId)
        {
            InitializeComponent();

            LoadSearchHelper searchBox = new LoadSearchHelper(listBox_itemList, itemNames);
            this.searchBox.TextChanged += searchBox.TextChanged;

            this.FormClosing += LoadQuestSelector_FormClosing;

            this._filterItemId = filterItemId;

            // load items
            load();
        }

        #region Window

        /// <summary>
        /// Loads the item on start of the window
        /// </summary>
        private void load()
        {
            _isLoading = true;
            try
            {
                // Maps
                foreach (KeyValuePair<int, Tuple<string, string, string>> item in Program.InfoManager.ItemNameCache) // itemid, <item category, item name, item desc>
                {
                    int itemId = item.Key;
                    string itemCategory = item.Value.Item1;
                    string itemName = item.Value.Item2;
                    string itemDesc = item.Value.Item3;

                    if (_filterItemId != 0 && (itemId / 10000 != _filterItemId)) // filters for item category
                        continue;

                    if (ItemIdsCategory.IsEquipment(itemId))
                    {
                        //WzImage eqpImg = Program.InfoManager.GetItemEquipSubProperty(itemId, itemCategory, Program.WzManager);
                        //if (eqpImg != null)
                        //{
                        string combinedId_ItemName = string.Format("[{0}] - ({1}) {2}", itemId, itemCategory, itemName);

                        itemNames.Add(combinedId_ItemName);
                        //}
                    }
                    else
                    {
                        if (Program.InfoManager.ItemIconCache.ContainsKey(itemId))
                        {
                            string combinedId_ItemName = string.Format("[{0}] - ({1}) {2}", itemId, itemCategory, itemName);

                            itemNames.Add(combinedId_ItemName);
                        }
                    }
                }
                itemNames.Sort();

                object[] itemObjs = itemNames.Cast<object>().ToArray();
                listBox_itemList.Items.AddRange(itemObjs);
            }
            finally
            {
                _isLoading = false;
                _bItemsLoaded = true;
            }
        }

        private void Load_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _selectedItemId = 0;
                Close(); // close window
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //loadButton_Click(null, null);
            }
        }

        /// <summary>
        /// The form is being closed by the user (e.g., clicking the X button)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadQuestSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_bNotUserClosing)
            {
                _selectedItemId = 0;
            }
        }
        #endregion

        /// <summary>
        /// On list box selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading || listBox_itemList.SelectedItem == null)
                return;

            string selectedItem = listBox_itemList.SelectedItem as string;

            const string pattern = @"\[(\d+)\]"; //  "[123] - SampleItem"
            Match match = Regex.Match(selectedItem, pattern);

            if (match.Success)
            {
                string itemId = match.Groups[1].Value;

                int intName = 0;
                int.TryParse(itemId, out intName);

                if (intName != 0)
                {
                    Tuple<string, string, string> itemInfo = Program.InfoManager.ItemNameCache[intName]; // // itemid, <item category, item name, item desc>

                    if (ItemIdsCategory.IsEquipment(intName))
                    {
                        WzImage eqpImg = Program.InfoManager.GetItemEquipSubProperty(intName, itemInfo.Item1, Program.WzManager);
                        if (eqpImg != null)
                            pictureBox_IconPreview.Image = ((WzCanvasProperty)eqpImg["info"]?["icon"]).GetLinkedWzCanvasBitmap();
                        else
                            pictureBox_IconPreview.Image = null;
                    }
                    else
                    {
                        if (Program.InfoManager.ItemIconCache.ContainsKey(intName))
                            pictureBox_IconPreview.Image = Program.InfoManager.ItemIconCache[intName].GetLinkedWzCanvasBitmap();
                        else
                            pictureBox_IconPreview.Image = null;
                    }

                    if (Program.InfoManager.ItemNameCache.ContainsKey(intName))
                        label_itemDesc.Text = Program.InfoManager.ItemNameCache[intName].Item3;
                    else
                        label_itemDesc.Text = string.Empty;

                    // set selected itemid
                    this._selectedItemId = intName;
                    this.button_select.Enabled = true;
                    return;
                }
            }
            this._selectedItemId = 0;
            button_select.Enabled = false;
        }

        private void listBox_itemList_measureItem(object sender, MeasureItemEventArgs e)
        {
            //e.ItemHeight = (int)e.Graphics.MeasureString(listBox_itemList.Items[e.Index].ToString(), listBox_itemList.Font, listBox_itemList.Width).Height;
        }

        private void listBox_itemList_drawItem(object sender, DrawItemEventArgs e)
        {
            //e.DrawBackground();
            //e.DrawFocusRectangle();

            //e.Graphics.DrawString(listBox_itemList.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        /// <summary>
        /// On select button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_select_Click(object sender, EventArgs e)
        {
            _bNotUserClosing = true;
            Close();
        }
    }
}
