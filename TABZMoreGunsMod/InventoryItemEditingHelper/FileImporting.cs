using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TABZMoreGunsMod.InventoryItemEditingHelper
{
    public class FileImporting
    {
        public static Texture2D ImportImage(string filePath)
        {
            byte[] imageBytes = File.ReadAllBytes(filePath);
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(imageBytes);
            return image;
        }
    }
}
