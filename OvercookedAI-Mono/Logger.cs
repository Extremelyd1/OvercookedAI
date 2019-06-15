using System;
using System.IO;
using UnityEngine;

namespace AI {

    class Logger {

        //private static Logger INSTANCE;

        //private Logger() {

        //}

        //public static Logger Get() {
        //    if (INSTANCE == null) {
        //        INSTANCE = new Logger();
        //    }

        //    return INSTANCE;
        //}

        public static void Log(String message) {
            String str = File.ReadAllText("D:\\Programs\\Steam\\steamapps\\common\\Overcooked! 2\\Output.txt");
            File.WriteAllText("D:\\Programs\\Steam\\steamapps\\common\\Overcooked! 2\\Output.txt",
                str + message + "\n");
        }

        public static String FormatPosition(Vector3 location) {
            return "x=" + location.x + ", y=" + location.y + ", z=" + location.z;
        }

    }
}
