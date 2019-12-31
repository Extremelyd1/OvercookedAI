using System;
using System.IO;
using UnityEngine;

namespace AI {

    class Logger {

        public static void Log(String message) {
            String str = File.ReadAllText("D:\\Programs\\Steam\\steamapps\\common\\Overcooked! 2\\Output.txt");
            File.WriteAllText("D:\\Programs\\Steam\\steamapps\\common\\Overcooked! 2\\Output.txt",
                str + message + "\n");
        }

        public static String FormatPosition(Vector3 location) {
            return "x=" + location.x + ", y=" + location.y + ", z=" + location.z;
        }

        public static void Clear() {
            File.WriteAllText("D:\\Programs\\Steam\\steamapps\\common\\Overcooked! 2\\Output.txt",
                "");
        }

    }
}
