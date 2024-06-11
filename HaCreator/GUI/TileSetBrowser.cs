﻿/* Copyright (C) 2015 haha01haha01

* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using HaCreator.CustomControls;
using System.Diagnostics;

namespace HaCreator.GUI
{
    public partial class TileSetBrowser : Form
    {
        private ListBox targetListBox;
        public ImageViewer selectedItem = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="target"></param>
        public TileSetBrowser(ListBox target)
        {
            InitializeComponent();
            targetListBox = target;

            Load += TileSetBrowser_Load;
        }

        /// <summary>
        /// On load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileSetBrowser_Load(object sender, EventArgs e) {
            foreach (KeyValuePair<string, WzImage> tS in Program.InfoManager.TileSets) {
                WzImage tSImage = Program.InfoManager.TileSets[tS.Key];
                if (!tSImage.Parsed) 
                    tSImage.ParseImage();
                WzImageProperty enh0 = tSImage["enH0"];
                if (enh0 == null)
                    continue;
                WzCanvasProperty image = (WzCanvasProperty)enh0["0"];
                if (image == null)
                    continue;

                Bitmap bitmap = image.GetLinkedWzCanvasBitmap();

                ImageViewer item = koolkLVContainer.Add(bitmap, tS.Key, true); // add to container and get back the ImageViewer object
                item.MouseDown += new MouseEventHandler(item_Click);
                item.MouseDoubleClick += new MouseEventHandler(item_DoubleClick);
            }
        }

        /// <summary>
        /// Tile item double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_DoubleClick(object sender, MouseEventArgs e)
        {
            if (selectedItem == null) 
                return;
            targetListBox.SelectedItem = selectedItem.Name;
            Close();
        }

        /// <summary>
        /// Tile itme click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_Click(object sender, MouseEventArgs e)
        {
            if (selectedItem != null)
                selectedItem.IsActive = false;
            selectedItem = (ImageViewer)sender;
            selectedItem.IsActive = true;
        }

        /// <summary>
        /// On keydown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileSetBrowser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
        }
    }
}
