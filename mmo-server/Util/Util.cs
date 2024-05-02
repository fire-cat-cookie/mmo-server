using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server {
    class Util {
        public static float Clamp(float value, float min, float max) {
            if (value >= min && value <= max) {
                return value;
            }
            else if (value < min) {
                return min;
            }
            else if (value > max) {
                return max;
            }
            return value;
        }
    }
}
