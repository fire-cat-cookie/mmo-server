using mmo_server.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mmo_server.Debug {
    class Debug {
        public enum MessageTypes {
            Default = 0, Login = 1
        }

        private static Debug instance;
        private Config config;

        public static void SetInstance(Config config) {
            instance = new Debug(config);
        }

        private Debug(Config config) {
            this.config = config;
        }

        public static void Log(string message, MessageTypes type) {
            if (instance != null) {
                instance._Log(message, type);
            } else {
                Console.Write("Warning: Tried to log a message, but no instance of logger was found");
            }
        }

        private void _Log(string message, MessageTypes type) {
            if (type == MessageTypes.Login && !config.debugMessages.Login) {
                return;
            }
            Console.WriteLine(message);
        }

    }
}
