﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TABZMoreGunsMod
{
    [Serializable]
    public struct ConfigInfo
    {
        public float CameraFOV;
        public float CameraRenderDistance;
        public float MinBrightness;

        public ConfigInfo(float CameraFOV, float CameraRenderDistance, float MinBrightness)
        {
            this.CameraFOV = CameraFOV;
            this.CameraRenderDistance = CameraRenderDistance;
            this.MinBrightness = MinBrightness;
        }

        public static ConfigInfo FromJson(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<ConfigInfo>(json);
        }

        public void ToJsonFile(string path)
        {
            string file = JsonUtility.ToJson(this);
            StreamWriter s = File.CreateText(path + "/TABZMoreGunsModConfig.json");
            s.Write(file);
            s.Flush();
            s.Close();
        }
    }
}